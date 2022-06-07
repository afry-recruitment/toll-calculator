using System;
using System.Collections.Generic;
using System.Text;

namespace TollCalculator
{
    public class DiplomatTollCalculator : TollCalculatorBase, IVehicleTollCalculator
    {
        private readonly Diplomat diplomat;
        public DiplomatTollCalculator(Diplomat diplomat) :
            base(diplomat)
        {
            this.diplomat = diplomat;
        }
    }
}
