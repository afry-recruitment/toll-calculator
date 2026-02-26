namespace TollFeeCalculator.Models;

public class VehicleConfiguration
{
    public IReadOnlyCollection<string> TollFreeVehicleTypes { get; init; } = [];
}