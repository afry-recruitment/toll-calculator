using System;
using System.Collections.Generic;
using System.Text;

namespace TollCalculator
{
    public class EmergencyTollCalculator : TollCalculatorBase, IVehicleTollCalculator
    {
        private readonly Emergency emergency;
        public EmergencyTollCalculator(Emergency emergency) :
            base(emergency)
        {
            this.emergency = emergency;
        }
    }
}
