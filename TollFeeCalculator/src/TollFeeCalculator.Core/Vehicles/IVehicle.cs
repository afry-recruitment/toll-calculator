namespace TollFeeCalculator.Core.Vehicles
{
    /// <summary>
    /// A vehicle that can pass through a toll station.
    ///
    /// The original interface exposed GetVehicleType() as a string and TollCalculator compared it 
    /// against a TollFreeVehicles enum. 
    /// 
    /// That forced the calculator to know every vehicle category (open for modification on each 
    /// new category) and turned renames into silent runtime errors. 
    /// 
    /// The vehicle now answers the only question the calculator cares about - "are you toll-free?" 
    /// - so a new category is a new class, never a change in the calculator.
    /// </summary>
    public interface IVehicle
    {
        /// <summary>
        /// True when this vehicle category is exempt from toll by law (e.g.motorbikes;
        /// in the system also tractors, emergency, diplomat, foreign and
        /// military vehicles). Each concrete vehicle owns its own answer.
        /// </summary>

        bool IsTollFree();
    }
}