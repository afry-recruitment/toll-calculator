namespace TollFeeCalculator
{
    public class Military : Vehicle
    {
        public Military(string licensePlate) : base(licensePlate)
        {
        }

        public override string GetVehicleType()
        {
            return "Military";
        }
    }
}
