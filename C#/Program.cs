using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollFeeCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
           
            var vehical = new VehicleType("Car");
            TollCalculator tc = new TollCalculator();
            var staticDatetimes = new DateTime[] 
            {
                  new DateTime (2022,6,14,6,31,0),
                  new DateTime(2022,6,14,7,10,0),
                  new DateTime(2022,6,14,14,29,0)
            };

            int totalFee = tc.GetTotalTollFee(vehical, staticDatetimes.ToArray());

          
            /*
            DateTime dateCheck;
            List<DateTime> userDatetimes = new List<DateTime>();
            string[] userDatetime = { "2022-6-14 7:35:0", "2022-6-14 13:35:0", "2022-14-14 19:31:0" };
            foreach (string dateString in userDatetime)
            {
                if (DateTime.TryParse(dateString, out dateCheck))
                {
                    userDatetimes.Add(dateCheck);
                }
                else
                {
                    Console.WriteLine("  Unable to parse '{0}'.", dateString);
                }
            }

            int totalFee = tc.GetTotalTollFee(vehical, userDatetimes.ToArray());
            */

            Console.WriteLine("Total Toll Fee :" + totalFee);
            Console.ReadLine();
           
        }
    }
}
