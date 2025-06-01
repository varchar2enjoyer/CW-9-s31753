using System.ComponentModel.DataAnnotations;
namespace CW9.DTOs;

public class MedicamentPostDto
{
    [Required]
    public int IdMedicament { get; set; }
    [Required]
    public int Dose { get; set; }
    [Required]
    public string Details { get; set; } = null!;
}