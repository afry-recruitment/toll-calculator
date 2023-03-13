using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TollCalculator.Data;

namespace TollCalculator.Repos
{
    public class PassingDate
    {
        private static bool IsRedDay(DateTime passingDate)
        {
            using (HttpClient client = new())
            {
                // using free online api for free days
                const string uri = "https://sholiday.faboul.se/dagar/v2.1/";
                var uriWithDate = uri
                    + passingDate.Year
                    + "/" + passingDate.Month
                    + "/" + passingDate.Day;

                var response = client.GetAsync(uriWithDate).Result;
                if (response.IsSuccessStatusCode)
                {
                    var resultstring = response.Content.ReadAsStringAsync().Result;
                    if (JObject.Parse(resultstring)["dagar"][0]["röd dag"].ToString() == "Ja")
                        return true;
                }
                else
                    throw new Exception("api : https://sholiday.faboul.se/dagar/v2.1/ seems not be down, try later ");
            }
            return false;

        }

        public static bool IsDateTollDate(DateTime passingDate)
        {
            // no toll Red day
            if (IsRedDay(passingDate))
                return false;

            // no toll before red day I assume, but it not clear by spec. 
            var dayAfterPassingDate = passingDate.AddDays(1);
            if (IsRedDay(dayAfterPassingDate))
                return false;

            // Saturday and sundday free
            if (passingDate.DayOfWeek == DayOfWeek.Saturday
                || passingDate.DayOfWeek == DayOfWeek.Sunday)
                return false;

            //// July free in Gothenburg but not here as I understand
            //if (passingDate.Month == 7)
            //    return false;

            // then toll
            return true;
        }
    }
}
