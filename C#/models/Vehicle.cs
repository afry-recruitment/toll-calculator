using System;
using System.ComponentModel.DataAnnotations;

namespace TollFeeCalculator
{
    public class Vehicle
    {
        [Key]
        public string LicensePlate { get; set; }
        public VehicleType VehicleType { get; set; }
        public Vehicle(string licensePlate, VehicleType vehicleType)
        {
            LicensePlate = licensePlate;
            VehicleType = vehicleType;
        }
    }

    public enum VehicleType
    {
        car,
        Motorbike,
        Tractor,
        Emergency,
        Diplomat,
        Foreign,
        Military
    }
}