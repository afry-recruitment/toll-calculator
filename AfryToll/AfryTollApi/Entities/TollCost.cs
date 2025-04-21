using System.ComponentModel.DataAnnotations.Schema;

namespace AfryTollApi.Entities
{
    public class TollCost
    {
        public int Id { get; set; }
        public int Cost { get; set; }
        public string Time { get; set; }
        public int Traffic { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
    }
}
