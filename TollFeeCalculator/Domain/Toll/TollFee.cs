using System;
using System.Collections.Generic;
using System.Linq;

namespace TollFeeCalculator.Domain
{
    public enum FeeLevel
    {
        Low,
        Medium,
        High
    }

    public class TollFeeInterval
    {
        public TimeSpan Start { get; set; }
        public TimeSpan End   { get; set; }
        public FeeLevel FeeLevel { get; set; }
    }

    public static class TollFeePolicy
    {
        private const int LowFee = 8;
        private const int MediumFee = 13;
        private const int HighFee = 18;

        private static readonly List<TollFeeInterval> Intervals = new List<TollFeeInterval>
        {
            new TollFeeInterval { Start = TimeSpan.FromHours(6), End = TimeSpan.FromHours(6).Add(TimeSpan.FromMinutes(30)), FeeLevel = FeeLevel.Low },
            new TollFeeInterval { Start = TimeSpan.FromHours(6).Add(TimeSpan.FromMinutes(30)), End = TimeSpan.FromHours(7), FeeLevel = FeeLevel.Medium },
            new TollFeeInterval { Start = TimeSpan.FromHours(7), End = TimeSpan.FromHours(8), FeeLevel = FeeLevel.High },
            new TollFeeInterval { Start = TimeSpan.FromHours(8), End = TimeSpan.FromHours(8).Add(TimeSpan.FromMinutes(30)), FeeLevel = FeeLevel.Medium },
            new TollFeeInterval { Start = TimeSpan.FromHours(8).Add(TimeSpan.FromMinutes(30)), End = TimeSpan.FromHours(15), FeeLevel = FeeLevel.Low },
            new TollFeeInterval { Start = TimeSpan.FromHours(15), End = TimeSpan.FromHours(15).Add(TimeSpan.FromMinutes(30)), FeeLevel = FeeLevel.Medium },
            new TollFeeInterval { Start = TimeSpan.FromHours(15).Add(TimeSpan.FromMinutes(30)), End = TimeSpan.FromHours(17), FeeLevel = FeeLevel.High },
            new TollFeeInterval { Start = TimeSpan.FromHours(17), End = TimeSpan.FromHours(18), FeeLevel = FeeLevel.Medium },
            new TollFeeInterval { Start = TimeSpan.FromHours(18), End = TimeSpan.FromHours(18).Add(TimeSpan.FromMinutes(30)), FeeLevel = FeeLevel.Low }
        };

        public static int GetFee(DateTime date)
        {
            TimeSpan timeOfDay = date.TimeOfDay;
            var interval = Intervals.FirstOrDefault(i => timeOfDay >= i.Start && timeOfDay < i.End);
            if (interval == null)
            {
                return 0;
            }

            return interval.FeeLevel switch
            {
                FeeLevel.Low    => LowFee,
                FeeLevel.Medium => MediumFee,
                FeeLevel.High   => HighFee,
                _ => 0
            };
        }
    }
}