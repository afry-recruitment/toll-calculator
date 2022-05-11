using System;

namespace TollFeeCalculator
{
    public class Motorbike : Vehicle
    {
        public string LicensePlate { get; set; }
        public string GetVehicleType()
        {
            return "Motorbike";
        }
    }

    public class Car : Vehicle
    {
        public string LicensePlate { get; set; }
        public String GetVehicleType()
        {
            return "Car";
        }
    }

    public class Tractor : Vehicle
    {
        public string LicensePlate { get; set; }
        public String GetVehicleType()
        {
            return "Tractor";
        }
    }

    public class Emergency : Vehicle
    {
        public string LicensePlate { get; set; }
        public String GetVehicleType()
        {
            return "Emergency";
        }
    }

    public class Diplomat : Vehicle
    {
        public string LicensePlate { get; set; }
        public String GetVehicleType()
        {
            return "Diplomat";
        }
    }

    public class Military : Vehicle
    {
        public string LicensePlate { get; set; }
        public String GetVehicleType()
        {
            return "Military";
        }
    }

    public class Foreign : Vehicle
    {
        public string LicensePlate { get; set; }
        public String GetVehicleType()
        {
            return "Foreign";
        }
    }
}