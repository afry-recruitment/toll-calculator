namespace toll_calculator
{
    internal static class TollTimeOfDayCalculator
    {
        public static int GetTollFee(DateTime date)
        {

            int hour = date.Hour;
            int minute = date.Minute;

            if (Is_6_00_to_6_29(date)) return Toll.LowTraffic;
            else if (Is_6_30_to_6_59(date)) return Toll.MidTraffic;
            else if (hour == 7 && minute >= 0 && minute <= 59) return Toll.RushHourTraffic;
            else if (hour == 8 && minute >= 0 && minute <= 29) return Toll.MidTraffic;
            else if (hour >= 8 && hour <= 14 && minute >= 30 && minute <= 59) return Toll.LowTraffic;
            else if (hour == 15 && minute >= 0 && minute <= 29) return Toll.MidTraffic;
            else if (hour == 15 && minute >= 0 || hour == 16 && minute <= 59) return Toll.RushHourTraffic;
            else if (hour == 17 && minute >= 0 && minute <= 59) return Toll.MidTraffic;
            else if (hour == 18 && minute >= 0 && minute <= 29) return Toll.LowTraffic;
            else return 0;
        }
        private static bool Is_6_00_to_6_29(DateTime date)
        {
            int hour = date.Hour;
            int minute = date.Minute;
            return (hour == 6 && minute >= 0 && minute <= 29);
        }
        private static bool Is_6_30_to_6_59(DateTime date)
        {
            int hour = date.Hour;
            int minute = date.Minute;
            return (hour == 6 && minute >= 30 && minute <= 59);
        }
    }
}
