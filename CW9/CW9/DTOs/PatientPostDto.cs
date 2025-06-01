using System.ComponentModel.DataAnnotations;

namespace CW9.DTOs;

public class PatientPostDto
{
    [Required]
    public int IdPatient { get; set; }
    [Required]
    public string FirstName { get; set; } = null!;
    [Required]
    public string LastName { get; set; } = null!;
    [Required]
    public DateTime BirthDate { get; set; }
    
}