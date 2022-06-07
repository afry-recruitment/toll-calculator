using System;
using System.Collections.Generic;
using System.Text;

namespace TollCalculator
{
    public class TractorTollCalculator : TollCalculatorBase, IVehicleTollCalculator
    {
        private readonly Tractor tractor;
        public TractorTollCalculator(Tractor tractor) :
            base(tractor)
        {
            this.tractor = tractor;
        }
    }
}
