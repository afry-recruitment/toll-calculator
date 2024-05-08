// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Globalization;
using TollFeeCalculator;

public class Program
{
    static void Main() 
    {
        Vehicle theVehicle = WhatAreYouDriving();
        
        // Set a date
        DateTime createdDate = RandomDateFrom2013(); 
        Console.WriteLine("Date created: " + createdDate.ToString("yyyy-MM-dd") + ", a " + createdDate.DayOfWeek);

        // Check if date is holliday or not
        bool isHolliday = TollCalculator.IsTollFreeDate(createdDate);
        if (isHolliday && (createdDate.DayOfWeek != DayOfWeek.Saturday || createdDate.DayOfWeek != DayOfWeek.Sunday)) {
            Console.WriteLine("No charge today, it's a holliday!");
            System.Environment.Exit(0);
        }

        // Create trips and fees
        int totalTollFee = CreateTripsAndFees(createdDate);

        // Max fee for one day is 60 SEK.
        if (totalTollFee > 60) totalTollFee = 60;

        // Result of everything
        Console.WriteLine("\nTotal toll fee: " + totalTollFee + " SEK.");
    }

    static Vehicle WhatAreYouDriving() 
    {
        /* CHOOSE YOUR VEHICLE */
        Vehicle theVehicle = new Car();
        string vehicleType = "";
        bool isTollFree = false;
        int numberOfFailedAttempts = 0;

        do {
            Console.WriteLine("Choose your vehicle by number: \n 1. Car  \n 2. Motorbike");
            string chosenVehicle = Console.ReadLine();

            if (chosenVehicle == "1") {
                /* CAR */
                Vehicle blueCar = new Car();
                vehicleType = blueCar.GetVehicleType();
                isTollFree = TollCalculator.IsTollFreeVehicle(blueCar);
                theVehicle = blueCar;
                break;
            }
            if (chosenVehicle == "2") { 
                /* MOTORBIKE */
                Vehicle redMotorbike = new Motorbike();
                vehicleType = redMotorbike.GetVehicleType();
                isTollFree = TollCalculator.IsTollFreeVehicle(redMotorbike);
                theVehicle = redMotorbike;
                break;
            }
        
            numberOfFailedAttempts++;
            Console.WriteLine("(Number of attemtps: " + numberOfFailedAttempts + ")");
        }
        while (numberOfFailedAttempts >= 1 && numberOfFailedAttempts <= 2);
        
        if (numberOfFailedAttempts == 3) {
            Console.WriteLine("* System shut down after too many failed attempts. *");
            System.Environment.Exit(0);
        }

        if (isTollFree) {
            Console.WriteLine("\nYou're driving a " + vehicleType + ".\n\nHave a nice Toll-Free day! =)");
            System.Environment.Exit(0);
        }
        else {
            Console.WriteLine("\nYou're driving a " + vehicleType + ".");
            Console.WriteLine("You will be charged between 6:00 am and 6:30 pm to a maximum of 60 SEK/day on every holliday-free monday to friday.\n");
        }

        return theVehicle;
    }

    private static DateTime RandomDateFrom2013() 
    {
        Random rnd = new Random();
        int randomMonth = rnd.Next(1, 12);
        int randomDay = 0;

        if (randomMonth == 2) randomDay = rnd.Next(1, 28);
        if (randomMonth % 2 == 0) randomDay = rnd.Next(1, 30);
        else randomDay = rnd.Next(1, 31);

        DateTime date = new DateTime(2013, randomMonth, randomDay, 4, 0, 0, 0);

        return date;
    }

    private static int CreateTripsAndFees(DateTime createdDate) 
    {
        // Randomize the number of trips during that date
        Random rndTrips = new Random();
        int amountOfTrips = rndTrips.Next(1,4);
        Console.WriteLine("Amount of trips: " + amountOfTrips);
        
        int hourCount = 0;
        int tripCount = 0;
        int totalTollFee = 0;
        for (int i = 0; i < amountOfTrips; i++) 
        {
            // Create a list of date times
            List<DateTime> dateTimes = new List<DateTime>();
            //dateTimes.ForEach(i => Console.Write("{0}\t", i));
            
            // Set hour of the trip (Start hour of the day is set to 4 AM when random date is being created)
            Random rnd = new Random();
            int randomHour = rnd.Next(1, 6);
            hourCount += randomHour;
            DateTime newTimeStamp = createdDate.AddHours(hourCount);
            int setMinutes = rnd.Next(1,59);
            newTimeStamp = newTimeStamp.AddMinutes(setMinutes);
            
            
            tripCount++;
            Console.WriteLine("\nTrip #" + tripCount + newTimeStamp.ToString(" HH:mm"));
            
            // Randomize the number of passages each trip
            Random rndPassages = new Random();
            int amountOfPassages = rndPassages.Next(1,10);
            Console.WriteLine("Amount of passages: " + amountOfPassages);
            
            // Create time stamps for each passage and add to the dateTimes array
            int passagesCount = 0;
            do 
            {
                // Set random amount of minutes between each toll passing within the same trip
                Random rndMinutes = new Random();
                int randomMinutes = rndMinutes.Next(5,15);

                newTimeStamp = newTimeStamp.AddMinutes(randomMinutes);
                //Console.WriteLine(newTimeStamp.ToString("yyyy-MM-dd HH:mm"));
                dateTimes.Add(newTimeStamp);

                passagesCount++;
            }
            while (passagesCount != amountOfPassages);

            if (hourCount >= 24) break;

            // Convert list to array
            DateTime[] dateTimeArray = dateTimes.ToArray();
            Console.WriteLine("dateTimeArray:");
            foreach(var item in dateTimeArray)
            {
                Console.WriteLine(item.ToString("yyyy-MM-dd HH:mm"));
            }

            int tollFee = CalculateTollFee(dateTimeArray).Result;
            Console.WriteLine("\nPartial toll fee: " + tollFee + " SEK.");
            totalTollFee += tollFee;
            Console.WriteLine("Total toll fee: " + totalTollFee + " SEK.");
        }
        return totalTollFee;
    }

    public static async Task<int> CalculateTollFee(DateTime[] dates)
    {
        DateTime intervalStart = dates[0].Date;
        DateTime intervalLast = dates.Last();
        // Console.WriteLine("dates[0].date = " + intervalStart.ToString("yyyy/MM/dd HH:mm"));
        Console.WriteLine("First time entry: " + dates[0].ToString("yyyy/MM/dd HH:mm"));
        Console.WriteLine("Last time entry: " + intervalLast.ToString("yyyy/MM/dd HH:mm"));
        DateTime intervalCompare = dates[0].Date;
        TimeSpan timeLimit = new TimeSpan(1, 0, 0);
        int highestFee = 0;
        int compareFee = 0;
        int totalFee = 0;
        int partialTollFee = 0;
        
        int passagesCount = 1;
        foreach (DateTime date in dates)
        {
            Console.WriteLine("\npassage nr: " + passagesCount + " Time: " + date.ToString("yyyy/MM/dd HH:mm"));
            passagesCount++;
            if (date.Hour >= 6 && date.Hour <= 18 ) //added if-statement to rule out hours before 6:00 och after 18:30
            { 
                // Set compare time, set fee for that start time
                intervalCompare = date;
                compareFee = await Task.Run(() => TollCalculator.GetTollFee(date) );

                // Check next time - is it a higher fee? is duration within 60 minutes? then set higher fee
                // is duration after 60 minutes - add fee
                if (intervalStart.Hour == 0 || dates.Length == 1) {
                    intervalStart = date;
                    Console.WriteLine("intervalStart: " + intervalStart.ToString("yyyy/MM/dd HH:mm"));
                    highestFee = compareFee;
                }
                else {
                    TimeSpan timeDifference = intervalCompare - intervalStart;
                    Console.WriteLine("intervalStart: " + intervalStart.ToString("yyyy/MM/dd HH:mm"));
                    Console.WriteLine("intervalCompare: " + intervalCompare.ToString("yyyy/MM/dd HH:mm"));
                    Console.WriteLine("timeDifference: " + timeDifference);
                    
                    if (timeDifference < timeLimit) 
                    {
                        if (compareFee > highestFee) {
                            highestFee = compareFee;
                            Console.WriteLine("Highest fee changed to: " + highestFee + " SEK.");
                        }
                    }
                    else {
                        highestFee = compareFee;
                        intervalStart = date;
                        partialTollFee += totalFee;
                        Console.WriteLine("Partial toll fee: " + partialTollFee + " SEK.");

                        // if (date == intervalLast) {
                        //     partialTollFee += highestFee;
                        // }
                    }
                }
                totalFee = partialTollFee + highestFee;
                Console.WriteLine("Highest fee: " + highestFee + " SEK.");
                Console.WriteLine("Compare fee: " + compareFee + " SEK."); 
                Console.WriteLine("Total trip fee: " + totalFee + " SEK.");
            }
        }

        if (totalFee > 60) totalFee = 60;
        return totalFee;
    }

}

