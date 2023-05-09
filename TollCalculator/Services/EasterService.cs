
// Code nicked from : 
// https://github.com/DannyBoyNg/CalculateEasterDate/blob/master/CalculateEasterDateForYear/Easter.cs
// it contains a lot of one letter variables, but 
//  Im treating this as a black box, and adding some tests around it, choosing not 
// to fiddle with the code. 
// check this article for an explanation of how easter is calculated: 
// https://www.geeksforgeeks.org/how-to-calculate-the-easter-date-for-a-given-year-using-gauss-algorithm/

namespace TollFeeCalculator
{
  public class EasterService
  {
    static Dictionary<int, DateTime> cache = new Dictionary<int, DateTime>();

    public static DateTime EasterDateForYear(int? yearArg = null)
    {
      int year = (yearArg == null) ? DateTime.Now.Year : year = yearArg.Value;
      if (cache.ContainsKey(year)) return cache[year];
      int day = 0;
      int month = 0;

      int g = year % 19;
      int c = year / 100;
      int h = (c - c / 4 - (8 * c + 13) / 25 + 19 * g + 15) % 30;
      int i = h - h / 28 * (1 - h / 28 * (29 / (h + 1)) * ((21 - g) / 11));

      day = i - ((year + year / 4 + i + 2 - c + c / 4) % 7) + 28;
      month = 3;
      if (day > 31)
      {
        month++;
        day -= 31;
      }

      var result = new DateTime(year, month, day);
      cache.Add(year, result);
      return result;
    }
  }
}