using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollFeeCalculator
{
    public enum VehicleType
    {
        Car,
        Motorbike,
        Tractor,
        Emergency,
        Diplomat,
        Foreign,
        Military
    }
    public class Vehicle
    {
        public string LicensePlate { get; }
        public VehicleType Type { get; }

        public Vehicle(string licensePlate, VehicleType type)
		{
			// Guard against empty plates
			if (string.IsNullOrWhiteSpace(licensePlate))
				throw new ArgumentException("License plate cannot be empty.", nameof(licensePlate));

			LicensePlate = licensePlate;
			Type = type;
		}
    }
}
