namespace TollFeeCalculator
{
    public class Foreign : IVehicle
    {
        public string VehicleType { get => nameof(Foreign); }
        public bool IsTollFree { get => true; }
    }
}