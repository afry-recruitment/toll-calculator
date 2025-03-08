namespace TaxApp;

public class MotorCycle : IVehicle
{
    public TollFreeVehicles GetVehicleType() => TollFreeVehicles.Motorcycle;
}
