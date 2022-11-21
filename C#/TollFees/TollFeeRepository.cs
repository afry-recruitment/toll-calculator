using System.Collections.Generic;
using TollFeeCalculator.Vehicles;

namespace TollFeeCalculator.TollFees
{
    internal class TollFeeRepository : ITollFeeRepository
    {
        public IEnumerable<VehicleClassification> TollFreeVehicleClassifications { get; }

        public IEnumerable<VehicleType> TollFreeVehicleTypes { get; }

        public TollFeeRepository()
        {
            TollFreeVehicleClassifications = new List<VehicleClassification>()
            {
                VehicleClassification.Diplomat,
                VehicleClassification.Emergency,
                VehicleClassification.Foreign,
                VehicleClassification.Military
            };
            TollFreeVehicleTypes = new List<VehicleType>()
            {
                VehicleType.Motorbike,
                VehicleType.Tractor,
            };
        }

        public TollFeeRepository(
            IEnumerable<VehicleClassification> tollFreeVehicleClassifications,
            IEnumerable<VehicleType> tollFreeVehicleTypes)
        {
            TollFreeVehicleClassifications = tollFreeVehicleClassifications;
            TollFreeVehicleTypes = tollFreeVehicleTypes;
        }

    }
}
