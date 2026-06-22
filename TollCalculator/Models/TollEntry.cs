namespace TollCalculator.Models
{
    /// <summary>
    /// Represents a vehicle passing through a toll station.
    /// </summary>
    public class TollEntry
    {
        /// <summary>
        /// The registration number of the vehicle passing through the toll station.
        /// </summary>
        public string RegNo { get; private set; }

        /// <summary>
        /// The date and time the vehicle passed through the toll station.
        /// </summary>
        public DateTime EntryTime { get; private set; }

        /// <summary>
        /// Initializes a new toll entry with a registration number and entry time.
        /// </summary>
        /// <param name="regNo">Registration number of the vehicle</param>
        /// <param name="entryTime">Date and time of the toll entry</param>
        public TollEntry(string regNo, DateTime entryTime)
        {
            RegNo = regNo;
            EntryTime = entryTime;
        }
    }
}
