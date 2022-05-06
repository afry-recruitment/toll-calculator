using System;
using TollFeeCalculator;

namespace afryCodeTest.toll_calculator.C_
{
    public class Motorbike : Vehicle
    {
        public string GetVehicleType()
        {
            return "Motorbike";
        }
    }

    public class Car : Vehicle
    {
        public String GetVehicleType()
        {
            return "Car";
        }
    }

    public class Tractor : Vehicle
    {
        public String GetVehicleType()
        {
            return "Tractor";
        }
    }

    public class Emergency : Vehicle
    {
        public String GetVehicleType()
        {
            return "Emergency";
        }
    }

    public class Diplomat : Vehicle
    {
        public String GetVehicleType()
        {
            return "Diplomat";
        }
    }

    public class Military : Vehicle
    {
        public String GetVehicleType()
        {
            return "Military";
        }
    }

    public class Foreign : Vehicle
    {
        public String GetVehicleType()
        {
            return "Foreign";
        }
    }
}