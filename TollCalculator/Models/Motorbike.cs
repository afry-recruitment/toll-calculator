namespace TollCalculator.Models
{
    public class Motorbike : Vehicle
    {
        public new VehicleType VehicleType { get; private set; }
        public Motorbike(string licensePlate) : base(licensePlate)
        {
            LicensePlate = licensePlate;
            VehicleType = VehicleType.Motorbike;
        }
    }
}
