using TollFeeCalculator.Models;

namespace TollFeeCalculator
{
    // General comments about what I have done:
    // I refactored and simplfied the classes, I saw no real need to the interface so I removed it. Further more I made a project
    // in order to more easily create new files etc, just felt easier to work with. I tried to divide the code so that the
    // TollCalculator-class had only directly relevant code in it, then moved the other code to other classes where I felt it
    // belonged better. This is not tested since I was a bit out of time, instead I hope you will see as a suggestion of the 
    // way I would go. In a real world scenario this would of course be tested properly.

    public class TollCalculator
    {
        private const int LowFee = 8;
        private const int MediumFee = 13;
        private const int HighFee = 18;
        private const int MaxDailyFee = 60;

        // Moved all free dates to seperate static provider to maintain better readability 
        private readonly List<DateTime> tollFreeDates = TollFreeDatesProvider.TollFreeDates;


        //This method was refactored and also fixed since it seemed broken. The "DateTime previousDate = dates[0];"
        //was just checking the first date of the dateslist. This would not then check every new toll-date against the previous
        //to see if it occured within the last hour. Reused most of the logic inside since it looked thorough.

        // Added timespan instead of previous implementation and named the different fee-values for better readability.

        public int GetTollFee(Vehicle vehicle, DateTime[] dates)
        {
            // check vehicle directly here
            if (dates == null || dates.Length == 0 || vehicle.IsFreeVehicle)
                return 0;

            int totalFee = 0;
            DateTime previousDate = dates[0];
            
            foreach (DateTime currentDate in dates)
            {
                TimeSpan timeDifference = currentDate - previousDate;
                int minutes = (int)timeDifference.TotalMinutes;

                if (minutes > 60 || currentDate.Hour != previousDate.Hour)
                {
                    totalFee += GetTollFee(currentDate, vehicle);
                    previousDate = currentDate;
                }
                else
                {
                    int currentFee = GetTollFee(currentDate, vehicle);
                    if (currentFee > totalFee)
                    {
                        totalFee = currentFee;
                    }
                }
            }

            totalFee += GetTollFee(previousDate, vehicle);

            return totalFee > MaxDailyFee ? MaxDailyFee : totalFee;
        }

        // Refactored for better reabability, took some help online since I had a feeling a dictionary would be right
        // but I was not totally sure about the correct implementation. 
        public int GetTollFee(DateTime date, Vehicle vehicle)
        {
            if (IsTollFreeDate(date)) return 0;

            int hour = date.Hour;
            int minute = date.Minute;

            Dictionary<(int, int), int> tollFeeByTimeRange = new Dictionary<(int, int), int>
            {
                {(6, 0), LowFee},
                {(6, 30), MediumFee},
                {(7, 0), HighFee},
                {(8, 0), MediumFee},
                {(15, 0), MediumFee},
                {(16, 0), HighFee},
                {(17, 0), MediumFee},
                {(18, 0), LowFee}
            };

            // This just loops through each dictionary-item and check the Dictionary<(int, int) as item 1 and 2
            // against the parameter-date.
            foreach (var timeRange in tollFeeByTimeRange.Keys)
            {
                if (hour == timeRange.Item1 && minute >= timeRange.Item2 && minute <= timeRange.Item2 + 29)
                {
                    return tollFeeByTimeRange[timeRange];
                }
            }

            return 0;
        }


        // Remove previous function that was more dense but, in my opinion, a bit less readable, which is something I value. 
        // Here the IsTollFreeDate-function is broken up to a static list collection and a short concise function.
        private bool IsTollFreeDate(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday || tollFreeDates.Contains(date.Date);
        }
    }
}
