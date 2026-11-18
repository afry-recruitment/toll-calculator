namespace TollFeeCalculator
{
    public class Tractor : Vehicle
    {
        public Tractor(string licensePlate) : base(licensePlate)
        {
        }

        public override string GetVehicleType()
        {
            return "Tractor";
        }
    }
}
