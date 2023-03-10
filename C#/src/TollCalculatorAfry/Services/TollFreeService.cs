using Nager.Date;
using Nager.Date.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TollCalculatorAfry.Models;

namespace TollCalculatorAfry.Services
{
    public class TollFreeService
    {
        public static bool IsTollFree(DateTime passage)
        {
            try
            {
                var isHoliday = IsTollFreeDate(passage);

                if (passage.DayOfWeek != DayOfWeek.Saturday && passage.DayOfWeek != DayOfWeek.Sunday && !isHoliday && passage.Month != 7)
                    return false;

                return true;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static Boolean IsTollFreeDate(DateTime dateFree)
        {

            try
            {
                var xml = XDocument.Load(@"FreeDates.xml");
                var query = from c in xml.Root.Descendants("date")
                            where (int)c.Element("year") == dateFree.Date.Year & (int)c.Element("month") == dateFree.Date.Month & (int)c.Element("day") == dateFree.Date.Day
                            select c.Element("year").Value + " " +
                                   c.Element("month").Value + " " +
                                   c.Element("day").Value;

                return query != null ? query.ToList().Count > 0 : false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static bool IsTollFreeVehicle(IVehicle vehicle)
        {
            if (vehicle == null) return false;
            String vehicleType = vehicle.GetVehicleType();
            return vehicleType.Equals(VehicleType.Motorbike.ToString()) ||
                   vehicleType.Equals(VehicleType.Tractor.ToString()) ||
                   vehicleType.Equals(VehicleType.Emergency.ToString()) ||
                   vehicleType.Equals(VehicleType.Diplomat.ToString()) ||
                   vehicleType.Equals(VehicleType.Foreign.ToString()) ||
                   vehicleType.Equals(VehicleType.Military.ToString());
        }
    }
}
