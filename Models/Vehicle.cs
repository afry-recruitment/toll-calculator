using System;
using System.ComponentModel.DataAnnotations;

namespace tollCalculator.Models
{

	public class Vehicle
	{
			[Key]
			public int VehicleID { get; set; }

			[Required]
			public string LicensePlate { get; set; }

			public int VehicleTypeID { get; set; }

			public virtual VehicleType? VehicleType { get; set; }

			public int? ActiveFeeDataID { get; set; }
			public virtual ActiveFeeData? ActiveFeeData { get; set; }

			public virtual ICollection<FeeDataReceipt> FeeDataReceipts { get; set; }
			= new List<FeeDataReceipt>();
	}
}
