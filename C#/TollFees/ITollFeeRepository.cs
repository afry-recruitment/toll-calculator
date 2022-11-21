using System;
using System.Collections.Generic;
using TollFeeCalculator.Vehicles;

namespace TollFeeCalculator.TollFees
{
    public interface ITollFeeRepository
    {
        IList<VehicleClassification> TollFreeVehicleClassifications { get; }

        IList<VehicleType> TollFreeVehicleTypes { get; }

        IList<TimeSpan>

    }
}
