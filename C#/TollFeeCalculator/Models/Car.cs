namespace TollFeeCalculator;

public class Car : IVehicle
{
    public string GetVehicleType()
    {
        return TollPayingVehiclesEnum.Car.ToString();
    }
}
