using System;
using System.ComponentModel.DataAnnotations;

namespace tollCalculator.Models
{ 
	public class ActiveFeeData
	{
		[Key]
		public int ActiveFeeDataID { get; set; }

		public DateTime? StartCheckIn { get; set; }

		public DateTime? CurrentCheckIn { get; set; }

		public decimal? TotalFee { get; set; }

		public virtual Vehicle? Vehicle { get; set; }

	}
}
