namespace TollFeeCalculator;
public class Diplomat : IVehicle
{
    public string VehicleType { get => nameof(Diplomat); }
    public bool IsTollFree { get => true; }
}