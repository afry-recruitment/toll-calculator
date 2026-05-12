using Interfaces;
using Models;
using Utils;

var tollCalculator = new TollCalculator();
var car = new Car();

Console.WriteLine("Single passage fees");
tollCalculator.PrintSingleFee("06:00-06:29", car, tollCalculator.SetDateTime(2026, 1, 2, 6, 0));
tollCalculator.PrintSingleFee("06:30-06:59", car, tollCalculator.SetDateTime(2026, 1, 2, 6, 30));
tollCalculator.PrintSingleFee("07:00-07:59", car, tollCalculator.SetDateTime(2026, 1, 2, 7, 0));
tollCalculator.PrintSingleFee("08:00-08:29", car, tollCalculator.SetDateTime(2026, 1, 2, 8, 0));
tollCalculator.PrintSingleFee("08:30-14:59", car, tollCalculator.SetDateTime(2026, 1, 2, 14, 59));
tollCalculator.PrintSingleFee("15:00-15:29", car, tollCalculator.SetDateTime(2026, 1, 2, 15, 0));
tollCalculator.PrintSingleFee("15:30-16:59", car, tollCalculator.SetDateTime(2026, 1, 2, 15, 30));
tollCalculator.PrintSingleFee("17:00-17:59", car, tollCalculator.SetDateTime(2026, 1, 2, 17, 0));
tollCalculator.PrintSingleFee("18:00-18:29", car, tollCalculator.SetDateTime(2026, 1, 2, 18, 0));
tollCalculator.PrintSingleFee("Outside toll hours", car, tollCalculator.SetDateTime(2026, 1, 2, 18, 30));

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
    tollCalculator.PrintSingleFee(vehicle.GetVehicleType(), vehicle, tollCalculator.SetDateTime(2026, 1, 2, 7, 0));
}

Console.WriteLine();
Console.WriteLine("Toll-free dSetDateTimees");
tollCalculator.PrintSingleFee("Weekend", car, tollCalculator.SetDateTime(2026, 1, 5, 7, 0));
tollCalculator.PrintSingleFee("2013 public holiday", car, tollCalculator.SetDateTime(2026, 1, 1, 7, 0));
tollCalculator.PrintSingleFee("2013 non-holiday weekday", car, tollCalculator.SetDateTime(2026, 2, 1, 7, 0));
tollCalculator.PrintSingleFee("Non-2013 weekday", car, tollCalculator.SetDateTime(2026, 1, 2, 7, 0));

Console.WriteLine();
Console.WriteLine("Daily fee scenarios");
tollCalculator.PrintDailyFee("First pass in 60-minute window",car,tollCalculator.SetDateTime(2026, 1, 2, 6, 0));

tollCalculator.PrintDailyFee("Within 60 minutes, next fee is higher", car,
    tollCalculator.SetDateTime(2026, 1, 2, 6, 0),
    tollCalculator.SetDateTime(2026, 1, 2, 6, 30));

tollCalculator.PrintDailyFee("Within 60 minutes, next fee is lower", car,
    tollCalculator.SetDateTime(2026, 1, 2, 7, 0),
    tollCalculator.SetDateTime(2026, 1, 2, 7, 30));
    tollCalculator.PrintDailyFee("More than 60 minutes apart",car,
    tollCalculator.SetDateTime(2026, 1, 2, 6, 0),
    tollCalculator.SetDateTime(2026, 1, 2, 7, 1));

tollCalculator.PrintDailyFee("Daily fee is capped at 60", car,
    tollCalculator.SetDateTime(2026, 1, 2, 6, 0),
    tollCalculator.SetDateTime(2026, 1, 2, 7, 1),
    tollCalculator.SetDateTime(2026, 1, 2, 8, 2),
    tollCalculator.SetDateTime(2026, 1, 2, 15, 30),
    tollCalculator.SetDateTime(2026, 1, 2, 17, 0));
