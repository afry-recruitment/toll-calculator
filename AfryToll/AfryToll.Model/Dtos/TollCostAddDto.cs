using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfryToll.Model.Dtos
{
    public class TollCostAddDto
    {
        public int Id { get; set; }
        public int Cost { get; set; }

        public string Time { get; set; } = "08:00";
        public int Traffic { get; set; }
        public int CategoryId { get; set; }
    }
}
