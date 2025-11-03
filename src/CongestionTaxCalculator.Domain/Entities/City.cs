namespace CongestionTaxCalculator.Domain.Entities;

public class City
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int MaxDailyFee { get; set; }
    public int SingleChargeRuleMinutes { get; set; }
    public bool IsDayBeforePublicHolidayTollFree { get; set; } = true;

    public ICollection<TollFeeRule> TollFeeRules { get; set; } = [];
    public ICollection<TollFreeWeekday> TollFreeWeekdays { get; set; } = [];
    public ICollection<TollFreeMonth> TollFreeMonths { get; set; } = [];
    public ICollection<PublicHoliday> PublicHolidays { get; set; } = [];
    public ICollection<VehicleExemption> VehicleExemptions { get; set; } = [];
}
