namespace TollFeeCalculator.Enterprise.ValueObjects
{
    public readonly record struct FeeRange(TimeOnly From, TimeOnly To, int Fee);
}
