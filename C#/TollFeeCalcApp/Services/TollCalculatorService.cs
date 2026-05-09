public class TollCalculatorService : ITollCalculatorService
{
    public bool IsTollFree(IVehicle vehicle)
    {
        return vehicle.Type switch
        {
            VehicleType.Tractor => true,
            VehicleType.Motorbike => true,
            VehicleType.Emergency => true,
            VehicleType.Diplomat => true,
            VehicleType.Foreign => true,
            VehicleType.Military => true,
            _ => false
        };
    }
}