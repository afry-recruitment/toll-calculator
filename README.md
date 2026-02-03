# Afry.TollCalculator usage notes

`Afry.TollCalculator` is primarily a **DLL/library** project. The repository also includes a small **Minimal API** (`Afry.TollCalculator.SampleApi`) to quickly try the library end-to-end, plus a dedicated **test project** (`Afry.TollCalculator.Tests`) that exercises the core behaviors.

## API documentation (XML summary tags)
The library uses XML documentation comments (extensive `/// <summary>` tags and related XML doc tags) so consumers get helpful IntelliSense and can generate API documentation as part of their build/deployment pipeline.

## Money representation
`Money` is intended to be a reliable way of tracking monetary values.

- Amounts are stored in **minor currency units** (for example, öre for SEK).
- This avoids floating-point rounding issues that can occur with `double`/`decimal` when doing repeated calculations.
- Currency is stored alongside the amount and basic operators validate that currencies match.

## Result pattern (errors without exceptions)
The library favors returning `Result<T>` values to represent success/failure instead of throwing exceptions for expected error cases.

- This keeps API behavior explicit for callers (success value vs. error message).
- Exceptions are still appropriate for truly exceptional situations (for example, misconfiguration detected during startup validation).

## Date/time handling and localization
The library uses `DateTimeOffset` for timestamps.

- `DateTimeOffset` preserves the time *and* the offset, making it safer for APIs than `DateTime`.
- The library treats the provided timestamp as authoritative and does not enforce a specific culture/time zone.

The **calling application** should enforce localization (culture, UI culture, and request localization) appropriate for its environment.

- `Afry.TollCalculator.SampleApi` shows one way to do this by enforcing a configurable culture (defaulting to `sv-SE`).

## Configuration-first behavior
All toll-calculation domain rules are stored in configuration (`TollSettings`) to keep the library flexible:

- Currency
- Min/max fee bounds
- Toll-free vehicles
- Toll-free weekdays
- Toll-free hours
- Toll-free days
- Fee schedule

This makes it possible to adjust behavior without code changes.

## Vehicle model
`Vehicle` is intentionally simple at the moment and primarily wraps a `VehicleType` enum.

If vehicle types gain additional behavior/data beyond an enum value, the model can be extended to a polymorphic design (for example, an abstract `Vehicle` base type with derived types like `Car`, `Motorbike`, `Emergency`, etc.).

## Registering and using the calculator
Register services via the extension method `AddTollCalculator`.

Example (ASP.NET / generic host):

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTollCalculator(builder.Configuration);

var app = builder.Build();
app.Run();
```

### Settings validation on startup
`AddTollCalculator` wires options validation and calls `ValidateOnStart()`, which means:

- The application fails fast during startup if settings are invalid (for example, invalid weekday names like `"Sunda"`).
- This helps prevent errors from surfacing later at runtime on the first request.

## How fee calculation works (high level)
The toll fee calculation is performed in a few steps:

1) **Validate input**: a vehicle must be provided and at least one pass timestamp must exist.
2) **Group by local date**: all passes must fall on the same *local* day (based on `DateTimeOffset.LocalDateTime`).
3) **Compute fee for each pass**:
   - If the vehicle is toll-free, the day is toll-free, or the time-of-day is within configured toll-free hours, the fee is `0`.
   - Otherwise, the time-of-day is matched against the configured fee schedule to get the fee.
4) **Apply the 60-minute rule**: passes within 60 minutes are treated as a single interval where only the highest fee in that interval is charged.
5) **Apply daily cap**: the total is capped at `maxDailyFeeMinorUnit`.

## Toll-free hours, toll-free vehicles, and alternative approaches
Toll-free hours and toll-free vehicles are configured through `TollSettings`.

Toll-free dates are also configured (as explicit dates) for flexibility. Another possible approach would be a mixed strategy:

- Keep a set of *fixed* holidays hardcoded (for example, Jan 1st)
- Derive movable holidays from an Easter calculation algorithm

The project deliberately uses the **settings-driven approach** instead, so changes to holidays and exemptions can be done by editing configuration rather than shipping a new build.

P.S. The movie that the GIF comes from is called "Hackers".
