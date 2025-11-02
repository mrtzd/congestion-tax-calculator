using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Domain.Enums;
using CongestionTaxCalculator.Domain.Policies;

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

        // Act
        var isFree = TollExemptionPolicy.IsTollFree(vehicle);

        // Assert
        Assert.Equal(expectedIsFree, isFree);
    }
}
