using System;
using System.Collections.Generic;
using System.Text;

namespace TollCalculator
{
    public class Emergency : Vehicle, IVehicle
    {
        public override bool IsTollFreeVehicle()
        {
            return true;
        }
    }
}
