using System.ComponentModel.DataAnnotations.Schema;

namespace AfryTollApi.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string PlateNumber { get; set; }
        public string Password { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
    }
}
