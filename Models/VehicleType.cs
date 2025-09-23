using System;
using System.ComponentModel.DataAnnotations;

namespace tollCalculator.Models
{
	public class VehicleType
	{
		[Key]
		public int VehicleTypeID { get; set; }
		[Required]
		public VehicleTypeEnum Type { get; set; }
		public int TollExemptID { get; set; }
		public virtual TollExempt? TollExempt{ get; set; }
		public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
	}

	public enum VehicleTypeEnum
	{
		//https://www.transportstyrelsen.se/sv/vagtrafik/fordon/skatter-och-avgifter/trangselskatt/undantag-fran-trangselskatt/

		[Display(Name = "Tullpliktig")]
		Tullpliktig = 0,

		[Display(Name = "Utryckningsfordon")]
		Utryckningsfordon = 1,

		[Display(Name = "Buss")]
		Buss = 2,

		[Display(Name = "Diplomat")]
		Diplomat = 3,

		[Display(Name = "Motorcykel")]
		Motorcykel = 4,

		[Display(Name = "Militär")]
		Militar = 5,

		[Display(Name = "Rörelsehinder")]
		Rorelsehinder = 6
	}
}

