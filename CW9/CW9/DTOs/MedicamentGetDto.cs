﻿namespace CW9.DTOs;

public class MedicamentGetDto
{
    public int IdMedicament { get; set; }
    public string Name { get; set; } = null!;
    public int Dose { get; set; }
    public string Description { get; set; } = null!;
    public string Details { get; set; } = null!;
}