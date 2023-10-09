using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toll_Fee_Calculator
{
    public class TollCalculator
    {

        Holiday[] Holidays { get; set; }
        Vehicle[] Vehicles { get; set; }

        /**
         * Constructor to initiate the TollCalculator
         *
         * @param Holidays - list of all holidays in a single year
         * @param Vehicles - list of all vehicles passed through toll
         */
        public TollCalculator(Holiday[] holidays, Vehicle[] vehicles)
        {
            this.Holidays = holidays;
            this.Vehicles = vehicles;
        }

        /**
         * Simulate over all the vehicles one by one to calculate total toll fee for each day
         *
         */
        public void TollCalculatorSimulator()
        {
            for (int i = 0; i < Vehicles.Length; i++)
            {
                Vehicles.ElementAt(i).TotalTollFee = GetTotalTollFee(Vehicles.ElementAt(i));
                foreach (var item in Vehicles.ElementAt(i).TotalTollFee)
                {
                    Console.WriteLine("Total Toll Fee of Vehicle-" + (i + 1) + " " + Vehicles.ElementAt(i).GetVehicleType() + " on date " + item.Key + ": " + item.Value);
                }
            }
        }
        /**
         * Calculate the total toll fee for each day
         *
         * @param vehicle - the vehicle
         * @return - the total toll fee for each day as dictionary
         */
        public Dictionary<DateTime, int> GetTotalTollFee(Vehicle vehicle)
        {
            DateTime intervalStart = vehicle.Dates[0];
            int totalFee = 0;
            Dictionary<DateTime, int> totalTollFee = new Dictionary<DateTime, int>();
            for (int i = 0; i < vehicle.Dates.Length; i++)
            {
                DateTime date = vehicle.Dates[i];
                int nextFee = GetTollFee(date, vehicle);
                int tempFee = GetTollFee(intervalStart, vehicle);
                TimeSpan ts = date - intervalStart;
                if(ts.TotalDays >= 1)
                {
                    totalFee = 0;
                }
                if (ts.TotalMinutes <= 60)
                {
                    if (totalFee > 0) totalFee -= tempFee;
                    if (nextFee >= tempFee) tempFee = nextFee;
                    totalFee += tempFee;
                }
                else
                {
                    intervalStart = date;
                    totalFee += nextFee;
                }
                if (totalFee > 60) totalFee = 60;
                totalTollFee[date.Date] = totalFee;
            }
            return totalTollFee;
        }


        /**
         * Get Toll Fee of vehicle for a single instance in a day
         *
         * @param Vehicle - the vehicle
         * @param Datetime - the datetime of vehicle when it crosses toll
         * @return - the toll fee of that datetime
         */
        private int GetTollFee(DateTime date, Vehicle vehicle)
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
            else if (hour == 15 && minute >= 30 || hour == 16 && minute <= 59) return 18;
            else if (hour == 17 && minute >= 0 && minute <= 59) return 13;
            else if (hour == 18 && minute >= 0 && minute <= 29) return 8;
            else return 8;
        }

        /**
         * Check if it is toll free vehicle or not
         *
         * @param Vehicle - the vehicle
         * @return - true or false
         */
        private bool IsTollFreeVehicle(Vehicle vehicle)
        {
            if (vehicle == null) return false;
            return vehicle.IsTollFree;
        }

        /**
         * Check if it is toll free Date or not
         *
         * @param DateTime - the datetime of vehicle when it crosses toll
         * @return - true or false
         */
        private Boolean IsTollFreeDate(DateTime date)
        {
            int year = date.Year;
            int month = date.Month;
            int day = date.Day;

            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;

            if (year == 2023)
            {
                foreach (var holiday in Holidays)
                {
                    if(holiday.Equals(new Holiday(date.Day, date.Month)))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

    }
}
