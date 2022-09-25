using TollCalculator.Models;
public class VehicleSeeds
{
    public Vehicle GetCarVehicle()
    {
        return new Car();
    }
    public Vehicle GetMotorbikeVehicle()
    {
        return new Motorbike();
    }
}