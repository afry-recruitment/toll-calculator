using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tollCalculator.Models;
using tollCalculator.Data;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TollFeeCalculator;

namespace tollCalculator.Services
{
	public class VehicleServices
	{
		private readonly ApplicationDbContext _dbContext;

		public VehicleServices(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		public string inputLicensePlate()
		{
			try
			{
				return Console.ReadLine();
			}
			catch 
			{
				return "";
			}
			
			
		}

		public Vehicle? getVehicle()
		{
			string currentVehicle = inputLicensePlate();

			if (string.IsNullOrEmpty(currentVehicle))
			{
				return null;
			}

			try
			{
				return _dbContext.Vehicles
				.FirstOrDefault(v => v.LicensePlate == currentVehicle);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Vehicle not found: {ex.Message}");
				return (null);
			}

		}

		public (bool success, Vehicle? vehicle) isVehicleRetrieved()
		{
			Vehicle? vehicle = getVehicle();

			if (vehicle == null)
			{
				return (false, vehicle);
			}
			else
			{
				return (true, vehicle);
			}
		}

		public bool isTaxExempt(Vehicle vehicle)
		{
			try
			{
				return _dbContext.Vehicles
			.Where(v => v.VehicleID == vehicle.VehicleID)
			.Select(v => v.VehicleType.TollExempt.IsExempt)
			.FirstOrDefault();

			}
			catch
			{
				//will return tax-free if faulty database
				return true;
			}
		}
	
		

		
	}
}
