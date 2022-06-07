using System;
using System.Collections.Generic;
using System.Text;

namespace TollCalculator
{
    public class MotorbikeTollCalculator : TollCalculatorBase, IVehicleTollCalculator
    {
        private readonly Motorbike motorbike;
        public MotorbikeTollCalculator(Motorbike motorbike) :
            base(motorbike)
        {
            this.motorbike = motorbike;
        }
    }
}
