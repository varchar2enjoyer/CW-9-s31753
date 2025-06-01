namespace CW9.DTOs;

public class PrescriptionGetDto
{
    public int IdPrescription { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public IEnumerable<MedicamentGetDto> Medicaments { get; set; } = null!;
    public DoctorGetDto Doctor { get; set; } = null!;
}