namespace TollCalculator;

public class Military : IVehicle
{
    public string VehicleType { get => nameof(Military); }
    public bool IsTollFree { get => true; }
}
