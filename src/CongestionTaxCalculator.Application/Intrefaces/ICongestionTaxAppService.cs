using CongestionTaxCalculator.Application.DTOs;

namespace CongestionTaxCalculator.Application.Interfaces;

public interface ICongestionTaxAppService
{
    Task<int> CalculateTaxAsync(TaxCalculationRequest request);
}
