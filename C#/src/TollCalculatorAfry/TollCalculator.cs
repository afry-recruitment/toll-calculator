using System;
using System.Linq;
using TollCalculatorAfry.Models;
using TollCalculatorAfry.Services;

namespace TollCalculatorAfry
{
    public class TollCalculator
    {
        /**
 * Calculate the total toll fee for one day
 *
 * @param vehicle - the vehicle
 * @param dates   - date and time of all passes on one day
 * @return - the total toll fee for that day
 */

        public int GetTollFee(IVehicle vehicle, DateTime[] dates)
        {
            try
            {
                var totalFee = 0;

                if (TollFreeService.IsTollFreeVehicle(vehicle)) return 0;

                if (dates != null)
                {
                    // order the request list by date and time
                    var newrequest = dates.OrderBy(x => x.Date).ThenBy(x => x.Hour).ThenBy(x => x.Minute).ToArray();

                    totalFee = new TollService().GetTollFee(newrequest);
                }
                else
                {
                    totalFee = 0;
                }

                return totalFee;
            }
            catch (Exception)
            {
                throw;
            }
        }

       
       
    }
}
