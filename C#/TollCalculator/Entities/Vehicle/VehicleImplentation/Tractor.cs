namespace TollCalculator;
public class Tractor : IVehicle
{
    public string VehicleType { get => nameof(Tractor); }
    public bool IsTollFree { get => true; }
}