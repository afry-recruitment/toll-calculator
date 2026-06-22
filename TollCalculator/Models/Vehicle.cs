namespace TollCalculator.Models
{
    /// <summary>
    /// Abstract base class for all vehicle types.
    /// </summary>
    public abstract class Vehicle
    {
        /// <summary>
        /// The vehicle registration number.
        /// </summary>
        public string RegNo { get; private set; }

        /// <summary>
        /// Initializes a new vehicle with the given registration number.
        /// </summary>
        /// <param name="regNo">Registration number of the vehicle</param>
        protected Vehicle(string regNo)
        {
            RegNo = regNo;
        }

        /// <summary>
        /// Returns the vehicle type as a string.
        /// </summary>
        /// <returns>A string representing the vehicle type</returns>
        public abstract string GetVehicleType();
    }
}