using System;
using System.Collections.Generic;
using TollFeeCalculator.TollFees;
using TollFeeCalculator.Vehicles;

namespace TollFeeCalculator
{
    public class TollFeeRepositoryInit
    {
        public static ITollFeeRepository GetNewTollFeeRepository()
        {
            return new TollFeeRepository(
                GetTollFeeTimeIntervals(),
                GetTollFreeVehicleClassifications(),
                GetTollFreeVehicleTypes(),
                GetTollFreeDates(),
                GetTollFeeFreeDaysOfWeek(),
                GetMaxTollFeePerDay(),
                GetCombineTollFeeTimeSpan());
        }

        private static IList<VehicleClassification> GetTollFreeVehicleClassifications()
        {
            return new List<VehicleClassification>()
            {
                VehicleClassification.Diplomat,
                VehicleClassification.Emergency,
                VehicleClassification.Foreign,
                VehicleClassification.Military
            };
        }

        private static IList<VehicleType> GetTollFreeVehicleTypes()
        {
            return new List<VehicleType>()
            {
                VehicleType.Motorbike,
                VehicleType.Tractor,
            };
        }

        private static IList<TollFeeTimeInterval> GetTollFeeTimeIntervals()
        {
            return new List<TollFeeTimeInterval>()
            {
                new(new TimeOnly(6, 0), new TimeOnly(6, 30), 8),
                new(new TimeOnly(6, 30), new TimeOnly(7, 0), 13),
                new(new TimeOnly(7, 0), new TimeOnly(8, 0), 18),
                new(new TimeOnly(8, 0), new TimeOnly(8, 30), 13),
                new(new TimeOnly(8, 30), new TimeOnly(14, 59), 8),
                new(new TimeOnly(15, 0), new TimeOnly(15, 30), 13),
                new(new TimeOnly(15, 30), new TimeOnly(17, 0), 18),
                new(new TimeOnly(17, 0), new TimeOnly(18, 0), 13),
                new(new TimeOnly(18, 0), new TimeOnly(18, 30), 8),
            };
        }

        private static IList<DateOnly> GetTollFreeDates()
        {
            List<DateOnly> tollFreeDates = new()
            {
                new DateOnly(2013, 1, 1),
                new DateOnly(2013, 3, 28),
                new DateOnly(2013, 3, 29),
                new DateOnly(2013, 4, 1),
                new DateOnly(2013, 4, 30),
                new DateOnly(2013, 5, 1),
                new DateOnly(2013, 5, 8),
                new DateOnly(2013, 5, 9),
                new DateOnly(2013, 6, 5),
                new DateOnly(2013, 6, 6),
                new DateOnly(2013, 6, 21),
                new DateOnly(2013, 11, 1),
                new DateOnly(2013, 12, 24),
                new DateOnly(2013, 12, 25),
                new DateOnly(2013, 12, 26),
                new DateOnly(2013, 12, 31)
            };

            for (var day = 1; day <= 31; day++)
            {
                tollFreeDates.Add(new DateOnly(2013, 7, day));
            }

            return tollFreeDates;
        }

        private static IList<DayOfWeek> GetTollFeeFreeDaysOfWeek()
        {
            return new List<DayOfWeek>()
            {
                DayOfWeek.Saturday,
                DayOfWeek.Sunday
            };
        }

        private static int GetMaxTollFeePerDay()
        {
            return 60;
        }

        private static TimeSpan GetCombineTollFeeTimeSpan()
        {
            return new TimeSpan(1, 0, 0);
        }
    }
}
