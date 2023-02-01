using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollCalculator.Model
{
    public abstract class Vehicle
    {
        public abstract bool ExceptionsFromCongestionTax();
        public abstract int Weight { set; }
    }
}
