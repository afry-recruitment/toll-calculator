namespace TollFeeCalculator
{
    public class Car : IVehicle
    {
        public string VehicleType { get => nameof(Car); }
        public bool IsTollFree { get => false; }
    }
}