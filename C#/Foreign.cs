namespace TollFeeCalculator
{
    public class Foreign : Vehicle
    {
        public Foreign(string licensePlate) : base(licensePlate)
        {
        }

        public override string GetVehicleType()
        {
            return "Foreign";
        }
    }
}
