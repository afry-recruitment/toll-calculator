using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollFeeCalculator
{
    public class Vehicle
    {
        /// <summary>
        /// The <see cref="VehicleType"/> is a <see langword="string"/> that is used to get or set a vehicle type.
        /// </summary>
        public string VehicleType { get; set; }

        /// <summary>
        /// The <see cref="Vehicle"/> is used to create a new vehicle.
        /// </summary>
        /// <param name="VehicleType">The <see langword="string"/> Vehicle type is used to say what type of vehicle it is.</param>
        public Vehicle(string VehicleType)
        {
            this.VehicleType = VehicleType;
        }

        /// <summary>
        /// The <see cref="GetVehicleType"/> is used to get the <seealso cref="VehicleType"/>.
        /// </summary>
        /// <returns>The <see cref="VehicleType"/>.</returns>
        public string GetVehicleType()
        {
            return VehicleType;
        }
    }
}