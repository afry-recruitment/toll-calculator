namespace TollFeeCalculator
{
    public class Diplomat : Vehicle
    {
        public Diplomat(string licensePlate) : base(licensePlate)
        {
        }

        public override string GetVehicleType()
        {
            return "Diplomat";
        }
    }
}
