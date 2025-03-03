using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollCalculator.Models;

public abstract class Vehicle
{
    public virtual VehicleType VehicleType { get; }
}