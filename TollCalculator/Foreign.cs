
namespace TollFeeCalculator
{
    public class Foreign : Vehicle
    {
        public override bool IsTollFree => true;
        public override string GetVehicleType()
        {
            return "Foreign";
        }
    }
}
