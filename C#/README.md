# C# Implementation

This is my C# implementation of the toll fee calculator, built with .NET 10.

## How it works

The calculator determines the fee for each passage based on the time of day. It also handles the one-hour charging rule, the daily limit of 60 SEK, toll-free vehicles, weekends, and holidays.

The project includes an xUnit test project to check the calculations and the main rules.

## Project structure

- `TollCalculator/` – The console application and calculation logic.
- `TollCalculator.Tests/` – Unit tests.
- `TollCalculator.slnx` – Visual Studio solution.

## Run the application

Open TollCalculator.slnx in Visual Studio, set TollCalculator as the startup project, and press F5 or Ctrl+F5 to run it.

Alternatively, open a terminal in the C# folder and run:

```bash
dotnet run --project "TollCalculator/TollCalculator.csproj"
```

## Run the tests

In Visual Studio, open Test Explorer and select Run All Tests.

Alternatively, open a terminal in the C# folder and run:

```bash
dotnet test "TollCalculator.Tests/TollCalculator.Tests.csproj"
```

## Note

The holiday dates are hardcoded for 2013 in the current implementation. The logic would need to be updated to support other years.

## Bonus

The GIF in the original README is from the movie "Hackers"
Hack the planet!
