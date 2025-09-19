using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tollCalculator.Models;
using tollCalculator.Data;
using TollFeeCalculator;
using tollCalculator.Services;

namespace tollCalculator.Services
{
	public class ActiveFeeDataServices
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly TimeServices _timeServices;

		public ActiveFeeDataServices(ApplicationDbContext dbContext, TimeServices timeServices)
		{
			_dbContext = dbContext;
			_timeServices = timeServices;

		}

		public bool VehicleHasOpenRecord(Vehicle vehicle)
		{
			return vehicle.ActiveFeeDataID != null;
		}

		public bool? CreateNewRecord(Vehicle vehicle, bool hasOpenRecord)
		{
			if (hasOpenRecord) //if there is already an existing record connected to vechicle
			{
				return null;
			}
			var newFeeData = new ActiveFeeData
			{
				StartCheckIn = DateTime.Now,
				CurrentCheckIn = DateTime.Now,
				TotalFee = 0m,
			};

			try
			{
				_dbContext.ActiveFeeDatas.Add(newFeeData);

				_dbContext.SaveChanges();

				int generatedId = newFeeData.ActiveFeeDataID;

				LinkRecordVehicle(vehicle, newFeeData.ActiveFeeDataID);

				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return false;
			}
		}

		public void LinkRecordVehicle(Vehicle vehicle, int id)
		{

			try
			{
				var myVehicle = _dbContext.Vehicles.FirstOrDefault(v => v.VehicleID == vehicle.VehicleID);

				vehicle.ActiveFeeDataID = id;

				// Spara ändringarna
				_dbContext.SaveChanges();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}



		}

		public bool IsWithinSameDay(Vehicle vehicle, DateTime now)
		{
			//var openRecord = _dbContext.ActiveFeeDatas.FirstOrDefault(a => a.ActiveFeeDataID == vehicle.ActiveFeeDataID);

			//if ((openRecord.StartCheckIn.Value - now).TotalMinutes <= 60)
			//{
			//	return true;
			//}
			//else
			//{
			//	return false;
			//}

			var openRecord = _dbContext.ActiveFeeDatas
			.FirstOrDefault(a => a.ActiveFeeDataID == vehicle.ActiveFeeDataID);

			if (openRecord?.StartCheckIn == null)
				return false;

			return openRecord.StartCheckIn.Value.Date == now.Date;

		}

		public void SettingTimeVariables(bool sameDay, DateTime now, DateTime? startCheckIn, DateTime? currentCheckIn, decimal totalFee)
		{
			if (!sameDay)
			{
				startCheckIn = now;
				totalFee = 0.00m;
			}
			else
			{
				currentCheckIn = now;
				startCheckIn = startCheckIn;
			}
		}

		public bool IsHolidayOrTollExempt(bool isHoliday, bool isTaxExempt)
		{
			if (isHoliday == true || isTaxExempt == true)
			{
				return true;
			}
			return false;
		}

		public void ActionBranchTollOrNot(bool isHolidayOrTaxExempt, Vehicle vehicle, FeeDataReceipt tempFeeReceipt, DateTime now, ActiveFeeData tempActiveFeeData)
		{
			if (isHolidayOrTaxExempt)
			{
				//filling in the FeeDataReceipt record
				tempFeeReceipt.FeeCheckInTime = now;
				tempFeeReceipt.FeeAmount = 0.00m;
				tempFeeReceipt.VehicleID = vehicle.VehicleID;

				//filling in the ActiveFeeData record
				//tempActiveFeeData.StartCheckIn = now; //stämmer ej
				tempActiveFeeData.CurrentCheckIn = now;
				tempActiveFeeData.TotalFee = 0.00m;

				//Köra UpdateActiveFeeData
				bool successUpdate = UpdateActiveFeeData(tempActiveFeeData, vehicle);
				bool successCreate = CreateFeeDataReceipt(tempFeeReceipt);
			}
			else
			{
				//OM DET INTE ÄR HOLIDAY OCH DET BLIR RIKTIGA PRISER

				//tempActiveFeeData.StartCheckIn = now;
				tempActiveFeeData.CurrentCheckIn = now;
				tempActiveFeeData.TotalFee = GetTotalAmount(vehicle);



			}

		}

		//public bool UpdateActiveFeeData(ActiveFeeData activeFeeData, Vehicle vehicle)
		//{
		//	if (vehicle.ActiveFeeDataID.HasValue)
		//	{
		//		try
		//		{
		//			activeFeeData.ActiveFeeDataID = vehicle.ActiveFeeDataID.Value;
		//			_dbContext.ActiveFeeDatas.Update(activeFeeData);
		//			_dbContext.SaveChanges();
		//			return true;
		//		}
		//		catch (Exception ex)
		//		{
		//			Console.WriteLine(ex.ToString());
		//			return false;
		//		}


		//	}
		//	else
		//	{
		//		Console.WriteLine("Vehicle is not connected to ActiveFeeData record");
		//		return false;
		//	}		
		//}

		public bool UpdateActiveFeeData(ActiveFeeData activeFeeData, Vehicle vehicle)
		{
			if (vehicle.ActiveFeeDataID.HasValue)
			{
				try
				{
					var existing = _dbContext.ActiveFeeDatas
						.FirstOrDefault(a => a.ActiveFeeDataID == vehicle.ActiveFeeDataID.Value);

					if (existing == null)
					{
						Console.WriteLine("ActiveFeeData record not found.");
						return false;
					}

					existing.CurrentCheckIn = activeFeeData.CurrentCheckIn;
					existing.TotalFee = activeFeeData.TotalFee;

					_dbContext.SaveChanges();
					return true;
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex);
					return false;
				}
			}

			return false;
		}

		public bool CreateFeeDataReceipt(FeeDataReceipt tempFeeData)
		{
			if (tempFeeData != null)
			{
				try
				{
					_dbContext.FeeDataReceipts.Add(tempFeeData);
					_dbContext.SaveChanges();
					return true;
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.ToString());
					return false;
				}
			}

			else
			{
				Console.WriteLine("The FeeDataReceipt record is empty");
				return false;
			}
		}


		public decimal? GetTotalAmount(Vehicle vehicle)
		{
			try
			{
				decimal? tempTotalFee = _dbContext.ActiveFeeDatas
				.Where(a => a.ActiveFeeDataID == vehicle.ActiveFeeDataID)
				.Select(a => a.TotalFee)
				.FirstOrDefault();

				return tempTotalFee;
			}
			catch (Exception ex)
			{
				return -0.1m;
			}
		}

		
	}
}
