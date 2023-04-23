namespace TollFeeCalculator
{
    public class Motorbike : Vehicle
    {
        public Motorbike()
        {
            VehicleType = "Motorbike";
            IsTollFree = true;
            TollFees = new List<TollFee>();
        }
    }
}
