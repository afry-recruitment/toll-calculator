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
			//DateTime? startCheckin = null;

			while (true)
			{
				
				now = timeServices.getCurrentDateTime();
				List<DateTime> holidayDates = holidayService.ReadCsv(filePath);
				isWeekEnd = timeServices.isWeekEnd(now);
				isHoliday = timeServices.isHoliday(now, holidayDates);
				isRushour = timeServices.isRushHour(now);
				var result = vehicleServices.isVehicleRetrieved();
				if (result.success)
				{
					//temp variables
					//----------------
					DateTime? startCheckIn = null;
					DateTime? currentCheckIn = null; ;
					decimal totalFee = 0;
					FeeDataReceipt tempFeeReceipt = new FeeDataReceipt();
					ActiveFeeData tempActiveFeeData = new ActiveFeeData();
					//----------------------------------------

					bool taxExempt = vehicleServices.isTaxExempt(result.vehicle);
					bool openRecord = activeFeeDataServices.VehicleHasOpenRecord(result.vehicle);
					bool? success = activeFeeDataServices.CreateNewRecord(result.vehicle, openRecord);
					if (success == false)
						continue;

					bool sameDay = activeFeeDataServices.IsWithinSameDay(result.vehicle, now);
					activeFeeDataServices.SettingTimeVariables(sameDay, now, startCheckIn, currentCheckIn, totalFee);

					bool isHolidayOrTaxExempt = activeFeeDataServices.IsHolidayOrTollExempt(isHoliday, taxExempt);
					activeFeeDataServices.ActionBranchTollOrNot(isHolidayOrTaxExempt, result.vehicle, tempFeeReceipt, now, tempActiveFeeData);

				}

			}

		

			
		}
		// Kör metoden för att läsa CSV
		
	}
}