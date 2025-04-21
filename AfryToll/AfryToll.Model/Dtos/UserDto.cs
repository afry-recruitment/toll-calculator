using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfryToll.Model.Dtos
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string PlateNumber { get; set; }
        public string Password { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
