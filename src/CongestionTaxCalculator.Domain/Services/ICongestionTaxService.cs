using CongestionTaxCalculator.Domain.Entities;

namespace CongestionTaxCalculator.Domain.Services;

public interface ICongestionTaxService
{
    int CalculateTax(Vehicle vehicle, IReadOnlyList<DateTime> dates, City city);
}
