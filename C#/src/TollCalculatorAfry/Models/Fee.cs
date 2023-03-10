using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollCalculatorAfry.Models
{
    public class Fee
    {
        public Fee(TimeSpan fromMinute, TimeSpan toMinute, int price)
        {
            FromMinute = fromMinute;
            ToMinute = toMinute;
            Price = price;
        }

        public int Id { get; set; }
        public TimeSpan FromMinute { get; set; }
        public TimeSpan ToMinute { get; set; }
        public int Price { get; set; }
    }
}
