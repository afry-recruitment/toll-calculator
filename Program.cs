using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using tollCalculator.Data;
using tollCalculator.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using tollCalculator.Services;
using static System.Formats.Asn1.AsnWriter;

namespace TollFeeCalculator;
internal class Program
{
	private static void Main(string[] args)
	{
		var host = Host.CreateDefaultBuilder(args)
			.ConfigureServices((context, services) =>
			{
				var connectionString = context.Configuration.GetConnectionString("DefaultConnection");
				services.AddDbContext<ApplicationDbContext>(options =>
					options.UseSqlServer(connectionString));

				//Services
				services.AddTransient<HolidayServices>();
				services.AddTransient<TimeServices>();
				services.AddTransient<VehicleServices>();
				services.AddTransient<ActiveFeeDataServices>();
			})
			.Build();

		string projectRoot = AppContext.BaseDirectory; // typ bin\Debug\netX.X
		string filePath = Path.Combine(projectRoot, @"..\..\..\Holidays\2025.csv");

		//Service objects
		var holidayService = host.Services.GetRequiredService<HolidayServices>();
		var timeServices = host.Services.GetRequiredService<TimeServices>();
		var vehicleServices = host.Services.GetRequiredService<VehicleServices>();
		var activeFeeDataServices = host.Services.GetRequiredService<ActiveFeeDataServices>();

		List<DateTime> holidayList = holidayService.ReadCsv(Path.Combine(projectRoot, @"..\..\..\Holidays\2025.csv"));


		using (var scope = host.Services.CreateScope())
		{
			var _dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

			//Declaring variables
			DateTime now;
			bool isHoliday = false;
			bool isWeekend = false;
			bool isRushour = false;

			while (true)
			{
				var result = vehicleServices.IsVehicleRetrieved();
				if (result.success)
				{
					//temp variables
					//----------------
					FeeDataReceipt tempFeeReceipt = new FeeDataReceipt();
					ActiveFeeData tempActiveFeeData = new ActiveFeeData();
					ActiveFeeData activeFeeDataRecord = new ActiveFeeData();
					//----------------------------------------

					now = timeServices.GetCurrentDateTime();

					///Testing times and dates
					//now = new DateTime(2025,10,6,7,35,0); //Rush hour
					now = new DateTime(2025,10,6,15,15,0); //Rush hour
					//now = new DateTime(2025, 10, 6, 10, 11, 0); //Regular fee hours
					//now = new DateTime(2025, 10, 11, 10, 11, 0); //Weekend- Saturday
					//now = new DateTime(2025,12,25,12,00,0); //Holiday
					//now = new DateTime(2025,10,6,4,42,0); //Toll free hour


					List<DateTime> holidayDates = holidayService.ReadCsv(filePath);
					isWeekend = timeServices.IsWeekend(now);
					isHoliday = timeServices.IsHoliday(now, holidayDates);
					isRushour = timeServices.IsRushHour(now);

					bool taxExempt = vehicleServices.IsTaxExempt(result.vehicle);
					bool noTollHours = timeServices.IsDuringNoTollHours(now);
					bool openRecord = activeFeeDataServices.VehicleHasOpenRecord(result.vehicle);
					bool? success = activeFeeDataServices.CreateNewRecord(result.vehicle, openRecord);
					if (success == false)
						continue;

					activeFeeDataRecord = activeFeeDataServices.ActiveFeeRecord(result.vehicle);

					bool sameDay = activeFeeDataServices.IsWithinSameDay(now, activeFeeDataRecord);
					bool isHolidayOrTaxExempt = activeFeeDataServices.IsHolidayOrTollExempt(isHoliday, taxExempt, isWeekend);
					activeFeeDataServices.ActionBranchTollOrNot(isHolidayOrTaxExempt, noTollHours, result.vehicle, tempFeeReceipt, now, tempActiveFeeData, isRushour, sameDay, activeFeeDataRecord);
					
				}
				Console.WriteLine("End of loop");
			}
		}
	}
}