namespace TollFeeCalculator
{
    public abstract class Vehicle
    {
        // I will only create two classes that inherit from Vehicle, Motorbike (toll-free) and Car (not toll-free) to avoid unneccessery classes since anymore are superfluous
        // Requirements only state that some vehicle types are fee-free, not that all vehicle types found in the TollFreeVehicles enum had to be implemented
        public bool IsTollFree { get; set; }
        public List<TollFee> TollFees { get; set; }
    }
}
