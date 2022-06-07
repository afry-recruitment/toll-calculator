using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TollCalculator;

namespace TollCalculator
{
    public class Car : Vehicle, IVehicle
    {
        public override bool IsTollFreeVehicle()
        {
            return false;
        }
    }
}