using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace TollFeeCalculator
{
    public static class ApiHelper
    {
        private const string BaseURL = "https://holidays.abstractapi.com/v1/";
        private const string APIKey = "0383d039f45345608a90e50aec32297a";
        public static bool GetPublicHoliday(DateTime date)
        {

            var tokenRequest = WebRequest.Create(BaseURL + $"?api_key={APIKey}&country=SE&year={date.Year}&month={date.Month}&day={date.Day}");
            tokenRequest.Method = "GET";

            using var response = tokenRequest.GetResponse();
            //if length is 2 then its an empty array
            return response.ContentLength == 2 ? false : true;
        }
    }
}
