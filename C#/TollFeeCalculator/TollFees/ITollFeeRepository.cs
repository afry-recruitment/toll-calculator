using System;
using System.Collections.Generic;
using TollFeeCalculator.Vehicles;

namespace TollFeeCalculator.TollFees
{
    public interface ITollFeeRepository
    {
        IList<VehicleClassification> TollFeeFreeVehicleClassifications { get; }

        IList<VehicleType> TollFeeFreeVehicleTypes { get; }

        IList<TollFeeTimeInterval> TollFeesForTimeIntervals { get; }

        IList<DateOnly> TollFeeFreeDates { get; }

        IList<DayOfWeek> TollFeeFreeDaysOfWeek { get; }
        
        int MaxTollFeePerDay { get; set; }

        TimeSpan CombineTollFeeTimeSpan {get ; set; }
    }
}
