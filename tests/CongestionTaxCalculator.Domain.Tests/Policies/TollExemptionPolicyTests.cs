using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Domain.Enums;
using CongestionTaxCalculator.Domain.Policies;
using CongestionTaxCalculator.Domain.Tests.Helpers;

namespace CongestionTaxCalculator.Domain.Tests.Policies;

public class TollExemptionPolicyTests
{
    [Theory]
    [InlineData(VehicleType.Car, false)]
    [InlineData(VehicleType.Motorbike, true)]
    [InlineData(VehicleType.Emergency, true)]
    [InlineData(VehicleType.Diplomat, true)]
    [InlineData(VehicleType.Foreign, true)]
    [InlineData(VehicleType.Military, true)]
    [InlineData(VehicleType.Bus, true)]
    public void IsTollFree_ShouldReturnCorrectStatus_ForVehicleType(VehicleType type, bool expectedIsFree)
    {
        // Arrange
        var vehicle = new Vehicle(type);
        var city = TestDataFactory.CreateGothenburgTestCity();
        var policy = new TollExemptionPolicy();

        // Act
        var isFree = policy.IsTollFree(vehicle, city.VehicleExemptions.Select(ve => ve.VehicleType));

        // Assert
        Assert.Equal(expectedIsFree, isFree);
    }
}
