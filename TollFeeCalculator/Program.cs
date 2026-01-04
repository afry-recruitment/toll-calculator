namespace TollFeeCalculator;

using System;
using System.Collections.Generic;
using System.Linq;
using TollFeeCalculator;

class Program
{
    // SIMULATION OF A SIMPLIFIED REALWORLD SCENARIO
    // Where our program is processing data from a Sensor Database and sending it to a BillingService.
    // 1. Camera registration -> Sensor Database
    // 2. Sensor Data -> TollCaculator (This is our program) 
    // 3. TollFee (Our output) -> Billing Service

	static void Main(string[] args)
	{
		var calculator = new TollCalculator();

		// 1. SIMULATED SENSOR DATA
		// In a real system, this would come from your "Sensor Database"
		var sensorLogs = new List<(string Plate, VehicleType Type, DateTime Time)>
		{
			("ABC-123", VehicleType.Car, new DateTime(2013, 1, 2, 06, 15, 0)),
			("ABC-123", VehicleType.Car, new DateTime(2013, 1, 2, 07, 05, 0)),
			("ABC-123", VehicleType.Car, new DateTime(2013, 1, 2, 15, 30, 0)),
			
			("TRK-888", VehicleType.Tractor, new DateTime(2013, 1, 2, 08, 00, 0)), // Exempt
			
			("XYZ-789", VehicleType.Car, new DateTime(2013, 1, 2, 09, 00, 0)),
			("XYZ-789", VehicleType.Car, new DateTime(2013, 1, 2, 17, 30, 0))
		};

		Console.WriteLine("========================================");
		Console.WriteLine("   GOTHENBURG TOLL BATCH PROCESSOR      ");
		Console.WriteLine("========================================\n");

		// 2. ORCHESTRATION: Group by Plate to calculate daily totals
		var groupedPassages = sensorLogs.GroupBy(log => log.Plate);

		foreach (var group in groupedPassages)
		{
			string currentPlate = group.Key;
			VehicleType currentType = group.First().Type;
			DateTime[] times = group.Select(g => g.Time).ToArray();

			try
			{
				// 3. Create the Vehicle object with the Plate
				var vehicle = new Vehicle(currentPlate, currentType);

				// 4. Calculate
				int totalFee = calculator.GetTotalTollFee(vehicle, times);

				// 5. Output results (Simulating the "BillingService" input)
				Console.WriteLine($"PLATE: {vehicle.LicensePlate,-10} | TYPE: {vehicle.Type,-10}");
				Console.WriteLine($"  Passages: {times.Length}");
				Console.WriteLine($"  Final Billing Amount: {totalFee} SEK");
				Console.WriteLine("----------------------------------------");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"[ERROR] Could not process {currentPlate}: {ex.Message}");
			}
		}

		Console.WriteLine("\nProcessing Complete. Data ready for BillingService.");
	}
}
