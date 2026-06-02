namespace TollFeeCalculator.Models
{
    public class Emergency : IVehicle
    {
        public bool IsTollFree => true;
    }
}
