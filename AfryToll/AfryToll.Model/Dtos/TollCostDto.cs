using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfryToll.Model.Dtos
{
    public class TollCostDto
    {
        public int Id { get; set; }
        public int Cost { get; set; }
        public int Traffic { get; set; }
        public string Time { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
