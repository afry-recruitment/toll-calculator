using System;
using System.Collections.Generic;
using System.Text;

namespace TollCalculator
{
    public interface IVehicleTollCalculator
    {
        int GetTollFee(DateTime[] dates, List<TimeToTollFee> timeToTollFees);
    }
}
