namespace TollFeeCalculator.Enums
{
    /// <summary>
    /// Available vehicleTypes for handling toll fee costs
    /// </summary>
    public enum VehicleTypes
    {
        /// <summary>
        /// Default, Vehicle is generic car type or unkown
        /// </summary>
        Car = 0,
        /// <summary>
        /// Vehicle is of motorbike type
        /// </summary>
        Motorbike = 1,
        /// <summary>
        /// Vehicle is of tractor type
        /// </summary>
        Tractor = 2,
        /// <summary>
        /// Vehicle is of a emergency type ex: ambulance or police
        /// </summary>
        Emergency = 3,
        /// <summary>
        /// Vehicle has diplomatic status
        /// </summary>
        Diplomat = 4,
        /// <summary>
        /// Vehicle is registered in foreign country
        /// </summary>
        Foreign = 5,
        /// <summary>
        /// Vehicle is of a military type
        /// </summary>
        Military = 6,
      
    }
}
