namespace TollFeeCalculator
{
    public class Emergency : Vehicle
    {
        public Emergency(string licensePlate) : base(licensePlate)
        {
        }

        public override string GetVehicleType()
        {
            return "Emergency";
        }
    }
}
