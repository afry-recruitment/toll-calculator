namespace TollCalculator.Models
{
    /// <summary>
    /// Represents the calculated toll fee for a vehicle.
    /// </summary>
    public class VehicleFee
    {
        /// <summary>
        /// The vehicle registration number.
        /// </summary>
        public string RegNo { get; set; }

        /// <summary>
        /// The total calculated toll fee for the day, capped at 60 SEK.
        /// </summary>
        public decimal TotalFee { get; set; }

        /// <summary>
        /// List of toll entry details with individual fees.
        /// </summary>
        public required List<VehicleFeeDetails> Details { get; set; }

        /// <summary>
        /// Indicates whether the vehicle was not found in the registry.
        /// Unknown vehicles are charged full fee with no exemptions.
        /// </summary>
        public bool IsUnknown { get; set; }
    }
}
