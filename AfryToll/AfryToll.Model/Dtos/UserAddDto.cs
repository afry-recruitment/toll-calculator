using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfryToll.Model.Dtos
{

        public class UserAddDto
        {
            [Required(ErrorMessage = "License plate is required.")]
            public string PlateNumber { get; set; }

            [Required(ErrorMessage = "Password plate is required.")]
            public string Password { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        }
    
}