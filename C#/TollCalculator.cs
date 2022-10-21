using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using TollFeeCalculator;

public class TollCalculator
{

    public string[] TollFreeVehicles = { "Motorbike", "Tractor", "Emergency", "Diplomat", "Foreign", "Military" };
    private string path = @"C:\RedDayDates.txt";

    /**
     * Calculate the total toll fee for one day
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */

    public int GetTollFees(string vehicle, DateTime[] dates)
    {
        DateTime intStart = dates[0];
        int maxFee = 0;

        int totalFee = 0;
        for (int i = 0; i < dates.Length; i++)
        {
            int tempFee = 0;

            var timeDifference = dates[i].Subtract(intStart);

            if (timeDifference.TotalMinutes > 60)
            {
                totalFee += maxFee;
                intStart = dates[i];
                maxFee = GetTollFee(dates[i], vehicle);
            }
            else
            {
                tempFee = GetTollFee(dates[i], vehicle);
                if (tempFee > maxFee) maxFee = tempFee;
            }
        }

        totalFee += maxFee;

        if (totalFee > 60) totalFee = 60;
        return totalFee;
    }

    private bool IsTollFreeVehicle(string vehicle)
    {
        for (int i = 0; i < TollFreeVehicles.Length; i++)
        {
            if (TollFreeVehicles[i].ToLower().Contains(vehicle.ToLower())) return true;
        }
        return false;
    }


    private int GetTollFee(DateTime date, string vehicle)
    {
        if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;

        int hour = date.Hour;
        int minute = date.Minute;

        if (hour == 6 && minute <= 29) return 8;
        else if (hour == 6 && minute <= 59) return 13;
        else if (hour == 7 && minute <= 59) return 18;
        else if (hour == 8 && minute <= 29) return 13;
        else if (hour >= 8 && hour <= 14 && minute <= 59) return 8;
        else if (hour == 15 && minute <= 29) return 13;
        else if ((hour >= 15 && hour <= 16 && minute <= 59)) return 18;
        else if (hour == 17 && minute <= 59) return 13;
        else if (hour == 18 && minute <= 29) return 8;
        else return 0;

    }

    private Boolean IsTollFreeDate(DateTime date)
    {
        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday || IsRedDay(date)) return true;

        return false;
    }



    private Boolean IsRedDay(DateTime date)
    {

        List<DateTime> Holiday = new List<DateTime>();

        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;

        using (StreamReader sr = new StreamReader(path))
        {
            while (sr.Peek() >= 0)
            {
                string str;
                string[] strArray;
                str = sr.ReadLine();

                strArray = str.Split(',');

                Holiday.Add(new DateTime(date.Year, int.Parse(strArray[0]), int.Parse(strArray[1])));

            }
        }
        if (Holiday.Contains(date.Date)) return true;

        
        return false;

    }
}