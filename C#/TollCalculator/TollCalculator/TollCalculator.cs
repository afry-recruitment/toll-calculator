using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TollCalculator.Interface;
using TollCalculator.Repos;


namespace TollCalculator
{
    public class TollCalculator
    {
        public static int GetTollFee(IVehicle vehicle, DateTime[] passingDates)
        {
            if (vehicle.IsTollFree())
                return 0;

            Dictionary<DateTime, int> paymentPassings = new();
            
            DateTime[] sortedPassingDates = passingDates.OrderBy(x => x.Date).ToArray();


            foreach (var passing in sortedPassingDates)
            {
                // next item if no toll
                if (!PassingDate.IsDateTollDate(passing))
                    continue;

                int tollCost = PaymentInterval.GetCost(passing);

                if (paymentPassings.Count == 0)
                {
                    paymentPassings.Add(passing, tollCost);
                    continue;
                }

                if ((passing - paymentPassings.Last().Key).TotalMinutes <= 60)
                {
                    if (paymentPassings.Last().Value < tollCost)
                        paymentPassings[paymentPassings.Last().Key] = tollCost;
                }
                else
                    paymentPassings.Add(passing, tollCost);
            }
            return SumUpMax60PerDay(paymentPassings);
        }


        private static int SumUpMax60PerDay(Dictionary<DateTime, int> paymentPassings)
        {
            int totalCost = 0;
            int dayCostMax60 = 0;
            DateTime currentDay = DateTime.Now;

            foreach (var passing in paymentPassings)
            {
                if (currentDay.Date != passing.Key.Date)
                {
                    totalCost += dayCostMax60;
                    currentDay = passing.Key;
                    dayCostMax60 = 0;
                }

                dayCostMax60 += passing.Value;
                if (dayCostMax60 > 60)
                    dayCostMax60 = 60;
            }
            totalCost += dayCostMax60;
            return totalCost;
        }
    }
}