namespace CongestionTaxCalculator.Domain.ValueObjects;

public record TollFeeRule(TimeOnly StartTime, TimeOnly EndTime, int Amount);
