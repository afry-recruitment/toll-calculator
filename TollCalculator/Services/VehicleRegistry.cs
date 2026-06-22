using TollCalculator.Models;

namespace TollCalculator.Services
{
    /// <summary>
    /// Registry of known vehicles accessible by registration number.
    /// Mirrors a real world vehicle registry where toll sensors look up vehicles by plate number.
    /// </summary>
    public class VehicleRegistry
    {
        private readonly Dictionary<string, Vehicle> _vehicles;

        /// <summary>
        /// Initializes the registry with a list of vehicles.
        /// </summary>
        /// <param name="vehicles">List of vehicles to register</param>
        /// <exception cref="ArgumentNullException">Thrown when vehicles is null</exception>
        /// <exception cref="ArgumentException">Thrown when duplicate registration numbers are found</exception>
        public VehicleRegistry(IEnumerable<Vehicle> vehicles)
        {
            ArgumentNullException.ThrowIfNull(vehicles);

            _vehicles = new Dictionary<string, Vehicle>();
            foreach (var vehicle in vehicles)
            {
                if (_vehicles.ContainsKey(vehicle.RegNo))
                    throw new ArgumentException($"Duplicate RegNo: {vehicle.RegNo}");
                _vehicles[vehicle.RegNo] = vehicle;
            }
        }

        /// <summary>
        /// Returns the vehicle with the given registration number, or null if not found.
        /// </summary>
        /// <param name="regNo">Registration number to look up</param>
        /// <returns>Vehicle if found, null otherwise</returns>
        public Vehicle? GetVehicle(string regNo)
        {
            return _vehicles.TryGetValue(regNo, out Vehicle? vehicle)
                ? vehicle
                : null;
        }
    }
}
