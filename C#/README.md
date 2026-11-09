The gif was from the movie Hackers from 1995: 

![alt text](image.png)

# Toll fee calculator — C#

A small, commented C# implementation of the AFRY exercise, informed by a review
of all **96 open pull requests** in the captured repository snapshot.
The core has no external packages. It separates the tariff, calendar and hourly
calculation, and includes a runnable console program and xUnit tests.

## Run it

Install the **.NET 8 SDK**, extract the ZIP, and open a terminal in the
`TollFeeSolution` directory:

```sh
dotnet run --project src/TollFeeDemo
```

This runs ten examples with an explanation of each result. To try your own times:

```sh
dotnet run --project src/TollFeeDemo -- Car 2013-02-04 05:59 06:00 07:00
```

Result: **18 SEK**. Vehicle names are case-insensitive in the console. Times accept
`HH:mm`, `HH:mm:ss`, or seconds with up to seven fractional digits. Use `--help`
after the `--` separator for help. Invalid input prints an explanation and exits
with code 2.

In Visual Studio, open `TollFeeCalculator.sln`, set **TollFeeDemo** as the startup
project, then press **Ctrl+F5**. `Program.cs` is the console entry point and uses
the included library; keep the complete solution together.

**The bundled calendar covers 2013.** This deliberately preserves the exercise's
specified dates and July exemption. It is not a current Swedish tax calculator.

## Start reading here

| File | Responsibility |
| --- | --- |
| [Program.cs](src/TollFeeDemo/Program.cs) | Ten examples, custom arguments and readable output |
| [TollCalculator.cs](src/TollFeeCalculator/TollCalculator.cs) | Validate, sort a copy, charge each window's maximum, apply the daily cap |
| [TollTariff.cs](src/TollFeeCalculator/TollTariff.cs) | Nine time-of-day price bands using `TimeOnly` |
| [VehicleType.cs](src/TollFeeCalculator/VehicleType.cs) | Recognized vehicle categories |
| [ConfiguredTollCalendar.cs](src/TollFeeCalculator/ConfiguredTollCalendar.cs) | Weekends, July, configured holidays and coverage checks |
| [ITollCalendar.cs](src/TollFeeCalculator/ITollCalendar.cs) | The extension point for a different exemption calendar |

The hourly loop keeps just three pieces of state: the start of the active window,
its highest fee, and the total for completed windows. A passage outside the active
window closes it and starts a new one. The last window is added once after the
loop. The input is copied and sorted, so callers can reuse their original data.

This takes O(n log n) time and O(n) memory for n passages. Calculation state is
local to each call; the default calendar can safely be shared between calls.

## Use the library

```csharp
using TollFeeCalculator;

var calculator = new TollCalculator();

int total = calculator.CalculateDailyFee(VehicleType.Car, new[]
{
    new DateTime(2013, 2, 4, 6, 0, 0),  // First window: initially 8 SEK.
    new DateTime(2013, 2, 4, 6, 30, 0), // Same window: maximum becomes 13.
    new DateTime(2013, 2, 4, 7, 0, 0),  // Exactly 60 minutes: maximum becomes 18.
    new DateTime(2013, 2, 4, 15, 30, 0) // Second window: another 18.
});
// total == 36

int standaloneFee = calculator.GetPassageFee(
    VehicleType.Car, new DateTime(2013, 2, 4, 7, 0, 0));
// standaloneFee == 18, before hourly grouping and the daily cap.
```

`CalculateDailyFee` accepts any `IEnumerable<DateTime>` and enumerates it once.
Use the complete day's passages for one vehicle. For several days, group by local
date and call it once per group. Separate partial-day calculations must not be
added together: their hourly windows and caps could overlap.

## Explicit rule choices

The assignment leaves some details open. These are the rules implemented and tested:

| Topic | Decision |
| --- | --- |
| Money | Whole SEK, stored as integers |
| Maximum | 60 SEK per vehicle per local calendar day |
| Hourly window | Fixed, anchored to the first chargeable passage |
| Within a window | Charge only the highest passage fee |
| Exactly 60 minutes | Included; one tick later starts a new window |
| Later passages | Never move or extend the current window |
| Free passages | Do not start, move, reset or extend a window |
| Ordering and duplicates | Accepted; do not change the result |
| Empty input | Zero for a valid vehicle category |
| Exempt vehicles | Motorbike, Tractor, Emergency, Diplomat, Foreign, Military |
| Other vehicles | `Car` and `Other` are chargeable; `Unknown` and invalid enum values are rejected |
| Calendar | Weekends, all of July, and the exercise's explicit 2013 holidays/eves |
| Unsupported year | Rejected until a calendar covering it is provided |
| Timestamps | City-local wall-clock `DateTime` with `Kind == Unspecified` |

Two examples settle the window interpretation:

- **06:00, 06:50, 07:40 → 31 SEK.** The first window costs 13; the second costs 18.
  Raising the fee at 06:50 does not extend the first window.
- **05:59, 06:00, 07:00 → 18 SEK.** The free 05:59 passage is ignored, so the
  window begins at 06:00. Removing that free passage leaves the result unchanged.

All input timestamps are validated before returning an exempt result. A motorbike
still cannot be submitted with mixed days, an unsupported year, or UTC timestamps.

## Tariff

Every range includes its start and excludes its end. For example, `06:29:59.9999999`
still costs 8 SEK, and `06:30:00` costs 13. Times outside these ranges cost zero.

| Start (inclusive) | End (exclusive) | SEK |
| --- | --- | ---: |
| 06:00 | 06:30 | 8 |
| 06:30 | 07:00 | 13 |
| 07:00 | 08:00 | 18 |
| 08:00 | 08:30 | 13 |
| 08:30 | 15:00 | 8 |
| 15:00 | 15:30 | 13 |
| 15:30 | 17:00 | 18 |
| 17:00 | 18:00 | 13 |
| 18:00 | 18:30 | 8 |

## Calendar and time zones

To support a different year, provide that year's complete approved exemption
list, including exempt holiday eves:

```csharp
var calendar = new ConfiguredTollCalendar(year, approvedExemptDates);
var calculator = new TollCalculator(calendar);
```

The calendar copies the list and rejects dates belonging to another year.
Weekends and July are handled separately; set `exemptJuly: false` for a policy
without that exemption. List completeness remains the caller's responsibility.
`ITollCalendar` permits another immutable implementation, including a multi-year
calendar, without changing the charging algorithm.

For real event timestamps, keep `DateTimeOffset` at the application boundary and
convert using the city's explicitly chosen `TimeZoneInfo`. Then pass the resulting
local `.DateTime`. Simply changing UTC's `Kind` to `Unspecified` does not convert it.
The core measures wall-clock elapsed time; a policy that charges during daylight-
saving clock changes would need its elapsed-time semantics defined separately.

## Build and test

```sh
dotnet restore TollFeeCalculator.sln
dotnet build TollFeeCalculator.sln --configuration Release --no-restore
dotnet test TollFeeCalculator.sln --configuration Release --no-build
```

NuGet access is required to restore the test packages. The library and console
have no external runtime dependencies.

Tests cover tariff boundaries to one tick, every minute of the day, all 365 dates
in 2013, window anchoring and finalization, free passages, duplicate/order
invariance, the daily cap, invalid input, calendar copying and lazy enumeration.
[VERIFICATION.md](VERIFICATION.md) records the results and the execution
environment's standard-runner limitation.

## Review and changes from the earlier version

[PR_REVIEW.md](PR_REVIEW.md) has an observation for every reviewed PR, the exact
head revisions, adopted ideas, and the review's limits. The result combines
small rule-focused types, clear contracts, a half-open tariff table, a single
window loop, useful console examples, and defensive tests.

This version intentionally replaces the earlier string-based `Vehicle` interface
and `Car`/`Motorbike` classes with `VehicleType`. Migrate callers as follows:

| Earlier API | New API |
| --- | --- |
| `GetTollFee(new Car(), dates)` | `CalculateDailyFee(VehicleType.Car, dates)` |
| `GetTollFee(date, new Motorbike())` | `GetPassageFee(VehicleType.Motorbike, date)` |
| Unlisted nonempty vehicle name | Map explicitly to `VehicleType.Other` if it is known to be chargeable |

The [original assignment](ASSIGNMENT.md) and [GPLv3 license](LICENSE) are retained.
The project is a source-code solution for review; no PR has been submitted.
