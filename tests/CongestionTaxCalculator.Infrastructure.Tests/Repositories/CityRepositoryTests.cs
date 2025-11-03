using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Domain.Enums;
using CongestionTaxCalculator.Infrastructure.Persistence;
using CongestionTaxCalculator.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CongestionTaxCalculator.Infrastructure.Tests.Repositories;

public class CityRepositoryTests
{
    private readonly ApplicationDbContext _context;
    private readonly CityRepository _repository;

    public CityRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _repository = new CityRepository(_context);
    }

    private void SeedDatabase()
    {
        var city = new City
        {
            Name = "TestCity",
            MaxDailyFee = 50,
            TollFeeRules = { new TollFeeRule { Amount = 10, StartTime = new TimeOnly(8, 0), EndTime = new TimeOnly(9, 0) } },
            PublicHolidays = { new PublicHoliday { Date = new DateOnly(2024, 1, 1) } },
            TollFreeMonths = { new TollFreeMonth { Month = 8 } },
            TollFreeWeekdays = { new TollFreeWeekday { DayOfWeek = DayOfWeek.Friday } },
            VehicleExemptions = { new VehicleExemption { VehicleType = VehicleType.Bus } }
        };

        _context.Cities.Add(city);
        _context.SaveChanges();
    }

    [Fact]
    public async Task GetCityByNameAsync_WhenCityExists_ShouldReturnCityWithAllRelatedRules()
    {
        // Arrange
        SeedDatabase();

        // Act
        var result = await _repository.GetCityByNameAsync("TestCity");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("TestCity", result.Name);
        Assert.Equal(50, result.MaxDailyFee);

        Assert.Single(result.TollFeeRules);
        Assert.Equal(10, result.TollFeeRules.First().Amount);

        Assert.Single(result.PublicHolidays);
        Assert.Single(result.TollFreeMonths);
        Assert.Single(result.TollFreeWeekdays);
        Assert.Single(result.VehicleExemptions);
    }

    [Fact]
    public async Task GetCityByNameAsync_WhenCityDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        // No data seeded for this test

        // Act
        var result = await _repository.GetCityByNameAsync("NonExistentCity");

        // Assert
        Assert.Null(result);
    }
}
