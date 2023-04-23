namespace TollFeeCalculator
{
    public class Motorbike : Vehicle
    {
        public Motorbike()
        {
            IsTollFree = true;
            TollFees = new List<TollFee>();
        }
    }
}
