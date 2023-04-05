using System;
using System.Collections.Generic;

namespace TollFeeCalculator
{
    public class TollFeeCalculator
    {
        private readonly Dictionary<DateTime, int> _hourlyFees;

        public TollFeeCalculator()
        {
            _hourlyFees = new Dictionary<DateTime, int>();
        }

        public int GetTollFee(DateTime[] dates, string vehicleType)
        {
            if (IsFeeFree(vehicleType) || IsFeeFreeDay(dates[0]))
            {
                return 0;
            }

            int totalFee = 0;
            int lastHour = -1;

            foreach (var date in dates)
            {
                if (IsFeeFreeDay(date))
                {
                    continue;
                }

                int fee = GetFee(date);
                int hour = date.Hour;

                if (hour == lastHour)
                {
                    if (fee > totalFee)
                    {
                        totalFee = fee;
                    }
                }
                else
                {
                    totalFee += fee;
                    lastHour = hour;
                }

                if (totalFee >= 60)
                {
                    return 60;
                }
            }

            return totalFee;
        }

        private int GetFee(DateTime date)
        {
            if (date.Hour < 6 || date.Hour > 18)
            {
                return 8;
            }
            else if (date.Hour < 7 || date.Hour >= 17)
            {
                return 13;
            }
            else
            {
                return 18;
            }
        }

        private bool IsFeeFree(string vehicleType)
        {
           
            return
                vehicleType == "Bus" ||
                vehicleType == "Police" ||
                vehicleType == "Ambulance" ||
                 vehicleType == "Bicycle" ||
                vehicleType == "Emergency";

        }

        private bool IsFeeFreeDay(DateTime date)
        {
            //We can add more holydays if we need!
            return


            (date.DayOfWeek == DayOfWeek.Saturday) ||
            (date.DayOfWeek == DayOfWeek.Sunday) ||
             (date.Month == 1 && date.Day == 1) || 
             (date.Month == 12 && date.Day == 24) || 
             (date.Month == 12 && date.Day == 25) || 
             (date.Month == 12 && date.Day == 31); 
        }
    }
}