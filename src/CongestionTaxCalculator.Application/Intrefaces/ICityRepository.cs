using CongestionTaxCalculator.Domain.Entities;

namespace CongestionTaxCalculator.Application.Interfaces;

public interface ICityRepository
{
    Task<City?> GetCityByNameAsync(string cityName);
}
