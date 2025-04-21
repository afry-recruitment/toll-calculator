using System.ComponentModel.DataAnnotations.Schema;

namespace AFRY_Toll.Entities
{
    public class Toll
    {
        public int Id { get; set; }
        public int UserCost { get; set; }
        public int UserId { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public int Temp { get; set; }
        public int TempCost { get; set; }
        public int TollCostId { get; set; }
        [ForeignKey("TollCostId")]
        public TollCost TollCost { get; set; }
    }
}
