using CongestionTaxCalculator.Application.DTOs;
using CongestionTaxCalculator.Application.Services;
using CongestionTaxCalculator.Domain.Policies;
using CongestionTaxCalculator.Domain.Services;

namespace CongestionTaxCalculator.Application.Tests.Services;

public class CongestionTaxAppServiceTests
{
    private readonly CongestionTaxAppService _appService;

    public CongestionTaxAppServiceTests()
    {
        var domainService = new CongestionTaxService(
            new TollExemptionPolicy(),
            new DateTollPolicy(2013),
            new TollFeePolicy()
        );
        _appService = new CongestionTaxAppService(domainService);
    }

    [Fact]
    public async Task CalculateTaxAsync_WithValidCarRequest_ShouldReturnCorrectTax()
    {
        // Arrange
        var request = new TaxCalculationRequest
        {
            VehicleType = "Car",
            Passages =
            [
                DateTime.Parse("2013-02-07 06:23:27"), // 8 SEK
                DateTime.Parse("2013-02-07 15:27:00")  // 13 SEK
            ]
        };
        // Expected result: 8 + 13 = 21

        // Act
        var result = await _appService.CalculateTaxAsync(request);

        // Assert
        Assert.Equal(21, result);
    }

    [Fact]
    public async Task CalculateTaxAsync_WithInvalidVehicleType_ShouldThrowArgumentException()
    {
        // Arrange
        var request = new TaxCalculationRequest
        {
            VehicleType = "Skateboard",
            Passages = [DateTime.Now]
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _appService.CalculateTaxAsync(request));
    }
}
