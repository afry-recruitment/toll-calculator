using System;
using System.Net;
using System.Threading;

namespace TollFeeCalculator
{
    public static class ApiHelper
    {
        private const string BaseURL = "https://holidays.abstractapi.com/v1/";
        private const string APIKey = Secrets.API_KEY;

        public static bool GetPublicHoliday(DateTime date)
        {
            var tokenRequest = WebRequest.Create(BaseURL + $"?api_key={APIKey}&country=SE&year={date.Year}&month={date.Month}&day={date.Day}");
            tokenRequest.Method = "GET";
            Thread.Sleep(1000);
            using var response = tokenRequest.GetResponse();
            //if length is 2 then its an empty array
            return response.ContentLength == 2 ? false : true;
        }
    }
}