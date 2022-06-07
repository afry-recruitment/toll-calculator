using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TollCalculator;

namespace TollCalculator
{
    public abstract class Vehicle : IVehicle
    {

        public string VehicleType => this.GetType().Name;

        public virtual bool IsTollFreeVehicle()
        {
            throw new NotImplementedException();
        }
    }
}