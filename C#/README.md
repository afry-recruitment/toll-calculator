# Toll Calculator (C#)

A small library that computes toll fees for vehicle passes during a single day,
following the requirements described in the [root README](../README.md).

## Layout

```
C#/
├── TollCalculator.sln
├── TollCalculator/                 # Class library
│   ├── TollCalculator.csproj
│   ├── TollCalculator.cs           # Fee schedule, hourly windowing, daily cap
│   ├── Vehicle.cs                  # Vehicle interface
│   ├── Car.cs                      # Chargeable vehicle
│   ├── Motorbike.cs                # Fee-free vehicle
│   └── TollFreeVehicle.cs          # Generic fee-free vehicle (Tractor, Emergency, ...)
└── TollCalculator.Tests/           # xUnit tests covering every requirement
    ├── TollCalculator.Tests.csproj
    └── TollCalculatorTests.cs
```

## Build and test

The library targets `net7.0`.

```bash
dotnet build C#/TollCalculator.sln
dotnet test  C#/TollCalculator.sln
```

## Fee schedule

| Time window  | Fee (SEK) |
|--------------|-----------|
| 06:00–06:29  | 8         |
| 06:30–06:59  | 13        |
| 07:00–07:59  | 18 (rush) |
| 08:00–08:29  | 13        |
| 08:30–14:59  | 8         |
| 15:00–15:29  | 13        |
| 15:30–16:59  | 18 (rush) |
| 17:00–17:59  | 13        |
| 18:00–18:29  | 8         |
| All other    | 0         |

The schedule lives as a small data table in
[`TollCalculator.cs`](TollCalculator/TollCalculator.cs), so adjusting fees only
requires editing one place.

## Behaviour summary

- Passes within a rolling 60-minute window are charged once at the highest
  applicable fee. The window restarts on the first pass that occurs more than
  60 minutes after the previous window's start.
- The daily total is capped at 60 SEK.
- Saturdays and Sundays are free.
- Toll-free vehicle types (`Motorbike`, `Tractor`, `Emergency`, `Diplomat`,
  `Foreign`, `Military`) are always free. Vehicle-type comparison is
  case-insensitive.
- Null or empty input returns 0.
- Inputs do not need to be pre-sorted; the calculator normalises order
  internally.

## Notable fixes over the inherited code

- The hourly window used `DateTime.Millisecond` (the 0–999 sub-second
  component) instead of elapsed time, which made nearly all passes appear to
  fall in the same hour.
- The interval anchor (`intervalStart`) was never advanced, so once a single
  hour had elapsed every later pass was charged in full with no
  deduplication.
- The fee schedule had gaps between 09:00 and 14:29 that returned 0.
- An operator-precedence bug made the 15:00–15:29 band unreachable.
- Null and empty inputs threw rather than returning 0.

These are all covered by tests in
[`TollCalculatorTests.cs`](TollCalculator.Tests/TollCalculatorTests.cs).

## Known limitations and suggested next steps

- The holiday calendar is hardcoded to Swedish public holidays for 2013 (the
  kata fixture). A production implementation should source holidays from a
  configurable provider that covers other years.
- The library assumes all passes belong to a single day. Callers should split
  multi-day data per day before invoking it.
- Fees and the daily cap are simplified for the kata (8 / 13 / 18 / 60 SEK)
  rather than the current Stockholm congestion-tax schedule.
