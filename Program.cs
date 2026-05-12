using Interfaces;
using Models;
using Utils;

var car = new Car();

Console.WriteLine("Single passage fees");
TollCalculator.PrintSingleFee("06:00-06:29", car, TollCalculator.SetDateTime(2026, 1, 2, 6, 0));
TollCalculator.PrintSingleFee("06:30-06:59", car, TollCalculator.SetDateTime(2026, 1, 2, 6, 30));
TollCalculator.PrintSingleFee("07:00-07:59", car, TollCalculator.SetDateTime(2026, 1, 2, 7, 0));
TollCalculator.PrintSingleFee("08:00-08:29", car, TollCalculator.SetDateTime(2026, 1, 2, 8, 0));
TollCalculator.PrintSingleFee("08:30-14:59", car, TollCalculator.SetDateTime(2026, 1, 2, 14, 59));
TollCalculator.PrintSingleFee("15:00-15:29", car, TollCalculator.SetDateTime(2026, 1, 2, 15, 0));
TollCalculator.PrintSingleFee("15:30-16:59", car, TollCalculator.SetDateTime(2026, 1, 2, 15, 30));
TollCalculator.PrintSingleFee("17:00-17:59", car, TollCalculator.SetDateTime(2026, 1, 2, 17, 0));
TollCalculator.PrintSingleFee("18:00-18:29", car, TollCalculator.SetDateTime(2026, 1, 2, 18, 0));
TollCalculator.PrintSingleFee("Outside toll hours", car, TollCalculator.SetDateTime(2026, 1, 2, 18, 30));

Console.WriteLine();
Console.WriteLine("Toll-free vehicles");
var tollFreeVehicles = new IVehicle[]
{
    new Motorbike(),
    new Tractor(),
    new Emergency(),
    new Diplomat(),
    new Foreign(),
    new Military()
};

foreach (var vehicle in tollFreeVehicles)
{
    TollCalculator.PrintSingleFee(vehicle.GetVehicleType(), vehicle, 
        TollCalculator.SetDateTime(2026, 1, 2, 7, 0));
}

Console.WriteLine();
Console.WriteLine("Toll-free dSetDateTimees");
TollCalculator.PrintSingleFee("Weekend", car, TollCalculator.SetDateTime(2026, 1, 5, 7, 0));
TollCalculator.PrintSingleFee("2013 public holiday", car, TollCalculator.SetDateTime(2026, 1, 1, 7, 0));
TollCalculator.PrintSingleFee("2013 non-holiday weekday", car, TollCalculator.SetDateTime(2026, 2, 1, 7, 0));
TollCalculator.PrintSingleFee("Non-2013 weekday", car, TollCalculator.SetDateTime(2026, 1, 2, 7, 0));

Console.WriteLine();
Console.WriteLine("Daily fee scenarios");
TollCalculator.PrintDailyFee("First pass in 60-minute window",car,
    TollCalculator.SetDateTime(2026, 1, 2, 6, 0));

TollCalculator.PrintDailyFee("Within 60 minutes, next fee is higher", car,
    TollCalculator.SetDateTime(2026, 1, 2, 6, 0),
    
TollCalculator.SetDateTime(2026, 1, 2, 6, 30));
TollCalculator.PrintDailyFee("Within 60 minutes, next fee is lower", car,
    TollCalculator.SetDateTime(2026, 1, 2, 7, 0), 
    
TollCalculator.SetDateTime(2026, 1, 2, 7, 30));
TollCalculator.PrintDailyFee("More than 60 minutes apart",car,
    TollCalculator.SetDateTime(2026, 1, 2, 6, 0),
    
TollCalculator.SetDateTime(2026, 1, 2, 7, 1));
TollCalculator.PrintDailyFee("Daily fee is capped at 60", car, 
        TollCalculator.SetDateTime(2026, 1, 2, 6, 0),
        
TollCalculator.SetDateTime(2026, 1, 2, 7, 1), 
TollCalculator.SetDateTime(2026, 1, 2, 8, 2), 
TollCalculator.SetDateTime(2026, 1, 2, 15, 30), 
TollCalculator.SetDateTime(2026, 1, 2, 17, 0));
