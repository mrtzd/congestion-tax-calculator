using CongestionTaxCalculator.Application.DTOs;
using CongestionTaxCalculator.Application.Interfaces;
using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Domain.Enums;
using CongestionTaxCalculator.Domain.Services;

namespace CongestionTaxCalculator.Application.Services;

public class CongestionTaxAppService(CongestionTaxService domainService) : ICongestionTaxAppService
{
    public Task<int> CalculateTaxAsync(TaxCalculationRequest request)
    {
        if (!Enum.TryParse<VehicleType>(request.VehicleType, true, out var vehicleType))
        {
            throw new ArgumentException("Invalid vehicle type specified.", nameof(request.VehicleType));
        }

        var vehicle = new Vehicle(vehicleType);
        var taxAmount = domainService.CalculateTax(vehicle, request.Passages);

        return Task.FromResult(taxAmount);
    }
}
