
namespace TollFeeCalculator
{
    public class Diplomat : Vehicle
    {
        public override bool IsTollFree => true;
        public override string GetVehicleType()
        {
            return "Diplomat";
        }
    }
}
