namespace CongestionTaxCalculator.Application.DTOs;

public class TaxCalculationRequest
{
    public required string VehicleType { get; set; }
    public required List<DateTime> Passages { get; set; }
}
