namespace TollCalculator.Models
{
    public class TollFee
    {
        public int tollFee { get; }
        public DateTime tollDate { get; }
        public string vehicleLicensePlate { get; }

        public TollFee(int _tollCost, DateTime _tollDate, string _vehicleLicensePlate)
        {
            tollFee = _tollCost;
            tollDate = _tollDate;
            vehicleLicensePlate = _vehicleLicensePlate;
        }
    }
}
