using System;

namespace TollFeeCalculator.models
{
    public class Motorbike : IVehicle
    {
        public Motorbike(string licensePlate) { LicensePlate = licensePlate; }
        public string LicensePlate { get; set; }
        public string GetVehicleType()
        {
            return "Motorbike";
        }
    }

    public class Car : IVehicle
    {
        public Car(string licensePlate) { LicensePlate = licensePlate; }
        public string LicensePlate { get; set; }
        public String GetVehicleType()
        {
            return "Car";
        }
    }

    public class Tractor : IVehicle
    {
        public Tractor(string licensePlate) { LicensePlate = licensePlate; }
        public string LicensePlate { get; set; }
        public String GetVehicleType()
        {
            return "Tractor";
        }
    }

    public class Emergency : IVehicle
    {
        public Emergency(string licensePlate) { LicensePlate = licensePlate; }
        public string LicensePlate { get; set; }
        public String GetVehicleType()
        {
            return "Emergency";
        }
    }

    public class Diplomat : IVehicle
    {
        public Diplomat(string licensePlate) { LicensePlate = licensePlate; }
        public string LicensePlate { get; set; }
        public String GetVehicleType()
        {
            return "Diplomat";
        }
    }

    public class Military : IVehicle
    {
        public Military(string licensePlate) { LicensePlate = licensePlate; }
        public string LicensePlate { get; set; }
        public String GetVehicleType()
        {
            return "Military";
        }
    }

    public class Foreign : IVehicle
    {
        public Foreign(string licensePlate) { LicensePlate = licensePlate; }
        public string LicensePlate { get; set; }
        public String GetVehicleType()
        {
            return "Foreign";
        }
    }
}