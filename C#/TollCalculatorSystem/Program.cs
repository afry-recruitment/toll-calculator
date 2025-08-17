using TollCalculatorSystem.Models;
using TollCalculatorSystem.Services;

var calculator = new TollCalculator();
//var car = new Car();
var mc = new Motorbike();

var passes = new[]
{
    new DateTime(2025, 8, 15, 15, 00, 00),
    new DateTime(2025, 8, 15, 06, 15, 00),
    new DateTime(2025, 8, 15, 07, 30, 00)
};

var fee = calculator.CalculateTollFee(mc, passes);

Console.WriteLine($"Calculated fee: {fee}");

// Visually inspect passes and fees
var tester = new TollCalculator();
tester.InspectAllTimeSlots();


// BUG FIXES (CRITICAL)

// - Extract hardcoded time/fee logic into data structure
// - Create toll schedule class or dict for easier maintenance
// - Move magic numbers (fees) to constants or config

// TESTS
// - Test motorbike is toll-free
// - Test weekend/holiday dates are toll-free (investigate holiday provider)
// - Test multiple intervals beyond 60 minutes
// - Test edge cases (midnight, exact hour etc)
// - Test invalid input (null vehicle, empty date array)

// - Consider separate classes: TollSchedule, TollRules, etc.
// - Add XML documentation comments for public methods

// - Add null checks and validation
// - Handle empty or unsorted date arrays
// - Add meaningful exceptions with messages