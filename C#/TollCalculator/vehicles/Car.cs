using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TollCalculator.TollCalculator;

namespace TollCalculator.vehicles
{
    public class Car : IVehicle
    {
        private bool _IsTollFreeVehicle = false;
        public bool IsTollFreeVehicle { get => _IsTollFreeVehicle; }
        public ITollCalculator TollCalculator => new CarTollCalculator();
        public string GetVehicleType()
        {
            return "Car";
        }

    }
}
