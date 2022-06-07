using System;
using System.Collections.Generic;
using System.Text;

namespace TollCalculator
{
    public class Foreign : Vehicle, IVehicle
    {
        public override bool IsTollFreeVehicle()
        {
            return true;
        }
    }
}
