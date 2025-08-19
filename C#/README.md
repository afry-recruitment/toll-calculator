**Toll Calculator**

A toll calculator that calculates toll fees for a vehicle in one day.

**Functionality**

- Calculate fees based on time of day
  Takes into account:
- Daily cap (maximum 60 SEK per day)
- Toll-free vehicle types (motorbike, tractor, emergency, diplomatic, foreign, military)
- Toll-free dates (weekends and swedish holidays via Public Holiday)
- Times/dates is data driven via FeeTable in code

**Structure**

TollCalculatorSystem.Services: contains calculation logic
TollCalculatorSystem.Models: vehicle definition
TollCalculatorSystem.Tests: unit tests (xUnit)

**Tests**

Covers (selection):

- 60 minute rule
- Daily cap
- Edge cases (exactly at interval change, seconds before etc)
- Robustness (null, mixed dates)

**How to run**

- Clone repository
- Run tests

**Example usage:**

var calculator = new TollCalculator();

var passes = new[]
{
new DateTime(2025, 8, 15, 15, 00, 00),
new DateTime(2025, 8, 15, 06, 15, 00),
new DateTime(2025, 8, 15, 07, 14, 00)
};

var fee = calculator.CalculateTollFee(VehicleType.Car, passes);

Console.WriteLine($"Calculated fee: {fee}");
