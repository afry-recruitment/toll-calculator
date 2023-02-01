using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TollCalculator.Model;

namespace TollCalculator.Parsing
{
    public class CsvLineParser
    {
        public static VheicleDataItem Parse(Vehicle vehicle, string[] csvlines)
        {
            var vheicleDataItems = new VheicleDataItem(vehicle, new List<DateTime>());

            foreach (var csvLine in csvlines)
            {
                var vheicleDataItem = Parse(csvLine);

                vheicleDataItems.registerDate.Add(vheicleDataItem);
            }

            return vheicleDataItems;
        }

        private static DateTime Parse(string csvLine)
        {
            DateTime enteredDate;
            try
            {
                var lineItems = csvLine.Split(';');
                var date = DateTime.Parse(lineItems[1]);
                enteredDate = DateTime.Parse(lineItems[1]);

            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
                // Set IndexOutOfRangeException to the new exception's InnerException.
                throw new FormatException("FormatException", e);
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine(e.Message);
                // Set IndexOutOfRangeException to the new exception's InnerException.
                throw new ArgumentOutOfRangeException("Index parameter is out of range.", e);
            }
            return enteredDate;
        }
    }
}

