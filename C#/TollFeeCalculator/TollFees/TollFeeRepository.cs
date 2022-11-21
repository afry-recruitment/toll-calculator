using System;
using System.Collections.Generic;
using TollFeeCalculator.Vehicles;

namespace TollFeeCalculator.TollFees
{
    public class TollFeeRepository : ITollFeeRepository
    {
        public IList<TollFeeTimeInterval> TollFeesForTimeIntervals { get; }

        public IList<VehicleClassification> TollFeeFreeVehicleClassifications { get; }

        public IList<VehicleType> TollFeeFreeVehicleTypes { get; }

        public IList<DateOnly> TollFeeFreeDates { get; }

        public IList<DayOfWeek> TollFeeFreeDaysOfWeek { get; }

        public int MaxTollFeePerDay { get; set; }

        public TimeSpan CombineTollFeeTimeSpan { get; set; }

        public TollFeeRepository()
        {
            TollFeesForTimeIntervals = new List<TollFeeTimeInterval>();
            TollFeeFreeVehicleClassifications = new List<VehicleClassification>();
            TollFeeFreeVehicleTypes = new List<VehicleType>();
            TollFeeFreeDates = new List<DateOnly>();
            TollFeeFreeDaysOfWeek = new List<DayOfWeek>();
            MaxTollFeePerDay = 60;
            CombineTollFeeTimeSpan = new TimeSpan(1, 0, 0);
        }

        public TollFeeRepository(
            IList<TollFeeTimeInterval> tollFeesForTimeIntervals,
            IList<VehicleClassification> tollFeeFreeVehicleClassifications,
            IList<VehicleType> tollFeeFreeVehicleTypes,
            IList<DateOnly> tollFeeFreeDates,
            IList<DayOfWeek> tollFeeFreeDaysOfWeek,
            int maxTollFeePerDay,
            TimeSpan combineTollFeeTimeSpan
            )
        {
            TollFeesForTimeIntervals = tollFeesForTimeIntervals;
            TollFeeFreeVehicleClassifications = tollFeeFreeVehicleClassifications;
            TollFeeFreeVehicleTypes = tollFeeFreeVehicleTypes;
            TollFeeFreeDates = tollFeeFreeDates;
            TollFeeFreeDaysOfWeek = tollFeeFreeDaysOfWeek;
            MaxTollFeePerDay = maxTollFeePerDay;
            CombineTollFeeTimeSpan = combineTollFeeTimeSpan;
        }
    }
}
