namespace toll_calculator.models;

internal sealed class TrafficTollSpecification
{
    public TrafficTollSpecification(
        DateTime validFrom, 
        DateTime validUntil,
        DailyTrafficTollSpecification dailyTrafficTollSpecification,
        DateTime[] tollFreeDates, 
        int[] tollFreeVehicleTypes)
    {
        ValidFrom = validFrom;
        ValidUntil = validUntil;
        DailyTrafficTollSpecification = dailyTrafficTollSpecification;
        TollFreeDates = tollFreeDates;
        TollFreeVehicleTypes = tollFreeVehicleTypes;

    }

    public DateTime ValidFrom { get; }
    public DateTime ValidUntil { get; }
    public DailyTrafficTollSpecification DailyTrafficTollSpecification { get; }
    public DateTime[] TollFreeDates { get; }
    public int[] TollFreeVehicleTypes { get; }
}
