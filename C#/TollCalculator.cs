using System;
using System.Globalization;
using TollFeeCalculator;

namespace TollFeeCalculator
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

        static void Main(string[] args)
        {
            TollCalculator calculator = new TollCalculator();

            /*Given that there was no dummy data below, a method should be introduced to retrive data from the source relevant in this scenario.
            The incoming data should follow a format that is aligned with the vehicle classification and date format, if it does not, functionality  
            to convert the incoming data into a format that can be managed by the solution will have to be added. */

            //GetData()

            // Create a vehicle instance. 
            //In a real life sceanrio it should be entered as a dynamic parameter
            Vehicle vehicle = new Vehicle(VehicleType.Car);

            // Dates and times the vehicle passed the toll station one day.
            //In a real life scenario it should be entered as a dynamic parameter.
         
            DateTime[] passes = new DateTime[]
            {
            new DateTime(2013, 2, 8, 6, 15, 0),  // 06:15  (Fee: 8 SEK)
            new DateTime(2013, 2, 8, 7, 0, 0),   // 07:00  (Fee: 18 SEK)
            new DateTime(2013, 2, 8, 15, 27, 0), // 15:27  (Fee: 18 SEK)
         

            };

           


            //   Call GetTollFee and capture the calculated integer result
            int totalFee = calculator.GetTollFee(vehicle, passes);

            //Output of the total fee of the day. Could be added to a log or to an export file in a real life scenario.
            Console.WriteLine($"Total toll fee is: {totalFee} SEK");
        }



        public int GetTollFee(Vehicle vehicle, DateTime[] dates)
        {
            if (dates == null || dates.Length == 0) return 0;
            DateTime intervalStart = dates[0];
            int totalFee = 0;
            int maxFeeInInterval = 0;

            foreach (DateTime date in dates)
            {
                int currentFee = GetTollFee(date, vehicle);

                // Check if next fee is within 60 minutes of interval start
                if ((date - intervalStart).TotalMinutes <= 60)
                {
                    if (currentFee > maxFeeInInterval)
                    {
                        totalFee = totalFee - maxFeeInInterval + currentFee;
                        maxFeeInInterval = currentFee;
                    }
                }

                // More than 60-minutes have passed and a new "fee window" is opened
                else
                {
                    intervalStart = date;
                    maxFeeInInterval = currentFee;
                    totalFee += currentFee;
                }
            }

            // Fee limit per day =  60 SEK
            if (totalFee > 60) totalFee = 60;

            return totalFee;
        }


        //Extract the current hour and minute count and compare it to the fee spans
        public int GetTollFee(DateTime date, Vehicle vehicle)
        {
            if (IsTollFreeDate(date) == true || IsTollFreeVehicle(vehicle) == true) return 0;

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

        //Check if the date is a weekend or bank holiday 
        private Boolean IsTollFreeDate(DateTime date)
        {
            int year = date.Year;
            int month = date.Month;
            int day = date.Day;

            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;

            if (year == 2013)
            {
                if (month == 1 && day == 1 ||
                    month == 3 && (day == 28 || day == 29) ||
                    month == 4 && (day == 1 || day == 30) ||
                    month == 5 && (day == 1 || day == 8 || day == 9) ||
                    month == 6 && (day == 5 || day == 6 || day == 21) ||
                    month == 7 ||
                    month == 11 && day == 1 ||
                    month == 12 && (day == 24 || day == 25 || day == 26 || day == 31))
                {
                    return true;
                }
            }
            return false;
        }


        //Check what vehicle type the current vehcile has and set a boolean to true or false depending on the type
        private bool IsTollFreeVehicle(Vehicle vehicle)
        {
            if (vehicle == null) return false;

            switch (vehicle.Type)
            {
                case VehicleType.Motorbike:
                case VehicleType.Tractor:
                case VehicleType.Emergency:
                case VehicleType.Diplomat:
                case VehicleType.Foreign:
                case VehicleType.Military:
                    return true;

                default:
                    return false;
            }
        }
    }
}
