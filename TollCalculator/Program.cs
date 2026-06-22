using TollCalculator.Models;
using TollCalculator.Services;

var registry = new VehicleRegistry(new List<Vehicle>
{
    new Car("CAR123"),
    new Bus("BUS888")
});

var holidayService = new SwedishHolidayService();
var calculator = new TollCalculatorService(registry, holidayService);

PrintFeeSchedule(calculator);
RunDemo(calculator);

static void PrintFeeSchedule(TollCalculatorService calculator)
{
    Console.WriteLine("=== Fee Schedule =======");
    foreach (var (from, to, fee) in TollCalculatorService.GetFeeSchedule())
    {
        Console.WriteLine($"  {from:hh\\:mm} - {to:hh\\:mm}   {fee,2} SEK");
    }
    Console.WriteLine("========================\n");
}

static void RunDemo(TollCalculatorService calculator)
{
    var entries = new List<TollEntry>
    {
        new TollEntry("CAR123", new DateTime(2026, 6, 8, 7, 0, 0)),   // 18 SEK
        new TollEntry("CAR123", new DateTime(2026, 6, 8, 7, 30, 0)),  // absorbed
        new TollEntry("CAR123", new DateTime(2026, 6, 9, 11, 30, 0)), // 8 
        new TollEntry("BUS888", new DateTime(2026, 6, 8, 7, 0, 0))    // toll free
    };

    Console.WriteLine("=== Toll Calculation Demo ===");
    var results = calculator.Calculate(entries);

    foreach (var result in results)
    {
        Console.WriteLine($"\n{result.RegNo}: {result.TotalFee} SEK total");
        foreach (var detail in result.Details)
        {
            Console.WriteLine($"  {detail.EntryTime:yy-MM-dd HH:mm}   {detail.Fee,2} SEK");
        }
    }
}