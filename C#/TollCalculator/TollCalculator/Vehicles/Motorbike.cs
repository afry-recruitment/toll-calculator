using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollCalculator
{
    public class Motorbike : Vehicle, IVehicle
    {
        public override bool IsTollFreeVehicle()
        {
            return true;
        }
    }
}
