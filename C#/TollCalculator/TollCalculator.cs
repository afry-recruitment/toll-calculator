using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices.Marshalling;
using System.Runtime.Serialization.Formatters;
using TollCalculator.Interfaces;

namespace TollCalculator
{
    public class TollCalculator
    {

        /**
         * Calculate the total toll fee for one day
         *
         * @param vehicle - the vehicle
         * @param dates   - date and time of all passes on one day
         * @return - the total toll fee for that day
         */

        //uträkning för total avgift, se över om den fungerar korrekt senare! 
        public int GetTollFee(IVehicle vehicle, DateTime[] dates)
        {
            if (dates == null || dates.Length == 0) return 0;

            Array.Sort(dates);

            DateTime intervalStart = dates[0];
            int currentWindowMaxFee = GetTollFee(intervalStart, vehicle);
            int totalFee = 0;

            for (int i = 1; i < dates.Length; i++)
            {
                var date = dates[i];
                int fee = GetTollFee(date, vehicle);

                double minutes = (date - intervalStart).TotalMinutes;

                if (minutes <= 60)
                {
                    if (fee > currentWindowMaxFee)
                    {
                        if (fee > currentWindowMaxFee) currentWindowMaxFee = fee;
                    }
                }
                else
                {
                    totalFee += currentWindowMaxFee;
                    intervalStart = date;
                    currentWindowMaxFee = fee;
                }
            }

            //foreach (DateTime date in dates)
            //{
            //    int nextFee = GetTollFee(date, vehicle);
            //    int tempFee = GetTollFee(intervalStart, vehicle);

            //    double diffInMinutes = (date - intervalStart).TotalMinutes;

            //    long minutes = date.Millisecond - intervalStart.Millisecond;
            //    //long minutes = diffInMillies / 1000 / 60;

            //    if (minutes <= 60)
            //    {
            //        if (totalFee > 0) totalFee -= tempFee;
            //        if (nextFee >= tempFee) tempFee = nextFee;
            //        totalFee += tempFee;
            //    }
            //    else
            //    {
            //        totalFee += nextFee;
            //    }

            totalFee += currentWindowMaxFee;
            //The maximum fee for one day is 60 SEK
            if (totalFee > 60) totalFee = 60;
            return totalFee;
        }

        //fordon som är tullfria
        private bool IsTollFreeVehicle(IVehicle vehicle)
        {
            if (vehicle == null) return false;
            String vehicleType = vehicle.GetVehicleType();
            return vehicleType.Equals(TollFreeVehicles.Motorbike.ToString()) ||
                   vehicleType.Equals(TollFreeVehicles.Tractor.ToString()) ||
                   vehicleType.Equals(TollFreeVehicles.Emergency.ToString()) ||
                   vehicleType.Equals(TollFreeVehicles.Diplomat.ToString()) ||
                   vehicleType.Equals(TollFreeVehicles.Foreign.ToString()) ||
                   vehicleType.Equals(TollFreeVehicles.Military.ToString());
        }

        //datum för priserna. Avigftsfia datum returnerar noll (IsTollFreeDate och IsTollFreeVehicle)
        public int GetTollFee(DateTime date, IVehicle vehicle)
        {
            if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;

            int hour = date.Hour;
            int minute = date.Minute;

            if (hour == 6 && minute >= 0 && minute <= 29) return 8;
            else if (hour == 6 && minute >= 30 && minute <= 59) return 13;
            else if (hour == 7 && minute >= 0 && minute <= 59) return 18;
            else if (hour == 8 && minute >= 0 && minute <= 29) return 13;
            else if (hour >= 8 && hour <= 14 && minute >= 30 && minute <= 59) return 8;
            else if (hour == 15 && minute >= 0 && minute <= 29) return 13;
            else if (hour == 15 && minute >= 0 || hour == 16 && minute <= 59) return 18;
            else if (hour == 17 && minute >= 0 && minute <= 59) return 13;
            else if (hour == 18 && minute >= 0 && minute <= 29) return 8;
            else return 0;
        }

        //datum som är tullfria. hårdkodad (2013, kommer inte fungera i framtiden)
        private Boolean IsTollFreeDate(DateTime date)
        {
            int year = date.Year;
            int month = date.Month;
            int day = date.Day;

            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;

            //AI, sätta röda datum för månaderna
            var fixedHoldays = new HashSet<string>
            {
                "1-1",   // Nyårsdagen
                "3-28",  // Skärtorsdagen
                "3-29",  // Långfredagen
                "4-1",   // Påskdagen
                "4-30",  // Valborgsmässoafton
                "5-1",   // Första maj
                "5-8",   // Kristi himmelsfärdsdag
                "5-9",   // Kristi himmelsfärdsdag
                "6-5",   // Nationaldagen
                "6-6",   // Nationaldagen
                "6-21",  // Midsommarafton
                "7-1",   // Sveriges nationaldag (om den infaller på en lördag)
                "11-1",  // Alla helgons dag
                "12-24", // Julafton
                "12-25", // Juldagen
                "12-26", // Annandag jul
                "12-31"  // Nyårsafton  
            };

            //använder date för att jämföra dagens datum med de röda dagarna för att se om det är en röd dag eller inte
            string key = $"{date.Month}-{date.Day}";
            if (fixedHoldays.Contains(key)) return true;


            return false;
        }

        //lista för tullfria fordon
        private enum TollFreeVehicles
        {
            Motorbike = 0,
            Tractor = 1,
            Emergency = 2,
            Diplomat = 3,
            Foreign = 4,
            Military = 5
        }

        private enum TollVehicles
        {
            Car = 0,
        }
    }
}
