namespace TollCalculator.Models
{
    /// <summary>
    /// Represents a motorbike exempt from toll fees.
    /// </summary>
    public class Motorbike : Vehicle
    {
        /// <summary>
        /// Initializes a new motorbike with the given registration number.
        /// </summary>
        /// <param name="regNo">Registration number of the motorbike</param>
        public Motorbike(string regNo) : base(regNo) { }

        /// <summary>
        /// Returns the vehicle type as a string.
        /// </summary>
        /// <returns>The string "Motorbike"</returns>
        public override string GetVehicleType()
        {
            return "Motorbike";
        }
    }
}