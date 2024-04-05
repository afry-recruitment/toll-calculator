using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TollFeeCalculator.Service
{
    public class TollFeeRateService
    {
        public List<TollFeeRate> LoadTollFeeRates()
        {
            try
            {
                var json = File.ReadAllText("tollFeeRates.json");
                var rates = JsonSerializer.Deserialize<List<TollFeeRate>>(json);
                return rates;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error deserializing JSON: {ex.Message}");
                return new List<TollFeeRate>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return new List<TollFeeRate>(); // Returns an empty list 
            }
        }
    }
}
