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

		/// <summary>
		/// Whether or not the vehicle already has an ActiveFeeData record in the database
		/// </summary>
		/// <param name="vehicle"></param>
		/// <returns>true/false</returns>
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
				Console.WriteLine($"Error saving new record to database: {ex.Message}");
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
				Console.WriteLine($"Error linking record with vehicle: {ex.Message}");
			}
		}

		/// <summary>
		/// Checks if the vehicle has been checked in within the same _day_, not within 24 hours
		/// </summary>
		/// <param name="now"></param>
		/// <param name="activeFeeDataRecord"></param>
		/// <returns>returns bool</returns>
		public bool IsWithinSameDay(DateTime now, ActiveFeeData activeFeeDataRecord)
		{
			if (activeFeeDataRecord?.StartCheckIn == null)
				return false;

			return activeFeeDataRecord.StartCheckIn.Value.Date == now.Date;
		}

		public bool IsHolidayOrTollExempt(bool isHoliday, bool isTaxExempt, bool isWeekend)
		{
			if (isHoliday == true || isTaxExempt == true || isWeekend)
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// The branch method for calculating the cost, managing amounts and handling toll exeptions
		/// </summary>
		/// <param name="isHolidayOrTaxExempt"></param>
		/// <param name="isDuringNoTollHours"></param>
		/// <param name="vehicle"></param>
		/// <param name="tempFeeReceipt"></param>
		/// <param name="now"></param>
		/// <param name="tempActiveFeeData"></param>
		/// <param name="isRushHour"></param>
		/// <param name="sameDay"></param>
		/// <param name="activeFeeDataRecord"></param>
		public void ActionBranchTollOrNot(bool isHolidayOrTaxExempt, bool isDuringNoTollHours, Vehicle vehicle,FeeDataReceipt tempFeeReceipt, DateTime now,
					ActiveFeeData tempActiveFeeData, bool isRushHour, bool sameDay,	ActiveFeeData activeFeeDataRecord)
		{
			tempActiveFeeData.CurrentCheckIn = now;
			tempActiveFeeData.TotalFee = activeFeeDataRecord.TotalFee;

			tempFeeReceipt.FeeCheckInTime = now;
			tempFeeReceipt.VehicleID = vehicle.VehicleID;
			tempFeeReceipt.FeeAmount = 0.00m;

			FeeDataReceipt? latestPaidFeeDataReceipt = GetLatestPaidReceiptSameDay(vehicle.VehicleID, now);
			bool withinSameHour = IsWithinSameHour(now, latestPaidFeeDataReceipt);

			//All exeptions when not paying toll fee
			if (isHolidayOrTaxExempt || withinSameHour || isDuringNoTollHours)
			{
				if (isHolidayOrTaxExempt && !sameDay)
				{
					tempActiveFeeData.StartCheckIn = now;
					tempActiveFeeData.TotalFee = 0m;
				}
				else if (isHolidayOrTaxExempt && sameDay)
				{
					tempActiveFeeData.StartCheckIn = activeFeeDataRecord.StartCheckIn;
					tempActiveFeeData.TotalFee = 0m;
				}
				else if (isDuringNoTollHours && !sameDay)
				{
					tempActiveFeeData.StartCheckIn = now;
					tempActiveFeeData.TotalFee = 0m;
				}
				else if (isDuringNoTollHours && sameDay)
				{
					tempActiveFeeData.StartCheckIn = activeFeeDataRecord?.StartCheckIn;
				}
				else if (withinSameHour)
				{
					tempActiveFeeData.StartCheckIn = activeFeeDataRecord.StartCheckIn;
				}

				UpdateActiveFeeData(tempActiveFeeData, activeFeeDataRecord, vehicle);
				CreateFeeDataReceipt(tempFeeReceipt);
				return;
			}

			// Normal toll fee
			if (!sameDay)
			{
				tempActiveFeeData.StartCheckIn = now;
				tempActiveFeeData.TotalFee = 0m;
			}
			else
			{
				tempActiveFeeData.StartCheckIn = activeFeeDataRecord.StartCheckIn;
			}

			tempFeeReceipt.FeeAmount = CalculatingToll(isRushHour, now);

			if (!ExceedsDailyAmount(tempFeeReceipt, tempActiveFeeData.TotalFee, tempActiveFeeData))
			{
				tempActiveFeeData.TotalFee += tempFeeReceipt.FeeAmount;
			}

			UpdateActiveFeeData(tempActiveFeeData, activeFeeDataRecord, vehicle);
			CreateFeeDataReceipt(tempFeeReceipt);
		}

		/// <summary>
		/// Retrieves the ActiveFeeRecord for the current vehicle
		/// </summary>
		/// <returns></returns>
		public ActiveFeeData? ActiveFeeRecord(Vehicle vehicle)
		{
			if (vehicle.ActiveFeeDataID.HasValue)
			{
				try
				{
					return _dbContext.ActiveFeeDatas
						.FirstOrDefault(a => a.ActiveFeeDataID == vehicle.ActiveFeeDataID.Value);
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Error retreiving record from database: {ex.Message}");
					return null;
				}
			}
			else
			{
				return null;
			}

		}
		
		public bool UpdateActiveFeeData(ActiveFeeData tempActiveFeeData, ActiveFeeData activeFeeDataRecord, Vehicle vehicle)
		{
			if (vehicle.ActiveFeeDataID.HasValue)
			{
				try 
				{ 
					
				activeFeeDataRecord.StartCheckIn = tempActiveFeeData.StartCheckIn;					
				activeFeeDataRecord.CurrentCheckIn = tempActiveFeeData.CurrentCheckIn;
				activeFeeDataRecord.TotalFee = tempActiveFeeData.TotalFee;

					_dbContext.SaveChanges();
					//Console.WriteLine($"CurrentCheckIn: {activeFeeDataRecord.CurrentCheckIn}, StartCheckIn:{activeFeeDataRecord.StartCheckIn}, TotalFee: {activeFeeDataRecord.TotalFee}");

					return true;
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Error updating record: {ex.Message}");
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
					Console.WriteLine($"Error creating record: {ex.Message}");
					return false;
				}
			}

			else
			{
				Console.WriteLine("The FeeDataReceipt record is empty");
				return false;
			}
		}

		/// <summary>
		/// Checks if current check in is within or exactly 60 min since last _paid_ check in.
		/// </summary>
		/// <param name="now"></param>
		/// <param name="latestPaidReceipt"></param>
		/// <returns>bool</returns>
		public bool IsWithinSameHour(DateTime now, FeeDataReceipt latestPaidReceipt)
		{
			if (latestPaidReceipt == null)
				return false;

			double minutesSinceLastPaid = (now - latestPaidReceipt.FeeCheckInTime).TotalMinutes;

			return minutesSinceLastPaid >= 0 && minutesSinceLastPaid <= 60;
		}

		public decimal CalculatingToll(bool isRushour, DateTime now )
		{
			if (isRushour)
			{
				return 18.00m;
			}

			TimeSpan currentTime = now.TimeOfDay;

			//Will not run if rush hour
			return currentTime switch
			{
				TimeSpan time when (time >= new TimeSpan(6, 0, 0) && time <= new TimeSpan(6, 29, 59)) => 8.00m,//between 6:00 - 6:29:59
				TimeSpan time when (time >= new TimeSpan(6, 30, 0) && time <= new TimeSpan(6, 59, 59)) => 13.00m,//between 6:30 - 6:59:59
				TimeSpan time when (time >= new TimeSpan(8, 0, 0) && time <= new TimeSpan(8, 29, 59)) => 13.00m,//between 8:00 - 8:29:59
				TimeSpan time when (time >= new TimeSpan(8, 30, 0) && time <= new TimeSpan(14, 59, 59)) => 8.00m,//between 8:30 - 14:59:59
				TimeSpan time when (time >= new TimeSpan(15, 0, 0) && time <= new TimeSpan(15, 29, 59)) => 13.00m,//between 15:00 - 15:29:59
				TimeSpan time when (time >= new TimeSpan(17, 0, 0) && time <= new TimeSpan(17, 59, 59)) => 13.00m,//between 17:00 - 17:59:59
				TimeSpan time when (time >= new TimeSpan(18, 0, 0) && time <= new TimeSpan(18, 29, 59)) => 8.00m,//between 18:00 - 18:29:59
				_ => 0.00m,
			};
		}
		public bool ExceedsDailyAmount(FeeDataReceipt tempFeeReceipt, decimal? totalAmount, ActiveFeeData tempActiveFeeData)
		{
			if (totalAmount + tempFeeReceipt.FeeAmount > 60.00m)
			{
				//the adjusted toll fee if daily amount has been maxed
				tempFeeReceipt.FeeAmount = 60.00m - (totalAmount ?? 0m);
				tempActiveFeeData.TotalFee = 60.00m;
				return true;
			}
			return false;
		}
		
		/// <summary>
		/// Get the latest connected FeeDataReceipt from the database, based on current date and if the fee was greater than 0
		/// </summary>
		/// <param name="vehicleId"></param>
		/// <param name="now"></param>
		/// <returns>FeeDataReceipt object</returns>
		public FeeDataReceipt? GetLatestPaidReceiptSameDay(int vehicleId, DateTime now)
		{
			try
			{
				return _dbContext.FeeDataReceipts
				.Where(r => r.VehicleID == vehicleId
				&& r.FeeAmount > 0
				&& r.FeeCheckInTime.Date == now.Date)
				.OrderByDescending(r => r.FeeCheckInTime)
				.FirstOrDefault();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error retrieving record or no previous records found: {ex.Message}");
				return null;
			}
			
		}
	}
}
