namespace TollFeeCalculator
{
    public class Motorbike : Vehicle
    {
        public Motorbike(string licensePlate) : base(licensePlate)
        {
        }

        public override string GetVehicleType()
        {
            return "Motorbike";
        }
    }
}
