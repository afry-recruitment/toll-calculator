using System;
using System.Collections.Generic;
using System.Text;

namespace TollCalculator
{
    public class ForeignTollCalculator : TollCalculatorBase, IVehicleTollCalculator
    {
        private readonly Foreign foreign;
        public ForeignTollCalculator(Foreign foreign) :
            base(foreign)
        {
            this.foreign = foreign;
        }
    }
}
