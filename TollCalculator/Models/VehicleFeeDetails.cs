namespace TollCalculator.Models
{
    /// <summary>
    /// Represents the fee details for a single toll entry.
    /// </summary>
    public class VehicleFeeDetails
    {
        /// <summary>
        /// The date and time the vehicle passed through the toll station.
        /// </summary>
        public DateTime EntryTime { get; set; }

        /// <summary>
        /// The toll fee charged for this entry. 
        /// Zero if the entry falls on a toll free date or vehicle is exempt.
        /// </summary>
        public decimal Fee { get; set; }
    }
}
