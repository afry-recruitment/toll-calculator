using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfryToll.Model.Dtos
{
    public class TollToAddDto
    {
        public int Id { get; set; }
        public int UserCost { get; set; }
        public int UserId { get; set; }

        public string Date { get; set; }
        public string Time { get; set; }
        public int Temp { get; set; }
        public int TempCost { get; set; }

        public int TollCostId { get; set; }
    }
}
