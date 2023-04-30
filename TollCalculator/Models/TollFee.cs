namespace TollCalculator.Models
{
    public class TollFee
    {
        public int TollFeeAmount { get; }
        public DateTime TollDate { get; }
        public string VehicleLicensePlate { get; }

        public TollFee(int tollFeeAmount, DateTime tollDate, string vehicleLicensePlate)
        {
            TollFeeAmount = tollFeeAmount;
            TollDate = tollDate;
            VehicleLicensePlate = vehicleLicensePlate;
        }
    }
}
