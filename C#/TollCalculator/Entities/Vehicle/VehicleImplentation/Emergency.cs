namespace TollFeeCalculator;
public class Emergency : IVehicle
{
    public string VehicleType { get => nameof(Emergency); }
    public bool IsTollFree { get => true; }
}