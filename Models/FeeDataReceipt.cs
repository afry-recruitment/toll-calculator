using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace tollCalculator.Models
{
	public class FeeDataReceipt
	{
		[Key]
		public int FeeDataReceiptID { get; set; }
		public DateTime FeeCheckInTime { get; set; }
		public decimal FeeAmount { get; set; }
		public int VehicleID { get; set; }
		public virtual Vehicle Vehicle { get; set; }
	}
}
