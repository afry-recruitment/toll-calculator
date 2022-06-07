using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TollCalculator;

namespace TollCalculator
{
    public class TollFeeCalculator
    {
        /**
         * Calculate the total toll fee for one day
         *
         * @param vehicle - the vehicle
         * @param dates   - date and time of all passes on one day
         * @return - the total toll fee for that day
         */
        public int GetTollFee(Vehicle vehicle, DateTime[] dates)
        {
            try { 
                var timeToTollFees = TimeToTollFeePopulator.InitializeTollFees();

                var factory = new VehicleTollFeeCalculatorFactory();
                var vehicleTollFeeCalculator = factory.CreateVehicleTollFeeCalculatorFactory(vehicle);

                var tollFee = vehicleTollFeeCalculator.GetTollFee(dates, timeToTollFees);
                return tollFee;
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("Error encountered. Contact the admininstrator");
            }
        }
    }

}