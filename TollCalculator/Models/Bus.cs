namespace TollCalculator.Models
{
    /// <summary>
    /// Represents a bus exempt from toll fees.
    /// </summary>
    public class Bus : Vehicle
    {
        /// <summary>
        /// Initializes a new bus with the given registration number.
        /// </summary>
        /// <param name="regNo">Registration number of the bus</param>
        public Bus(string regNo) : base(regNo) { }

        /// <summary>
        /// Returns the vehicle type as a string.
        /// </summary>
        /// <returns>The string "Bus"</returns>
        public override string GetVehicleType()
        {
            return "Bus";
        }
    }
}