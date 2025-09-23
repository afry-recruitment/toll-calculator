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

		using (var scope = host.Services.CreateScope())
		{
			var _dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

			//Declaring variables
			DateTime now;
			bool isHoliday = false;
			bool isWeekEnd = false;
			bool isRushour = false;

			while (true)
			{
				//now = timeServices.GetCurrentDateTime();
				//now = new DateTime(2025, 9, 22, 7, 32, 0); //testing rush hour
				//now = new DateTime(2025, 12, 24, 17, 35, 0); //testing within an hour later
				//now = new DateTime(2025, 10, 3, 07, 45, 0);
				//now = new DateTime(2025, 10, 3, 08, 16, 0);
				//now = new DateTime(2025, 10, 3, 09, 00, 0);
				now = new DateTime(2025, 10, 4, 08, 15, 0);
				//now = new DateTime(2025, 10, 3, 10, 10, 0);

				List<DateTime> holidayDates = holidayService.ReadCsv(filePath);
				isWeekEnd = timeServices.IsWeekend(now);
				isHoliday = timeServices.IsHoliday(now, holidayDates);
				isRushour = timeServices.IsRushHour(now);

				var result = vehicleServices.IsVehicleRetrieved();
				if (result.success)
				{
					//temp variables
					//----------------
					FeeDataReceipt tempFeeReceipt = new FeeDataReceipt();
					ActiveFeeData tempActiveFeeData = new ActiveFeeData();
					ActiveFeeData activeFeeDataRecord = new ActiveFeeData();
					//----------------------------------------

					bool taxExempt = vehicleServices.IsTaxExempt(result.vehicle);
					bool noTollHours = timeServices.IsDuringNoTollHours(now);
					bool openRecord = activeFeeDataServices.VehicleHasOpenRecord(result.vehicle);
					bool? success = activeFeeDataServices.CreateNewRecord(result.vehicle, openRecord);
					if (success == false)
						continue;

					activeFeeDataRecord = activeFeeDataServices.ActiveFeeRecord(result.vehicle);

					bool sameDay = activeFeeDataServices.IsWithinSameDay(now, activeFeeDataRecord);
					bool isHolidayOrTaxExempt = activeFeeDataServices.IsHolidayOrTollExempt(isHoliday, taxExempt);
					activeFeeDataServices.ActionBranchTollOrNot(isHolidayOrTaxExempt, noTollHours, result.vehicle, tempFeeReceipt, now, tempActiveFeeData, isRushour, sameDay, activeFeeDataRecord);

					Console.WriteLine("End of loop");
				}
			}
		}
	}
}