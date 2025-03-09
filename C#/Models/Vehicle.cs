using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TollFeeCalculator.interfaces;

namespace TollFeeCalculator.Models
{
	// TODO: Expand with a foreign key of ownerID(Owner of vehicle)
	public abstract class Vehicle : IVehicle
	{
		public int Id { get; set; }
		//OwnerId.
		public string RegistrationNumber { get; set; }

		public abstract String GetVehicleType();
	}
}