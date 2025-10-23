
namespace TollFeeCalculator
{
    public class Military : Vehicle
    {
        public override bool IsTollFree => true;
        public override string GetVehicleType()
        {
            return "Military";
        }
    }
}
