using System;
using System.Collections.Generic;
using TollFeeCalculator.TollFees;
using TollFeeCalculator.Vehicles;

namespace TollFeeCalculator
{

    public class TollCalculator
    {
        private readonly ITollFeeRepository _tollFeeRepository;

        public TollCalculator(ITollFeeRepository tollFeeRepository)
        {
            _tollFeeRepository = tollFeeRepository;
        }

        /**
     * Calculate the total toll fee for one day
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */
        public int GetTollFee(IVehicle vehicle, IList<DateTime> dates)
        {
            return TollFee.GetTollFeeForVehicle(vehicle, dates, _tollFeeRepository);
        }
    }
}