﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollFeeCalculator
{
    public class Tractor : IVehicle
    {
        string IVehicle.GetVehicleType()
        {
            return "Tractor";
        }
    }
}
