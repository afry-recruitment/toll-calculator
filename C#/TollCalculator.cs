using System;
using System.Globalization;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;


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

    public int GetTollFee(Vehicle vehicle, DateTime[] dates) 
    {
        //Check first if vehicle or date is toll free
        if (IsTollFreeVehicle(vehicle)|| IsTollFreeDate(dates[0])) return 0;

        DateTime intervalStart = dates[0];
        int totalFee = 0;
        int biggestFee = 0;
        int newFee = 0;

        for (int i = 0; i <= dates.Length - 1; i++)
        {

            TimeSpan comparedDates = dates[i].Subtract(intervalStart);

            if(comparedDates.TotalMinutes > 60)
            {
                //If it's more than 60min we start a new interval
                totalFee += biggestFee;
                intervalStart = dates[i];
                biggestFee = GetTollFee(dates[i]);
            } else
            {
                //If we are still in the same interval, compare new fee with biggest
                newFee = GetTollFee(dates[i]);
                if (newFee > biggestFee) biggestFee = newFee;

            }
        }
        totalFee += biggestFee;
        if (totalFee > 60) totalFee = 60;
        return totalFee;
    }

    private bool IsTollFreeVehicle(Vehicle vehicle)
    {
        if (vehicle == null) return false;
        String vehicleType = vehicle.GetVehicleType();
        return Enum.IsDefined(typeof(TollFreeVehicles), vehicleType);
    }

    public int GetTollFee(DateTime date)
    {

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

    private Boolean IsTollFreeDate(DateTime date) //Added api to check for red days
    {

        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;

        return CheckIfTollFree(date);

    }

        public bool CheckIfTollFree(DateTime date)
        {
            int year = date.Year;
            int month = date.Month;
            int day = date.Day;

            string urlDate = year.ToString() + "/" + month.ToString() + "/" + day.ToString();
            string url = "https://sholiday.faboul.se/dagar/v2.1/" + urlDate;

            WebRequest requestObj = WebRequest.Create(url);
            requestObj.Method = "GET";
            HttpWebResponse responseObj = null;
            responseObj = (HttpWebResponse)requestObj.GetResponse();

            string result = null;
            using (Stream stream = responseObj.GetResponseStream())
            {
                StreamReader sr = new StreamReader(stream);
                result = sr.ReadToEnd();
                Console.WriteLine(result);
                sr.Close();
            }

            JObject jsonResult = JObject.Parse(result);
            var dag = jsonResult["dagar"];
            JObject dagen = (JObject)dag[0];

            if (dagen["röd dag"].ToString() == "Ja")
            {
                return true;
            }
            else
            {
                return false;
            }


        }

        private enum TollFreeVehicles
    {
        Motorbike = 0,
        Tractor = 1,
        Emergency = 2,
        Diplomat = 3,
        Foreign = 4,
        Military = 5
    }
}
}