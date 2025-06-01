using CW9.Models;
using Microsoft.EntityFrameworkCore;

namespace CW9.Data;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }
}