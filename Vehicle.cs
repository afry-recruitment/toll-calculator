using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollFeeCalculator
{
    public class Vehicle
    {
        public enum VehicleType
        {
            Motorbike = 0,
            Tractor = 1,
            Car = 2,
            Truck = 3,
            Bus = 4
        }
        public VehicleType Type { get; set; }
        public enum VehicleClass
        {
            Civil = 0,
            Emergency = 1,
            Diplomat = 2,
            Foreign = 3,
            Military = 4
        }
        public VehicleClass Class { get; set; }

        public string RegistrationNumber { get; set; }
        
        public List<TollFee> TollFees { get; set; } = new List<TollFee>();

        public int GetTotalTollFee()
        {
            try
            {
                return TollFees.Count > 0 ? TollFees.Select(f => f.Fee).Sum() : 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 0;
            }
        }

        public int GetTotalTollFee(DateTime timeStamp)
        {
            try
            {
                return TollFees.Count > 0 ? TollFees.Where(t => t.TimeStamp.Date == timeStamp.Date).Select(f => f.Fee).Sum() : 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 0;
            }
        }
    }
}