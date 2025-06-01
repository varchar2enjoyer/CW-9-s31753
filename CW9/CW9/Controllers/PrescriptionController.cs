using CW9.DTOs;
using CW9.Exceptions;
using CW9.Services;
using Microsoft.AspNetCore.Mvc;

namespace CW9.Controllers;

[ApiController]
[Route("[controller]")]
public class PrescriptionController(IPrescriptionService prescriptionService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddPrescriptionAsync(PrescriptionPostDto prescriptionPostDto)
    {
        try
        {
            return Ok(await prescriptionService.AddPrescriptionAsync(prescriptionPostDto));
        }
        catch (PatientException e)
        {
            return BadRequest(e.Message);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetPatientAsync([FromRoute] int id)
    {
        try
        {
            return Ok(await prescriptionService.GetPatientByIdAsync(id));
        }
        catch (PatientException e)
        {
            return BadRequest(e.Message);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
    
}