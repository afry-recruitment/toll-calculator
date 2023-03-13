using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TollCalculator.Data;

namespace TollCalculator.Repos
{
     public class PaymentInterval
    {
        public static int GetCost(DateTime passingTime)
		{
            PaymentInterval ?pi = PaymentIntervals.PIList.FirstOrDefault(x => x.IsInIntervall(passingTime));
            if (pi == null)
                return 0;
            return pi.Cost;
		}

        public PaymentInterval(int _cost, DateTime _startIntervall, DateTime _stopInterval)
        {
            Cost = _cost;
            StartTime = _startIntervall;
            StopTime = _stopInterval;
        }

        public int Cost { get; }
        private readonly DateTime StartTime;
        private readonly DateTime StopTime;

        public bool IsInIntervall(DateTime passingTime)
        {
            // Creates two datetimes with same date as passingdate to compare

            DateTime startdateToCompare = new (passingTime.Year, passingTime.Month, passingTime.Day);
            startdateToCompare = startdateToCompare.AddHours(StartTime.Hour);
            startdateToCompare = startdateToCompare.AddMinutes(StartTime.Minute);

            DateTime stopdateToCompare = new (passingTime.Year, passingTime.Month, passingTime.Day);
            stopdateToCompare = stopdateToCompare.AddHours(StopTime.Hour);
            stopdateToCompare = stopdateToCompare.AddMinutes(StopTime.Minute);

            if (passingTime.CompareTo(startdateToCompare) < 0)
                return false;
            if (passingTime.CompareTo(stopdateToCompare) > 0)
                return false;

            return true;
        }
    }
}
