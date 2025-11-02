using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Domain.Enums;

namespace CongestionTaxCalculator.Domain.Policies;

public static class TollExemptionPolicy
{
    public static bool IsTollFree(Vehicle vehicle)
    {
        return vehicle.Type switch
        {
            VehicleType.Motorbike => true,
            VehicleType.Tractor => true,
            VehicleType.Emergency => true,
            VehicleType.Diplomat => true,
            VehicleType.Foreign => true,
            VehicleType.Military => true,
            VehicleType.Bus => true,
            _ => false
        };
    }
}
