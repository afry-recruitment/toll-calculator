
namespace TollFeeCalculator
{
    public class Motorbike : Vehicle
    {
        public override bool IsTollFree => true;
        public override string GetVehicleType()
        {
            return "Motorbike";
        }
    }
}
