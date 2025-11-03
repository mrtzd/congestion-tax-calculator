using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CongestionTaxCalculator.Application.DTOs;
using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CongestionTaxCalculator.Api.Tests;

public class TaxControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;

    public TaxControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));

                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("ApiTestDb");
                });
            });
        });

        _client = _factory.CreateClient();
    }

    private void SeedDatabaseForTest(City city)
    {
        // Get a scope to access services
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Ensure the database is clean and then seed it
        context.Database.EnsureDeleted();
        context.Cities.Add(city);
        context.SaveChanges();
    }

    [Fact]
    public async Task CalculateTax_WithValidRequestAndExistingCity_ReturnsOkWithCorrectTax()
    {
        // Arrange
        var city = new City
        {
            Name = "Testburg",
            MaxDailyFee = 60,
            SingleChargeRuleMinutes = 60,
            TollFeeRules = { new TollFeeRule { Amount = 18, StartTime = new(7, 0), EndTime = new(8, 0) } }
        };
        SeedDatabaseForTest(city);

        var request = new TaxCalculationRequest
        {
            CityName = "Testburg",
            VehicleType = "Car",
            Passages = [new DateTime(2024, 1, 10, 7, 30, 0)]
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/tax/calculate", request, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        var content = await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken: TestContext.Current.CancellationToken);
        var taxAmount = content.GetProperty("taxAmount").GetInt32();

        Assert.Equal(18, taxAmount);
    }

    [Fact]
    public async Task CalculateTax_WhenCityNotFound_ReturnsBadRequest()
    {
        // Arrange
        var request = new TaxCalculationRequest
        {
            CityName = "Nowhere",
            VehicleType = "Car",
            Passages = [DateTime.Now]
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/tax/calculate", request, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}