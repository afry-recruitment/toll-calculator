using TollCalculatorSystem.Models;
using TollCalculatorSystem.Services;

var calculator = new TollCalculator();
var car = new Car();

var passes = new[]
{
    new DateTime(2025, 8, 16, 06, 15, 00),
    new DateTime(2025, 8, 16, 06, 45, 00),
    new DateTime(2025, 8, 16, 07, 30, 00)
};

var fee = calculator.GetTollFee(car, passes);

Console.WriteLine($"Calculated fee: {fee}");

// BUG FIXES (CRITICAL)
// - Fix time calculation bug: date.Millisecond → (date - intervalStart).TotalMinutes is not right
// - Fix logic bug in GetTollFee: hour >= 8 && hour <= 14 && minute >= 30 (wrong condition)

// - Rename one GetTollFee method to avoid overload confusion
// - GetTollFee(vehicle, dates[]) stays as main method
// - GetTollFee(date, vehicle) → GetSingleTollFee(date, vehicle)

// - Replace string returns with enum for type safety
// - Create VehicleType enum: Car, Motorbike, Tractor, Emergency, etc.
// - Update Vehicle interface to return VehicleType instead of string

// - Extract hardcoded time/fee logic into data structure
// - Create toll schedule class or dict for easier maintenance
// - Move magic numbers (fees) to constants or config

// TESTS
// - Test motorbike is toll-free
// - Test weekend/holiday dates are toll-free (investigate holiday provider)
// - Test multiple intervals beyond 60 minutes
// - Test edge cases (midnight, exact hour etc)
// - Test invalid input (null vehicle, empty date array)

// - Move TollFreeVehicles enum to Models namespace
// - Consider separate classes: TollSchedule, TollRules, etc.
// - Add XML documentation comments for public methods

// - Add null checks and validation
// - Handle empty or unsorted date arrays
// - Add meaningful exceptions with messages

// - Sort dates array if not guaranteed to be sorted