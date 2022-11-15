using Helpers;
using System;
using System.Configuration;
using System.Globalization;
using TollCalculator.Models;
using TollCalculator.ViewModels;

namespace TollCalculator
{
    public class TollCalculator
    {
        /// <summary>
        /// Calculate the total toll fee for one day
        /// </summary>
        /// <returns>The total toll fee for that day</returns>
        public int GetTotalTollFeeByDay(TollVehicleViewModel vehicle)
        {
            vehicle.TollPassesDuringDay = OrderTollPassesByTime(vehicle.TollPassesDuringDay);

            DateTime intervalStart = vehicle.TollPassesDuringDay.FirstOrDefault();

            //Variable to keep track of previous date/time
            //before the next date is being processed. 
            DateTime previousDate = DateTime.MinValue;

            int totalFeeOutput = 0;

            foreach (DateTime date in vehicle.TollPassesDuringDay)
            {
                int nextTollFee = GetTollFee(date, vehicle.Vehicle);

                int temporaryTollFee = GetTollFee(intervalStart, vehicle.Vehicle);

                TimeSpan differenceInTimeBetweenPreviousTollPass = TimeSpan.Zero;

                if (IsFirstTollPass(previousDate))
                {
                    differenceInTimeBetweenPreviousTollPass = RemoveFirstTollPassFromDiffrenceInTime(intervalStart, date);
                }
                else
                {
                    differenceInTimeBetweenPreviousTollPass = date - previousDate;
                }

                //Todo: A vehicle should only be charged once an hour
                if (differenceInTimeBetweenPreviousTollPass.TotalMinutes <= 60)
                {
                    if (totalFeeOutput > 0)
                        totalFeeOutput -= temporaryTollFee;

                    //In the case of multiple fees in the same hour period, the highest one applies.
                    if (nextTollFee >= temporaryTollFee)
                        temporaryTollFee = nextTollFee;

                    totalFeeOutput += temporaryTollFee;
                }
                else
                {
                    totalFeeOutput += nextTollFee;
                }

                previousDate = date;
            }

            if (totalFeeOutput > 60) totalFeeOutput = 60;

            return totalFeeOutput;
        }

        private TimeSpan RemoveFirstTollPassFromDiffrenceInTime(DateTime intervalStart, DateTime date) 
            => date - intervalStart;

        private bool IsFirstTollPass(DateTime previousDate)
        {
            return previousDate != DateTime.MinValue;
        }

        private DateTime[] OrderTollPassesByTime(DateTime[] dateAndTimeOfAllTollPassesDuringOneDay)
        {
            dateAndTimeOfAllTollPassesDuringOneDay = dateAndTimeOfAllTollPassesDuringOneDay.OrderBy(x => x.TimeOfDay).ToArray();

            return dateAndTimeOfAllTollPassesDuringOneDay;
        }

        private bool IsTollFreeVehicle(IVehicle vehicle)
        {
            if (vehicle is ITollObligatedVehicle)
                return false;

            return vehicle is ITollFreeVehicle;
        }

        public int GetTollFee(DateTime date, IVehicle vehicle)
        {
            if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;

            int hour = date.Hour;
            int minute = date.Minute;

            if (hour == 6 && minute >= 0 && minute <= 29) return 8;
            else if (hour == 6 && minute >= 30 && minute <= 59) return 13;
            else if (hour == 7 && minute >= 0 && minute <= 59) return 18;
            else if (hour == 8 && minute >= 0 && minute <= 29) return 13;

            //8.30 >= <= 14.59
            else if (hour >= 8 && minute >= 30 && hour <= 14 && minute <= 59)
                return 8;

            else if (hour == 15 && minute >= 0 && minute <= 29) return 13;
            else if (hour == 15 && minute >= 0 || hour == 16 && minute <= 59) return 18;
            else if (hour == 17 && minute >= 0 && minute <= 59) return 13;
            else if (hour == 18 && minute >= 0 && minute <= 29) return 8;
            else return 0;
        }

        private bool IsTollFreeDate(DateTime date)
        {
            if (date.DayOfWeek is DayOfWeek.Saturday || date.DayOfWeek is DayOfWeek.Sunday) return true;

            return HolidayValidator.GetPublicHoliday(date, "sv-SE");
        }
    }
}