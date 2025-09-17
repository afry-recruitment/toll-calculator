using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tollCalculator.Models;
using tollCalculator.Data;
using TollFeeCalculator;

namespace tollCalculator.Services
{
	public class ActiveFeeDataServices
	{
		private readonly ApplicationDbContext _dbContext;

		public ActiveFeeDataServices(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public bool VehicleHasOpenRecord(Vehicle vehicle)
		{
			return vehicle.ActiveFeeDataID != null;
		}

		public void CreateNewRecord(Vehicle vehicle, bool hasOpenRecord)
		{
			if (!hasOpenRecord)
			{
				var newFeeData = new ActiveFeeData
				{
					StartCheckIn = DateTime.Now,
					CurrentCheckIn = DateTime.Now,
					TotalFee = 0m,
				};

				// Lägg till i DbSet
				_dbContext.ActiveFeeDatas.Add(newFeeData);

				// Spara ändringarna i databasen
				_dbContext.SaveChanges();

				int generatedId = newFeeData.ActiveFeeDataID;

				LinkRecordVehicle(vehicle, newFeeData.ActiveFeeDataID);
			}
		}
		public void LinkRecordVehicle(Vehicle vehicle, int id)
		{
			var myVehicle = _dbContext.Vehicles.FirstOrDefault(v => v.VehicleID == vehicle.VehicleID);

			vehicle.ActiveFeeDataID = id;

			// Spara ändringarna
			_dbContext.SaveChanges();

		}

		public bool IsWithinSameDay(Vehicle vehicle, DateTime now)
		{
			var openRecord = _dbContext.ActiveFeeDatas.FirstOrDefault(a => a.ActiveFeeDataID == vehicle.ActiveFeeDataID);

			if ((openRecord.StartCheckIn.Value - now).TotalMinutes <= 60)
			{
				return true;
			}
			else
			{
				return false;
			}
				

		}
	}

}
