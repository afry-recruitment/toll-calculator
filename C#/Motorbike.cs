namespace TollFeeCalculator
{
    public class Motorbike : IVehicle
    {
        bool IVehicle.IsTollFree() => true;
    }
}
