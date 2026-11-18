using System;

namespace TollFeeCalculator
{
    public abstract class Vehicle
    {
        protected Vehicle(string licensePlate)
        {
            if (licensePlate == null || licensePlate.Length != 6)
            {
                throw new ArgumentException("License plate must be 6 characters.", nameof(licensePlate));
            }

            LicensePlate = licensePlate;
        }

        public string LicensePlate { get; }

        public abstract string GetVehicleType();
    }
}