using CongestionTaxCalculator.Application.DTOs;
using CongestionTaxCalculator.Application.Interfaces;
using CongestionTaxCalculator.Application.Services;
using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Domain.Services;
using Moq;

namespace CongestionTaxCalculator.Application.Tests.Services;

public class CongestionTaxAppServiceTests
{
    private readonly Mock<ICityRepository> _mockCityRepository;
    private readonly Mock<ICongestionTaxService> _mockDomainService;
    private readonly CongestionTaxAppService _appService;

    public CongestionTaxAppServiceTests()
    {
        _mockCityRepository = new Mock<ICityRepository>();
        _mockDomainService = new Mock<ICongestionTaxService>();
        _appService = new CongestionTaxAppService(_mockCityRepository.Object, _mockDomainService.Object);
    }

    [Fact]
    public async Task CalculateTaxAsync_WithValidRequest_ShouldCallDomainServiceAndReturnResult()
    {
        // Arrange
        var request = new TaxCalculationRequest
        {
            CityName = "Gothenburg",
            VehicleType = "Car",
            Passages = new List<DateTime> { DateTime.Now }
        };
        var testCity = new City { Name = "Gothenburg" };
        var expectedTax = 42;

        _mockCityRepository
            .Setup(repo => repo.GetCityByNameAsync(request.CityName))
            .ReturnsAsync(testCity);

        _mockDomainService
            .Setup(ds => ds.CalculateTax(It.IsAny<Vehicle>(), request.Passages, testCity))
            .Returns(expectedTax);

        // Act
        var result = await _appService.CalculateTaxAsync(request);

        // Assert
        Assert.Equal(expectedTax, result);
        _mockCityRepository.Verify(repo => repo.GetCityByNameAsync("Gothenburg"), Times.Once); // Verify repo was called
    }

    [Fact]
    public async Task CalculateTaxAsync_WhenCityNotFound_ShouldThrowArgumentException()
    {
        // Arrange
        var request = new TaxCalculationRequest
        {
            CityName = "NonExistentCity",
            VehicleType = "Car",
            Passages = []
        };
        _mockCityRepository
            .Setup(repo => repo.GetCityByNameAsync(request.CityName))
            .ReturnsAsync((City?)null); // Simulate city not found

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _appService.CalculateTaxAsync(request));
    }
}
