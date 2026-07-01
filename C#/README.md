# Toll Fee Calculator

A C# class library for calculating vehicle toll fees based on time of day, vehicle type, and date.

## Background

This is a reimplementation of the city toll fee calculator. The original code contained several bugs that have been identified and fixed.

## Requirements

- .NET 10
- [PublicHoliday](https://www.nuget.org/packages/PublicHoliday) - Swedish public holiday calculations
- [Microsoft.Extensions.Logging.Abstractions](https://www.nuget.org/packages/Microsoft.Extensions.Logging.Abstractions) - logging abstractions
- [Microsoft.Extensions.DependencyInjection](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection) - DI support

## Build and Test

```bash
dotnet build
dotnet test
```

## Fee Schedule

| Time        | Fee (SEK) |
|-------------|-----------|
| 06:00-06:29 | 8         |
| 06:30-06:59 | 13        |
| 07:00-07:59 | 18        |
| 08:00-08:29 | 13        |
| 08:30-14:59 | 8         |
| 15:00-15:29 | 13        |
| 15:30-16:59 | 18        |
| 17:00-17:59 | 13        |
| 18:00-18:29 | 8         |
| Other       | 0         |

Maximum fee per day: **60 SEK**

## Toll-Free

The following are never charged:

- **Vehicles:** Motorbike, Tractor, Emergency, Diplomat, Foreign, Military
- **Dates:** Weekends, Swedish public holidays, the day before a public holiday, and all of July

## Project Structure

```
TollCalculator/
├── Models/
│   ├── IVehicle.cs                     # Vehicle interface and all vehicle implementations
│   ├── VehicleType.cs                  # VehicleType enum + IsTollFree() extension
│   └── FeeInterval.cs                  # Time interval and fee record
├── Services/
│   └── HolidayService.cs               # Toll-free date logic
├── Settings/
│   └── TollSettings.cs                 # Configurable business rules and fee schedule
├── TollCalculator.cs                   # Main calculator
└── ServiceCollectionExtensions.cs      # DI registration
```

## Bugs Fixed

### 1. Time difference calculation (Critical)

The original code used `date.Millisecond - intervalStart.Millisecond` which reads only the sub-second component (0-999 ms), not total elapsed time. This made the 60-minute window check essentially random.

**Fix:** Replaced with `(pass - windowStart).TotalMinutes`.

### 2. Hour window never advanced (Critical)

`intervalStart` never updated, meaning all time differences were calculated relative to the very first pass. A vehicle passing at 08:00 was compared against 06:00 even if 07:30 had started a new window.

**Fix:** Rewrote the loop with a sliding window that resets when a new hour begins.

### 3. Holiday check hardcoded to 2013 (Critical)

`IsTollFreeDate` was wrapped in `if (year == 2013)`, meaning no holiday exemptions applied for any other year.

**Fix:** Replaced with the `PublicHoliday` library which computes Swedish public holidays for any year.

### 4. Incorrect fee condition for 15:30-16:59 (Moderate)

The condition `hour == 15 && minute >= 0 || hour == 16 && minute <= 59` caught all of hour 15 due to operator precedence. It only worked accidentally because the 13 SEK clause appeared earlier in the chain.

**Fix:** Replaced the entire if-else chain with a `FeeInterval` table lookup.

## Design Decisions

- **`PublicHoliday` NuGet package** - accurate Swedish public holiday calculations across all years
- **`TollSettings`** - business rules like max daily fee and hour window are configurable without changing the code
- **`FeeInterval`** - fee schedule is defined as a list of time intervals, making it easy to add or modify fees
- **`HolidayService`** - all date logic is isolated in one place, separate from the calculator logic
- **`VehicleTypeExtensions`** - toll-free vehicle types are defined in one place, making it easy to add or remove types

## Logging and Dependency Injection

Logging was added using `Microsoft.Extensions.Logging.Abstractions` to give visibility into the calculator's behavior in production. Using `NullLogger` as the default means the library works out of the box without any logging configuration required.

Dependency Injection support was added via a `ServiceCollectionExtensions` class, allowing the library to be registered with a single line in any .NET application:

```csharp
services.AddTollCalculator();
```

This makes the library easy to integrate into ASP.NET or any other DI-based application, while still allowing manual instantiation for simpler use cases.

## My Thoughts

My first impression was that the code felt unstructured and simple. There were several bugs and the solutions to core problems like hour-window grouping and holiday handling were poorly implemented.

The models had clear shortcomings. A solid model layer is the foundation of a good library, and the original relied on magic strings instead of enums, making it fragile and hard to extend. Getting the models right was the first priority before touching any business logic.

Code documentation is also important — not just for explaining what the code does, but to make it easier for other developers to understand and maintain. The original had no documentation at all, which is a problem for any solution that is claimed to be production-ready.