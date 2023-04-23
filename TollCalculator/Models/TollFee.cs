namespace TollCalculator.Models
{
    public class TollFee
    {
        public int TollCost { get; private set; }
        public DateTime TollDate { get; private set; }
        public string VehicleLicensePlate { get; private set; }
    }
}
