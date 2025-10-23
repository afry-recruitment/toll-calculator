
namespace TollFeeCalculator
{
    public class Tractor : Vehicle
    {
        public override bool IsTollFree => true;
        public override string GetVehicleType()
        {
            return "Tractor";
        }
    }
}
