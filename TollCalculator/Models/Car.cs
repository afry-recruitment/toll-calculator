namespace TollFeeCalculator
{
    public class Car : Vehicle
    {
        public Car()
        {
            VehicleType = "Car";
            IsTollFree = false;
            TollFees = new List<TollFee>();
        }
    }
}