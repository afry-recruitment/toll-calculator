namespace TollFeeCalculator
{
    public class Car : Vehicle
    {
        public Car()
        {
            IsTollFree = false;
            TollFees = new List<TollFee>();
        }
    }
}