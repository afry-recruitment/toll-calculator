
namespace TollFeeCalculator
{
    public class Emergency : Vehicle
    {
        public override bool IsTollFree => true;
        public override string GetVehicleType()
        {
            return "Emergency";
        }
    }
}
