using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TollCalculatorAfry.Models;

namespace TollCalculatorAfry.Services
{
    public class TollService : ITollService
    {
        public static List<Fee> Range => new List<Fee>
        {
            new Fee(TimeSpan.FromMinutes(6 * 60), TimeSpan.FromMinutes(6 * 60 + 29), 8),
            new Fee(TimeSpan.FromMinutes(6 * 60 + 30), TimeSpan.FromMinutes(6 * 60 + 59), 13),
            new Fee(TimeSpan.FromMinutes(7 * 60), TimeSpan.FromMinutes(7 * 60 + 59), 18),
            new Fee(TimeSpan.FromMinutes(8 * 60), TimeSpan.FromMinutes(8 * 60 + 29), 13),
            new Fee(TimeSpan.FromMinutes(8 * 60 + 30), TimeSpan.FromMinutes(14 * 60 + 59), 8),
            new Fee(TimeSpan.FromMinutes(15 * 60), TimeSpan.FromMinutes(15 * 60 + 29), 13),
            new Fee(TimeSpan.FromMinutes(15 * 60 + 30), TimeSpan.FromMinutes(16 * 60 + 59), 18),
            new Fee(TimeSpan.FromMinutes(17 * 60), TimeSpan.FromMinutes(17 * 60 + 59), 13),
            new Fee(TimeSpan.FromMinutes(18 * 60), TimeSpan.FromMinutes(18 * 60 + 29), 8),
        };

        public int GetTollFee(DateTime[] request)
        {
            var totalFee = 0;

            try
            {
                // group by date since the calculation should be done based on the date of the month
                var groupedByDateList = request.GroupBy(u => u.Date).Select(grp => grp.ToArray()).ToArray();

                foreach (var item in groupedByDateList)
                {
                    totalFee += GetTollFeeForTheDay(item);

                }

                return totalFee;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static int GetTollFeeForTheDay(DateTime[] request)
        {
            var finalPriceList = new List<int>();
            var validityPeriod = TimeSpan.FromMinutes(60) + request[0].TimeOfDay; // The variable is to identify if the each passed time is between one hour

            try
            {
                for (int i = 0; i < request.Length; i++)
                {
                    //Check if the toll free is free
                    var isTollFree = TollFreeService.IsTollFree(request[i]);
                    if (!isTollFree)
                    {
                        if (i == 0)
                        {
                            //Get the current fee
                            var currentFee = GetFeeForTheTime(request[i]);
                            finalPriceList.Add(currentFee);
                        }
                        else
                        {
                            //If the each pass is beteween the 60 mins get the highest value
                            if (request[i].TimeOfDay <= validityPeriod)
                            {
                                var newList = request.Where(x => x.TimeOfDay >= (validityPeriod - TimeSpan.FromMinutes(60)) && x.TimeOfDay <= validityPeriod).ToArray();
                                var highestVal = GetTheHighest(newList);

                                finalPriceList[finalPriceList.Count - 1] = 0;
                                finalPriceList.Add(highestVal);

                            }
                            else
                            {
                                var currentFee = GetFeeForTheTime(request[i]);
                                validityPeriod = TimeSpan.FromMinutes(60) + request[i].TimeOfDay;
                                finalPriceList.Add(currentFee);
                            }
                        }

                    }
                }
                return finalPriceList.Sum() > 60 ? 60 : finalPriceList.Sum();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static int GetFeeForTheTime(DateTime passages)
        {
            var cost = 0;
            try
            {
                if (Range.FirstOrDefault(x => passages.TimeOfDay >= x.FromMinute && passages.TimeOfDay <= x.ToMinute) != null)
                {
                    cost = Range.FirstOrDefault(x => passages.TimeOfDay >= x.FromMinute && passages.TimeOfDay <= x.ToMinute).Price;
                }
                return cost;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static int GetTheHighest(DateTime[] passages)
        {
            var priceList = new List<int>();
            foreach (var item in passages)
            {
                priceList.Add(GetFeeForTheTime(item));
            }
            return priceList.Max();
        }
    }
}
