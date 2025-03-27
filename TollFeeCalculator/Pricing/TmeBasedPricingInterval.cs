namespace TollFeeCalculator.Pricing
{
    public class TmeBasedPricingInterval
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public decimal Price { get; set; }
    }
}
