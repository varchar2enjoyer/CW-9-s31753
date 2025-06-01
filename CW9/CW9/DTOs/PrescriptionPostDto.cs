using System.ComponentModel.DataAnnotations;
using CW9.Models;

namespace CW9.DTOs;

public class PrescriptionPostDto
{
    [Required]
    public Patient PatientPostDto { get; set; } = null!;
    [Required]
    public List<MedicamentPostDto> Medicaments { get; set; } = null!;
    [Required]
    public DateTime Date { get; set; }
    [Required]
    public DateTime DueDate { get; set; }
    [Required]
    public int IdDoctor { get; set; }
}