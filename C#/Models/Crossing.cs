using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TollFeeCalculator.Models;

namespace TollFeeCalculator.Models
{
    public class Crossing
    {
        public int Id { get; set; }
        public Vehicle Vehicle { get; set; }
        public ICollection<DateTime> HourlyCrossings { get; set; }
        public DateTime CrossingDate { get; set; }
        public int TotalFeeDay { get; set; }

        public Crossing()
        {
            CrossingDates = new List<DateTime>();
            TotalFeeDay = new Date();
        }
    }
}
