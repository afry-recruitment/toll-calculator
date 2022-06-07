using System;
using System.Collections.Generic;
using System.Text;

namespace TollCalculator
{
    public class Diplomat : Vehicle, IVehicle
    {
        public override bool IsTollFreeVehicle()
        {
            return true;
        }
    }
}
