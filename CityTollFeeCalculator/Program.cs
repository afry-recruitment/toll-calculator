
using CityTollFeeCalculator.Tolls;
using CityTollFeeCalculator.Tolls.Policy;
using CityTollFeeCalculator.Vehicle;

List<DateTime> tollPasses = new()
        {
            new DateTime(2025, 4, 3, 6, 15, 0),  // 06:15 AM - 8 SEK
            new DateTime(2025, 4, 3, 6, 45, 0),  // 06:45 AM - 13 SEK
            new DateTime(2025, 4, 3, 7, 30, 0),  // 07:30 AM - 18 SEK
            new DateTime(2025, 4, 3, 8, 10, 0),  // 08:10 AM - 13 SEK
            new DateTime(2025, 4, 3, 12, 45, 0), // 12:45 PM - 8 SEK
            new DateTime(2025, 4, 3, 15, 29, 0), // 04:20 PM - 18 SEK
            new DateTime(2025, 4, 3, 16, 20, 0), // 04:20 PM - 18 SEK
            new DateTime(2025, 4, 3, 18, 10, 0), // 06:10 PM - 8 SEK
            new DateTime(2025, 4, 3, 19, 30, 0)  // 07:30 PM - 0 SEK (no fee)
        };

List<IVehicle> vehicles = [new Tractor(), new Truck()];

ITollFeePolicy cityFeePolicy = new CityTollFeePolicy();
TollCalculator calculator = new TollCalculator(cityFeePolicy);

foreach (var vehicle in vehicles)
{
    int totalFee = calculator.GetTollFee(vehicle, tollPasses);
    Console.WriteLine($"Total Toll Fee for the {vehicle.GetVehicleType()}: {totalFee} SEK");
}
