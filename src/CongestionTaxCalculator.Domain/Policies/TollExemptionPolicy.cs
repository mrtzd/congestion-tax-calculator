using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Domain.Enums;

namespace CongestionTaxCalculator.Domain.Policies;

public class TollExemptionPolicy
{
    public bool IsTollFree(Vehicle vehicle, IEnumerable<VehicleType> exemptTypes)
    {
        return exemptTypes.Contains(vehicle.Type);
    }
}
