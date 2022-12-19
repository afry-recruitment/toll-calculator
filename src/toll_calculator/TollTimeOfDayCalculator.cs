using toll_calculator.valueobjects;
using static toll_calculator.valueobjects.TollTime;

namespace toll_calculator
{
    internal static class TollTimeOfDayCalculator
    {

        public static int GetTollFee(DateTime tollDateTime)
        {
            if (_0000_0600.Contains(tollDateTime)) return 0;
            if (_0600_0630.Contains(tollDateTime)) return TollFee.LowTraffic;
            if (_0630_0700.Contains(tollDateTime)) return TollFee.MidTraffic;
            if (_0700_0800.Contains(tollDateTime)) return TollFee.RushHourTraffic;
            if (_0800_0830.Contains(tollDateTime)) return TollFee.MidTraffic;
            if (_0830_1500.Contains(tollDateTime)) return TollFee.LowTraffic;
            if (_1500_1530.Contains(tollDateTime)) return TollFee.MidTraffic;
            if (_1530_1700.Contains(tollDateTime)) return TollFee.RushHourTraffic;
            if (_1700_1800.Contains(tollDateTime)) return TollFee.MidTraffic;
            if (_1800_1830.Contains(tollDateTime)) return TollFee.LowTraffic;
            if (_1830_2400.Contains(tollDateTime)) return 0;

            throw new NotImplementedException();
        }
    }
}
