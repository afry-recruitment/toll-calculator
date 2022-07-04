using System;
using TollCalculator.vehicles;

namespace TollCalculator.TollCalculator
{
    public interface ITollCalculator
    {
        int GetTollFee(IVehicle vehicle, DateTime[] dates);
    }
}