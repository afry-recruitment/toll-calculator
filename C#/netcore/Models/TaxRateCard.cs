using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace congestion_tax_api.Models
{
    public class TaxRateCard
    {
        public String City { get; set; }
        public int startHour { get; set; }
        public int startMinute { get; set; }
        public int endHour { get; set; }
        public int endMinute { get; set; }
        public int amount { get; set; }
    }
}
