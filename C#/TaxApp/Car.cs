namespace TaxApp;

public class Car : IVehicle
{
    public TollFreeVehicles GetVehicleType() => TollFreeVehicles.None;
}
