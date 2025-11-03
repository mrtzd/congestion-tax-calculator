using CongestionTaxCalculator.Application.Interfaces;
using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CongestionTaxCalculator.Infrastructure.Repositories;

public class CityRepository(ApplicationDbContext context) : ICityRepository
{
    public async Task<City?> GetCityByNameAsync(string cityName)
    {
        return await context.Cities
            .AsNoTracking()
            .AsSplitQuery()
            .Include(c => c.TollFeeRules)
            .Include(c => c.TollFreeWeekdays)
            .Include(c => c.TollFreeMonths)
            .Include(c => c.PublicHolidays)
            .Include(c => c.VehicleExemptions)
            .FirstOrDefaultAsync(c => c.Name == cityName);
    }
}
