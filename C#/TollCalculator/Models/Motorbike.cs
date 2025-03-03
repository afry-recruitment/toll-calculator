using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollCalculator.Models;

public class Motorbike : Vehicle
{
    public override VehicleType VehicleType => VehicleType.Motorbike;
}
