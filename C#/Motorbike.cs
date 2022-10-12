namespace TollFeeCalculator
{
    public class Motorbike : IVehicle
    {
        public string VehicleType { get => nameof(Motorbike); }
        public bool IsTollFree { get => true; }
    }
}
