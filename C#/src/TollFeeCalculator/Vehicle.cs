namespace TollFeeCalculator;

public enum VehicleType
{
    Car,
    Motorbike,
    Tractor,
    Emergency,
    Diplomat,
    Foreign,
    Military,
}

public sealed record Vehicle(VehicleType Type)
{
    private static readonly HashSet<VehicleType> TollFreeTypes =
    [
        VehicleType.Motorbike,
        VehicleType.Tractor,
        VehicleType.Emergency,
        VehicleType.Diplomat,
        VehicleType.Foreign,
        VehicleType.Military,
    ];

    public bool IsTollFree => TollFreeTypes.Contains(Type);
}
