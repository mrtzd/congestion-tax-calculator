using CongestionTaxCalculator.Application.DTOs;
using CongestionTaxCalculator.Application.Interfaces;
using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Domain.Enums;
using CongestionTaxCalculator.Domain.Services;

namespace CongestionTaxCalculator.Application.Services;

public class CongestionTaxAppService(ICityRepository cityRepository, ICongestionTaxService domainService) : ICongestionTaxAppService
{
    public async Task<int> CalculateTaxAsync(TaxCalculationRequest request)
    {
        var city = await cityRepository.GetCityByNameAsync(request.CityName);
        if (city == null)
        {
            throw new ArgumentException($"City '{request.CityName}' not found or has no tax rules configured.");
        }

        if (!Enum.TryParse<VehicleType>(request.VehicleType, true, out var vehicleType))
        {
            throw new ArgumentException("Invalid vehicle type specified.", nameof(request.VehicleType));
        }

        var vehicle = new Vehicle(vehicleType);

        var taxAmount = domainService.CalculateTax(vehicle, request.Passages, city);

        return taxAmount;
    }
}
