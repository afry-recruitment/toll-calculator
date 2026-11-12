# Toll Calculator (.NET 10)

A toll fee calculator based on city congestion requirements.

## Requirements covered

- Fees vary by time of day (8–18 SEK)
- Rush-hour windows use highest fee levels
- Maximum total fee per day is 60 SEK
- Vehicle is charged once per 60-minute window
- If multiple passes happen inside one 60-minute window, only the highest fee is charged
- Some vehicle types are toll-free
- Weekends and configured holidays are toll-free

## Project structure

- `C#/src/TollCalculator/Models`
  - Domain types (`IVehicle`, `VehicleType`, `TollFreeVehicles`, `Car`, `Motorbike`)
- `C#/src/TollCalculator/Services`
  - `TollCalculator` orchestration logic
- `C#/src/TollCalculator/Services/Policies`
  - `DefaultTollFeeSchedule`
  - `DefaultTollFreeVehiclePolicy`
  - `HolidayPolicy2013`
  - Policy interfaces (`ITollFeeSchedule`, `ITollFreeVehiclePolicy`, `ITollFreeDatePolicy`)
- `C#/tests/TollCalculator.Tests`
  - Unit tests for orchestration and policy classes

## Run the demo

From repository root:

1. `cd C#`
2. `dotnet run --project src/TollCalculator/TollCalculator.csproj`

The demo is implemented in `C#/src/TollCalculator/Program.cs` and prints a sample daily toll calculation.

## Run tests

From repository root:

1. `cd C#`
2. `dotnet test`

## Notes

- Holiday behavior is currently implemented with a 2013-specific policy (`HolidayPolicy2013`).
- The policy-based structure is designed for future extension (new regions/years) without changing core calculator flow.

