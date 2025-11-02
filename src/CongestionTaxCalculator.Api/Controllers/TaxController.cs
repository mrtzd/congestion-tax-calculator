using CongestionTaxCalculator.Application.DTOs;
using CongestionTaxCalculator.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CongestionTaxCalculator.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaxController(ICongestionTaxAppService appService) : ControllerBase
{
    [HttpPost("calculate")]
    public async Task<IActionResult> CalculateTax([FromBody] TaxCalculationRequest request)
    {
        try
        {
            var taxAmount = await appService.CalculateTaxAsync(request);
            return Ok(new { TaxAmount = taxAmount });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, "An unexpected error occurred.");
        }
    }
}
