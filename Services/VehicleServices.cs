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
		public string InputLicensePlate()
		{
			try
			{
				string input = Console.ReadLine()?.Trim().ToUpper() ?? "";

				// Allows for Swedish license plates, both regular and personalized
				//https://rkrattsdb.gov.se/SFSdoc/17/170098.PDF?utm_source
				//https://www.transportstyrelsen.se/sv/vagtrafik/fordon/aga-kopa-eller-salja-fordon/registreringsskyltar/personlig-skylt
				if (System.Text.RegularExpressions.Regex.IsMatch(input, @"^[A-ZÅÄÖ0-9 ]{1,7}$"))
				{
					return input;
				}

				Console.WriteLine("Invalid licence plate");
				return "";
			}
			catch (Exception ex) 
			{
				Console.WriteLine($"Error reading input value: {ex.Message}");
				return "";
			}
		}

		public Vehicle? GetVehicle()
		{
			string currentVehicle = InputLicensePlate();

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
				Console.WriteLine($"Vehicle not found: {ex.Message}");
				return (null);
			}

		}

		/// <summary>
		/// Whether the vehicle was successfully retreived from the database or not
		/// </summary>
		/// <returns>Bool and vehicle object</returns>
		public (bool success, Vehicle? vehicle) IsVehicleRetrieved()
		{
			Vehicle? vehicle = GetVehicle();

			if (vehicle == null)
			{
				return (false, vehicle);
			}
			else
			{
				return (true, vehicle);
			}
		}

		public bool IsTaxExempt(Vehicle vehicle)
		{
			try
			{
				var isTaxExempt = _dbContext.Vehicles
					.Include(v => v.VehicleType)
					.ThenInclude(t => t.TollExempt)
					.FirstOrDefault(v => v.VehicleID == vehicle.VehicleID);

				return isTaxExempt?.VehicleType?.TollExempt?.IsExempt ?? false;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error retrieving tax exemption: {ex.Message}");
				return false;
			}
		}
	}
}
