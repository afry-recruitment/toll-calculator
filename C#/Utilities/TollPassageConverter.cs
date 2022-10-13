using System;
using TollFeeCalculator;

namespace TollFeeCalculator.Utilities
{
    static class TollPassageConverter
    {
        public static void ConvertOldPassages(Vehicle vehicle, DateTime[] dates)
        {
            foreach (var date in dates)
            {
                _ = new TollStationPassage(vehicle, date);
            }
        }
    }
}
