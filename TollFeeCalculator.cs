using Nager.Holiday;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using TollFeeCalculator;
using static TollFeeCalculator.Vehicle;

namespace TollFeeCalculator
{
    public class TollFeeCalculator
    {

        /**
        NOTES TO THE REVIEWER(S)

        - This project is targeted NET Runtime 8.0
        - It use the nuget package Nager.Holiday to check for holiday(s)
        - Use the console app (program.cs) to set up a new vehicle object, populate it with fees and then calculate the fees

        */

        /**
         * Calculate the total toll fee for one day
         *
         * @param vehicle - the vehicle
         * @param dates   - date and time of all passes on one day
         * @return - the total toll fee for that day
         */

        /// <summary>
        /// New toll fee is registered and, if valid, applied to the vehicle's list of fees
        /// </summary>
        public void NewTollFee(ref Vehicle vehicle, DateTime timeStamp)
        {
            try
            {
                // if toll free vehicle or date, no fee shall be applied
                if (IsTollFreeVehicle(vehicle) || IsTollFreeDateAndTime(timeStamp))
                    return;

                TollFee tollFee = new TollFee() { Fee = CalculateTollFee(vehicle, timeStamp), TimeStamp = timeStamp };

                // if it is a new toll fee within the same hour, only highest should be applied
                if (vehicle.TollFees.Any() && timeStamp <= vehicle.TollFees.Last().TimeStamp.AddHours(1))
                {
                    if (tollFee.Fee < vehicle.TollFees.Last().Fee)
                        vehicle.TollFees.Last().Fee = tollFee.Fee;
                }
                else
                {
                    // if not, apply the difference between the max fee 60 a day and the new fee
                    if (vehicle.GetTotalTollFee() + tollFee.Fee >= 60)
                        tollFee.Fee = (tollFee.Fee - ((vehicle.GetTotalTollFee() + tollFee.Fee) - 60));

                    vehicle.TollFees.Add(tollFee);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        
        // get all fees for all dates, maybe not needed because the function may be called directly
        public int GetTotalTollFee(Vehicle vehicle)
        {
            return vehicle.GetTotalTollFee();
        }

        // get all fess for a specific date, maybe not needed because the function may be called directly
        public int GetTotalTollFee(Vehicle vehicle, DateTime timeStamp)
        {
            return vehicle.GetTotalTollFee(timeStamp);
        }

        // list of fee rules, should be better to have in a DB...
        List<FeeRule> FeeRules = new List<FeeRule>()
        {

            new FeeRule() {
                Fee = 8, StartDateTime = DateTime.Parse("2024-01-01 06:00:00"), EndDateTime = DateTime.Parse("2024-01-01 06:30:00")
            },
            new FeeRule() {
                Fee = 13, StartDateTime = DateTime.Parse("2024-01-01 06:30:00"), EndDateTime = DateTime.Parse("2024-01-01 07:00:00")
            },
            new FeeRule() {
                Fee = 18, StartDateTime = DateTime.Parse("2024-01-01 07:00:00"), EndDateTime = DateTime.Parse("2024-01-01 08:00:00")
            },
            new FeeRule() {
                Fee = 13, StartDateTime = DateTime.Parse("2024-01-01 08:00:00"), EndDateTime = DateTime.Parse("2024-01-01 08:30:00")
            },
            new FeeRule() {
                Fee = 8, StartDateTime = DateTime.Parse("2024-01-01 08:30:00"), EndDateTime = DateTime.Parse("2024-01-01 15:00:00")
            },
            new FeeRule() {
                Fee = 13, StartDateTime = DateTime.Parse("2024-01-01 15:00:00"), EndDateTime = DateTime.Parse("2024-01-01 15:30:00")
            },
            new FeeRule() {
                Fee = 18, StartDateTime = DateTime.Parse("2024-01-01 15:30:00"), EndDateTime = DateTime.Parse("2024-01-01 17:00:00")
            },
            new FeeRule() {
                Fee = 13, StartDateTime = DateTime.Parse("2024-01-01 17:00:00"), EndDateTime = DateTime.Parse("2024-01-01 18:00:00")
            },
            new FeeRule() {
                Fee = 8, StartDateTime = DateTime.Parse("2024-01-01 18:00:00"), EndDateTime = DateTime.Parse("2024-01-01 18:30:00")
            },

        };

        // check if timeStamp has a fee rule and if yes return the fee
        private int CalculateTollFee(Vehicle vehicle, DateTime timeStamp)
        {
            try
            {
                return FeeRules
                    .Where(t => (timeStamp.TimeOfDay >= t.StartDateTime.TimeOfDay &&
                                 timeStamp.TimeOfDay < t.EndDateTime.TimeOfDay)).Select(f => f.Fee).FirstOrDefault();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 0;
            }
        }


        /// <summary>
        /// Motorbike = 0,
        /// Tractor = 1
        /// </summary>
        private List<int> TollFreeVehicleTypes { get; set; } = new List<int>() { 0, 1 };

        /// <summary>
        /// Emergency = 1,
        /// Diplomat = 2,
        /// Foreign = 3,
        /// Military = 4
        /// </summary>
        private List<int> TollFreeVehicleClasses { get; set; } = new List<int>() { 1, 2, 3, 4 };

        private bool IsTollFreeVehicle(Vehicle vehicle)
        {
            try
            {
                if (TollFreeVehicleTypes.Contains((int)vehicle.Type))
                    return true;
                if (TollFreeVehicleClasses.Contains((int)vehicle.Class))
                    return true;
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        private bool IsTollFreeDateAndTime(DateTime timeStamp)
        {
            try
            {
                if (timeStamp.DayOfWeek == DayOfWeek.Saturday || timeStamp.DayOfWeek == DayOfWeek.Sunday)
                    return true;

                using (HolidayClient holidayClient = new HolidayClient())
                {
                    PublicHoliday[] publicHolidays = holidayClient.GetHolidaysAsync(timeStamp.Year, "se").GetAwaiter().GetResult();
                    bool ret = publicHolidays.Any(h => h.Date == timeStamp.Date);
                    return ret;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}