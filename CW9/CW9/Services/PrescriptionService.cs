using CW9.Data;
using CW9.DTOs;
using CW9.Exceptions;
using CW9.Models;
using Microsoft.EntityFrameworkCore;

namespace CW9.Services;

public interface IPrescriptionService
{
    public Task<int> AddPrescriptionAsync(PrescriptionPostDto prescriptionPostDto);
    public Task<PatientGetDto> GetPatientByIdAsync(int idPatient);
}

public class PrescriptionService(AppDbContext context) : IPrescriptionService
{
    public async Task<int> AddPrescriptionAsync(PrescriptionPostDto prescriptionPostDto)
    {
        var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            if (prescriptionPostDto.DueDate < prescriptionPostDto.Date)
            {
                throw new PatientException("Due date cannot be earlier than prescription date.");
            }

            if (prescriptionPostDto.Medicaments.Count > 10)
            {
                throw new PatientException("Cannot add more than 10 medicaments.");
            }

            var medicamentsIds = await context.Medicaments.Select(medicament => medicament.IdMedicament)
                .ToListAsync();

            foreach (var medicamentDto in prescriptionPostDto.Medicaments)
            {
                if (!medicamentsIds.Contains(medicamentDto.IdMedicament))
                {
                    throw new NotFoundException("Medicament does not exist.");
                }
            }

            var patient = await context.Patients.FindAsync(prescriptionPostDto.PatientPostDto.IdPatient);
            if (patient == null)
            {
                patient = new Patient
                {
                    FirstName = prescriptionPostDto.PatientPostDto.FirstName,
                    LastName = prescriptionPostDto.PatientPostDto.LastName,
                    BirthDate = prescriptionPostDto.PatientPostDto.BirthDate,
                    Prescriptions = new List<Prescription>()
                };
                await context.Patients.AddAsync(patient);
                await context.SaveChangesAsync();
            }

            var prescription = new Prescription
            {
                Date = prescriptionPostDto.Date,
                DueDate = prescriptionPostDto.DueDate,
                IdDoctor = prescriptionPostDto.IdDoctor,
                IdPatient = prescriptionPostDto.PatientPostDto.IdPatient
            };

            await context.Prescriptions.AddAsync(prescription);
            await context.SaveChangesAsync();

            foreach (var medicamentDto in prescriptionPostDto.Medicaments)
            {
                await context.PrescriptionMedicaments.AddAsync(new PrescriptionMedicament
                {
                    IdMedicament = medicamentDto.IdMedicament,
                    IdPrescription = prescription.IdPrescription,
                    Dose = medicamentDto.Dose,
                    Details = medicamentDto.Details
                });
                await context.SaveChangesAsync();
            }

            await transaction.CommitAsync();
            return prescription.IdPrescription;
        }
        catch (PatientException)
        {
            await transaction.RollbackAsync();
            throw;
        }
        catch (NotFoundException)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<PatientGetDto> GetPatientByIdAsync(int idPatient)
    {
        var patient = await context.Patients
            .Include(p => p.Prescriptions)
            .ThenInclude(p => p.PrescriptionMedicaments)
            .ThenInclude(p => p.Medicament)
            .FirstOrDefaultAsync(p => p.IdPatient == idPatient);

        if (patient == null)
        {
            throw new PatientException("Patient does not exist.");
        }

        var prescriptions = new List<PrescriptionGetDto>();

        foreach (var prescription in patient.Prescriptions)
        {
            var medicaments = new List<MedicamentGetDto>();

            foreach (var prescriptionMedicament in prescription.PrescriptionMedicaments)
            {
                medicaments.Add(new MedicamentGetDto
                {
                    IdMedicament = prescriptionMedicament.Medicament.IdMedicament,
                    Name = prescriptionMedicament.Medicament.Name,
                    Dose = prescriptionMedicament.Dose,
                    Description = prescriptionMedicament.Medicament.Description,
                    Details = prescriptionMedicament.Details
                });
            }

            var doctor = context.Doctors.Select(doc => new DoctorGetDto
            {
                IdDoctor = doc.IdDoctor,
                FirstName = doc.FirstName,
            }).FirstOrDefault(d => d.IdDoctor == prescription.IdDoctor);
            
            if (doctor == null)
            {
                throw new PatientException("Doctor does not exist.");
            }

            prescriptions.Add(new PrescriptionGetDto
            {
                IdPrescription = prescription.IdPrescription,
                Date = prescription.Date,
                DueDate = prescription.DueDate,
                Medicaments = medicaments,
                Doctor = doctor
            });
        }

        return new PatientGetDto
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            BirthDate = patient.BirthDate,
            Prescriptions = prescriptions
        };
    }
}