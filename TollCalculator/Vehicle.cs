
namespace TollFeeCalculator
{
    public abstract class Vehicle
    {
        public virtual bool IsTollFree => false;
        public abstract string GetVehicleType();
    }
}