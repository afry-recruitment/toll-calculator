using System;

namespace TollFeeCalculator
{
    public static class DateUtil
    {
        /**
         * Is the given date a holiday
         *
         * @param date   - date to test
         * @return - true if the date is a holiday
         */
        public static bool IsHoliday(DateTime date)
        {
            int month = date.Month;
            int day = date.Day;

            DateTime dateTruncateTimeOfDay = new DateTime(date.Year, date.Month, date.Day);

            //Nyårsdagen
            if (month == 1 && day == 1)
                return true;

            //Trettondedag jul
            if (month == 1 && day == 6)
                return true;

            DateTime easterDay = GaussEasterAlgorithm(date.Year);

            //Långfredagen, fredagen närmast före påskdagen
            if (easterDay.AddDays(-2) == dateTruncateTimeOfDay)
                return true;

            //Påskdagen, första söndagen efter vårdagsjämningen
            if (dateTruncateTimeOfDay == easterDay)
                return true;

            //Annandag påsk
            if (easterDay.AddDays(1) == dateTruncateTimeOfDay)
                return true;

            //Första maj
            if (month == 5 && day == 1)
                return true;

            //Kristi himmelsfärdsdag 39 dagar efter påskdagen
            if (easterDay.AddDays(39) == dateTruncateTimeOfDay)
                return true;

            //Pingstdagen 49 dagar efter påskdagen
            if (easterDay.AddDays(49) == dateTruncateTimeOfDay)
                return true;

            //Sveriges nationaldag
            if (month == 6 && day == 6)
                return true;

            //midsommardagen lördag mellan 20 26 juni
            DateTime midsummerDay = new DateTime(date.Year, 6, 20);

            while (midsummerDay.DayOfWeek != DayOfWeek.Saturday)
                midsummerDay = midsummerDay.AddDays(1);

            //midsommarafton dagen före midsommardagen
            if (midsummerDay.AddDays(-1) == dateTruncateTimeOfDay)
                return true;

            if (date == midsummerDay)
                return true;

            //Alla helgons dag lördag mellan 31 oktober och 6 november
            DateTime allSaintsday = new DateTime(date.Year, 10, 31);

            while (allSaintsday.DayOfWeek != DayOfWeek.Saturday)
                allSaintsday = allSaintsday.AddDays(1);

            if (dateTruncateTimeOfDay == allSaintsday)
                return true;

            //Julafton
            if (month == 12 && day == 24)
                return true;

            //Juldagen
            if (month == 12 && day == 25)
                return true;

            //Annandag jul
            if (month == 12 && day == 26)
                return true;

            //Nyårsafton
            if (month == 12 && day == 31)
                return true;

            return false;
        }

        /**
         * Use Gauss's easter alogrithm to calculate easterday
         *
         * @param year - the year we want to find easterday for
         * @return - date of easterday
         */
        public static DateTime GaussEasterAlgorithm(int year)
        {
            int a = year % 19;
            int b = year % 4;
            int c = year % 7;
            int k = year / 100;
            int p = (13 + 8 * k) / 25;
            int q = k / 4;
            int M = (15 - p + k - q) % 30;
            int N = (4 + k - q) % 7;
            int d = (19 * a + M) % 30;
            int e = (2 * b + 4 * c + 6 * d + N) % 7;

            if (d == 28 && e == 6)
                return new DateTime(year, 4, 18);

            if (d == 29 && e == 6)
                return new DateTime(year, 4, 19);

            int marchEaster = 22 + d + e;
            int aprilEaster = d + e - 9;

            if (marchEaster <= 31)
                return new DateTime(year, 3, marchEaster);

            return new DateTime(year, 4, aprilEaster);
        }
    }
}
