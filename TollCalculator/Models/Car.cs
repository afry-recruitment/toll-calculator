namespace TollCalculator.Models
{
    /// <summary>
    /// Represents a standard car subject to toll fees.
    /// </summary>
    public class Car : Vehicle
    {
        /// <summary>
        /// Initializes a new car with the given registration number.
        /// </summary>
        /// <param name="regNo">Registration number of the car</param>
        public Car(string regNo) : base(regNo) { }

        /// <summary>
        /// Returns the vehicle type as a string.
        /// </summary>
        /// <returns>The string "Car"</returns>
        public override string GetVehicleType()
        {
            return "Car";
        }
    }
}