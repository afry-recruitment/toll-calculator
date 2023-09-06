using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficTollCalculator.Interfaces;
using TrafficTollCalculator.Models;

namespace TrafficTollCalculator.Services
{
    public class TollFeesService : ITollFeesInterface
    {
        public int GetTollFee(List<List<DateTime>> timestampsPerVehicle, List<Vehicle> vehicles)
        {
            var totalFee = 0;
            DateTime FeesForCurrentDate = new DateTime(2023, 9, 4);
            for (int i = 0; i < Math.Min(vehicles.Count, timestampsPerVehicle.Count); i++)
            {

                var timestamps = timestampsPerVehicle[i];
                var vehicleType = vehicles[i].VehicleType;
                var lastTimestamp = DateTime.MinValue;
                var accumulatedFee = 0;

                foreach (var timestamp in timestamps)
                {
                    if (IsTollFreeDate(timestamp) || IsTollFreeVehicle(vehicleType!) || IsFeesSubjectToAnotherCurrentDate(FeesForCurrentDate, timestamp))
                        continue;

                    var applicableFee = GetApplicableFee(timestamp.TimeOfDay);

                    if (lastTimestamp == DateTime.MinValue)
                    {
                        lastTimestamp = timestamp;
                        accumulatedFee = applicableFee;
                    }
                    else
                    {
                        var timeDifference = timestamp - lastTimestamp;
                        if (timeDifference.TotalMinutes <= 60)
                        {
                            accumulatedFee = Math.Max(accumulatedFee, applicableFee);
                        }
                        else
                        {
                            totalFee += accumulatedFee;
                            lastTimestamp = timestamp;
                            accumulatedFee = applicableFee;
                        }
                    }
                }

                totalFee += accumulatedFee;

            }
            return Math.Min(totalFee, 60);
        }

        public bool IsFeesSubjectToAnotherCurrentDate(DateTime currentDate, DateTime dateStamp)
        {
            return currentDate.Date != dateStamp.Date;
        }

        public bool IsTollFreeDate(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday ||
                   date.DayOfWeek == DayOfWeek.Sunday ||
                   tollFreeDates.Contains(date.Date);
        }

        public int GetApplicableFee(TimeSpan timeOfDay)
        {
            if (tollFees.TryGetValue(timeOfDay, out int fee))
            {
                return fee;
            }
            else
            {
                var closestTime = tollFees.Keys.LastOrDefault(k => k < timeOfDay);
                if (closestTime != default(TimeSpan))
                {
                    return tollFees[closestTime];
                }
                else
                {
                    return 0; 
                }
            }
        }

        public bool IsTollFreeVehicle(string vehicleType)
        {
            return tollFreeVehicleTypes.Contains(vehicleType);
        }

        private readonly HashSet<string> tollFreeVehicleTypes = new HashSet<string>
        {
            "Motorbike", "Tractor", "Emergency", "Diplomat", "Foreign", "Military"
        };

        private readonly Dictionary<TimeSpan, int> tollFees = new Dictionary<TimeSpan, int>
        {
            { new TimeSpan(6, 0, 0), 8 },
            { new TimeSpan(6, 30, 0), 13 },
            { new TimeSpan(7, 0, 0), 18 },
            { new TimeSpan(8, 0, 0), 13 },
            { new TimeSpan(8, 30, 0), 8 },
            { new TimeSpan(15, 0, 0), 18 },
            { new TimeSpan(17, 0, 0), 13 },
            { new TimeSpan(18, 0, 0), 8 },
            { new TimeSpan(18, 30, 0), 0 },
        };

        private readonly HashSet<DateTime> tollFreeDates = new HashSet<DateTime>
            {
               new DateTime(2023, 1, 1),
               new DateTime(2023, 3, 28),
               new DateTime(2023, 3, 29),
               new DateTime(2023, 4, 1),
               new DateTime(2023, 4, 30),
               new DateTime(2023, 5, 1),
               new DateTime(2023, 5, 8),
               new DateTime(2023, 5, 9),
               new DateTime(2023, 6, 5),
               new DateTime(2023, 6, 6),
               new DateTime(2023, 6, 21),
               new DateTime(2023, 11, 1),
               new DateTime(2023, 12, 24),
               new DateTime(2023, 12, 25),
               new DateTime(2023, 12, 31)

            };
    }
}


