using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TollCalculator
{
    public static class TimeToTollFeePopulator
    {
        /// <summary>
        /// This initializes the toll fee,
        /// Accepts a CSV string to parse if there are new tolls to use, else uses the default
        /// Format will be StartTime(In HH:MM:SS format),EndTime(In HH:MM:SS Format),TollFee(In SEK)
        /// </summary>
        /// <param name="tollFeeCSV"></param>
        /// <returns></returns>
        public static List<TimeToTollFee> InitializeTollFees(List<string> tollFeeCSV = null)
        {
            if (tollFeeCSV != null && tollFeeCSV.Count > 0)
            {
                var timeToTollFeeList = new List<TimeToTollFee>();
                foreach (var tollFeeString in tollFeeCSV)
                {
                    var tollFeeList = tollFeeString.Split(",").ToList();
                    if (tollFeeList.Count == 3)
                    {
                        var startTime = tollFeeList[0].Split(":").Select(x => Convert.ToInt32(x)).ToList();
                        var endTime = tollFeeList[1].Split(":").Select(x => Convert.ToInt32(x)).ToList();
                        var tollFee = tollFeeList[2];

                        timeToTollFeeList.Add(new TimeToTollFee
                        {
                            TollFee = Convert.ToInt32(tollFee),
                            EndTime = new TimeSpan(endTime[0], endTime[1], endTime[2]),
                            StartTime = new TimeSpan(startTime[0], startTime[1], startTime[2])
                        });
                    }
                }

                return timeToTollFeeList;
            }
            else
            {
                return GenerateDefaultTollFees();
            }
        }

        private static List<TimeToTollFee> GenerateDefaultTollFees()
        {
            return new List<TimeToTollFee>()
            {
                new TimeToTollFee
                {
                    StartTime = new TimeSpan(6, 0, 0),
                    EndTime = new TimeSpan(6, 29, 59),
                    TollFee = 8
                },
                new TimeToTollFee
                {
                    StartTime = new TimeSpan(6, 30, 0),
                    EndTime = new TimeSpan(6, 59, 59),
                    TollFee = 13
                },
                new TimeToTollFee
                {
                    StartTime = new TimeSpan(7, 0, 0),
                    EndTime = new TimeSpan(7, 59, 59),
                    TollFee = 18
                },
                new TimeToTollFee
                {
                    StartTime = new TimeSpan(8, 0, 0),
                    EndTime = new TimeSpan(8, 29, 59),
                    TollFee = 13
                },
                new TimeToTollFee
                {
                    StartTime = new TimeSpan(8, 30, 0),
                    EndTime = new TimeSpan(14, 59, 59),
                    TollFee = 8
                },
                new TimeToTollFee
                {
                    StartTime = new TimeSpan(15, 0, 0),
                    EndTime = new TimeSpan(15, 29, 59),
                    TollFee = 13
                },
                new TimeToTollFee
                {
                    StartTime = new TimeSpan(15, 30, 0),
                    EndTime = new TimeSpan(16, 59, 59),
                    TollFee = 18
                },
                new TimeToTollFee
                {
                    StartTime = new TimeSpan(17, 0, 0),
                    EndTime = new TimeSpan(17, 59, 59),
                    TollFee = 13
                },
                new TimeToTollFee
                {
                    StartTime = new TimeSpan(18, 0, 0),
                    EndTime = new TimeSpan(17, 29, 59),
                    TollFee = 8
                },
            };
        }
    }
}
