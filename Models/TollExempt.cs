using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace tollCalculator.Models
{
	public class TollExempt
	{
		[Key]
		public int TollExemptID { get; set; }
		public bool IsExempt { get; set; } = false;
		public virtual ICollection<VehicleType> VehicleTypes { get; set; } = new List<VehicleType>();

	}
}
