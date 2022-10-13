using System;
using System.Collections.Generic;

namespace TollFeeCalculator.Models
{
    public class TollFreeDate : IModel
    {
        public static List<TollFreeDate> AllTollFreeDates { get; set; } = new(); // For use while db not implemented.
        public DateTime Date { get; set; }
        public int Id { get; init; }

        public TollFreeDate(DateTime date)
        {
            this.Date = date;
            AllTollFreeDates.Add(this);
        }

        public TollFreeDate(int year, int month, int day)
        {
            this.Date = new DateTime(year, month, day);
            AllTollFreeDates.Add(this);
        }

        public static void SetTollFreeDates(int year)
        {
            // API call to trusted source to get all public holidays for specified year.
            // Add each date as TollFreeDate
        }

        public static void SetTollFreeDates2022() // For testing purposes
        {
            AllTollFreeDates.Add(new TollFreeDate(new DateTime(2022, 1, 5)));
            AllTollFreeDates.Add(new TollFreeDate(new DateTime(2022, 1, 6)));
            AllTollFreeDates.Add(new TollFreeDate(new DateTime(2022, 4, 14)));
            AllTollFreeDates.Add(new TollFreeDate(new DateTime(2022, 4, 15)));
            AllTollFreeDates.Add(new TollFreeDate(new DateTime(2022, 4, 18)));
            AllTollFreeDates.Add(new TollFreeDate(new DateTime(2022, 5, 25)));
            AllTollFreeDates.Add(new TollFreeDate(new DateTime(2022, 5, 26)));
            AllTollFreeDates.Add(new TollFreeDate(new DateTime(2022, 6, 6)));
            AllTollFreeDates.Add(new TollFreeDate(new DateTime(2022, 6, 24)));
            AllTollFreeDates.Add(new TollFreeDate(new DateTime(2022, 11, 4)));
            AllTollFreeDates.Add(new TollFreeDate(new DateTime(2022, 12, 26)));
        }

        public static void SetTollFreeMonth(int month) // Set specific month as toll free.
        {
            var currentYear = DateTime.Now.Year;
            var days = DateTime.DaysInMonth(currentYear, month);
            for (int i = 1; i <= days; i++)
            {
                AllTollFreeDates.Add(new TollFreeDate(new DateTime(currentYear, month, i)));
            }
        }

        public static bool InList(List<TollFreeDate> allDates, DateTime time)
        {
            foreach (var date in allDates)
            {
                if(date.Date == time.Date)
                {
                    return true;
                }
            }
            return false;
        }
    }
}