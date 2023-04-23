namespace TollFeeCalculator
{
    public abstract class Vehicle
    {
        public string VehicleType { get; set; }
        public bool IsTollFree { get; set; }
        public List<TollFee> TollFees { get; set; }
    }
}
