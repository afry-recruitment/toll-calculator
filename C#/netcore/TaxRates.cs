using congestion_tax_api.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace congestion.calculator
{
    internal class TaxRates
    {
        static TaxRates()
        {
            var path = Path.Combine(Environment.CurrentDirectory,"Data\\", "RateCard.json");
            using var streamReader = new StreamReader(path);
            var json = streamReader.ReadToEnd();
            lstTaxRates = JsonConvert.DeserializeObject<TaxRateCard[]>(json);
        }

        public static TaxRateCard[] lstTaxRates { get; }
    }
}
