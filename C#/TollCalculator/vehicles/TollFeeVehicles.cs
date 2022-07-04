using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TollCalculator.TollCalculator;

namespace TollCalculator.vehicles
{
    public abstract class BaseTollFreeVehicle : IVehicle
    {
        public bool IsTollFreeVehicle => true;

        public ITollCalculator TollCalculator => new BaseTollCalculator();

        public abstract string GetVehicleType();
    }

    public class MotorBike : BaseTollFreeVehicle
    {
        public override string GetVehicleType()
        {
            return "Motorbike";
        }

    }
    public class Tractor : BaseTollFreeVehicle
    {
        public override string GetVehicleType()
        {
            return "Tractor";
        }
    }

    public class Emergency : BaseTollFreeVehicle
    {
        public override string GetVehicleType()
        {
            return "Emergency";
        }
    }

    public class Diplomat : BaseTollFreeVehicle
    {
        public override string GetVehicleType()
        {
            return "Diplomat";
        }
    }

    public class Foreign : BaseTollFreeVehicle
    {
        public override string GetVehicleType()
        {
            return "Foreign";
        }
    }

    public class Military : BaseTollFreeVehicle
    {
        public override string GetVehicleType()
        {
            return "Military";
        }
    }



}
