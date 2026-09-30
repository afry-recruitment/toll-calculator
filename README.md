# Toll calculator — C#

A small .NET 10 library that calculates a vehicle's toll fee for one day. It fixes
incorrect hourly grouping and tariff gaps in the original implementation, validates
inputs, and makes the supported calendar explicit.

The [original AFRY assignment](docs/assignment.md) is preserved. The C# implementation
is the submission; the Java starter is unchanged.

## Run

Install the .NET 10 SDK, then run from the repository root:

```sh
dotnet restore TollCalculator.sln --locked-mode
dotnet build TollCalculator.sln --configuration Release --no-restore
dotnet test TollCalculator.sln --configuration Release --no-build
```

No database, credentials, external service or running web server is required.
`global.json` selects a stable .NET 10 SDK. Tests use xUnit; the library has no
third-party runtime dependencies. Test dependencies are locked in `packages.lock.json`.

## Example

```csharp
using TollFeeCalculator;

var calculator = new TollCalculator();
var car = new Vehicle(VehicleType.Car);
var passages = new[]
{
    new DateTime(2013, 1, 2, 6, 0, 0),  // 8 SEK
    new DateTime(2013, 1, 2, 6, 45, 0), // 13 SEK, same window
    new DateTime(2013, 1, 2, 7, 15, 0)  // 18 SEK, new window
};

int fee = calculator.GetTollFee(car, passages); // 13 + 18 = 31 SEK
```

For a single passage, `calculator.GetTollFee(passages[0], car)` returns its individual
fee. Both entry points use the same tariff, vehicle exemptions and calendar.

## Rules and assumptions

The draft specifies hourly maximums, a 60 SEK daily cap, and exemptions, but does
not fully define tariff times or the holiday calendar. The implementation uses the
original schedule and the following explicit interpretations:

* Each fixed 60-minute window starts at its first passage. Exactly 60 minutes is
  included; one tick later starts another window. Later passages do not move the
  start or extend the window.
* Charge only the highest passage fee in each window, then sum window fees and
  cap the day at 60 SEK. There is no extra charge for opening a window.
* Zero-fee passages also start windows, preserving the apparent intent of the
  original code. This and the exact 60-minute boundary need business confirmation.
* Motorbike, tractor, emergency, diplomat, foreign and military vehicles are
  exempt, matching the original categories. Cars are chargeable.
* Weekends are free. The default calendar preserves all supplied 2013 exemptions,
  including all of July and the listed dates around holidays. These are assignment
  rules, not a claim about current legislation in a particular city.

| From (inclusive) | Until (exclusive) | Fee (SEK) |
|---|---|---:|
| 00:00 | 06:00 | 0 |
| 06:00 | 06:30 | 8 |
| 06:30 | 07:00 | 13 |
| 07:00 | 08:00 | 18 |
| 08:00 | 08:30 | 13 |
| 08:30 | 15:00 | 8 |
| 15:00 | 15:30 | 13 |
| 15:30 | 17:00 | 18 |
| 17:00 | 18:00 | 13 |
| 18:00 | 18:30 | 8 |
| 18:30 | Midnight | 0 |

### Calendar coverage

The default calculator supports **2013 only**. An unsupported year throws instead
of silently charging on a holiday for which no rules were supplied.

`TollCalendar` accepts a year and that year's complete set of special exemption
dates. Weekends are always handled automatically. Other exemptions, including
whole-month exemptions, must be included in the supplied dates. For another year,
pass its approved data to `new TollCalendar(year, approvedExemptionDates)` and
supply that calendar to `new TollCalculator(calendar)`. The caller owns the
completeness of that data; constructor validation can only check its year.

The collection is copied, and calendar state cannot be changed by callers after
construction. One calculator uses one calendar year; create another instance for
another year. There is no guessed recurring-holiday algorithm or network lookup.

### Input contract

| Input | Behavior |
|---|---|
| Null vehicle or passage array | `ArgumentNullException` |
| Empty array with a valid vehicle | 0 SEK |
| Undefined `VehicleType` enum value | `ArgumentOutOfRangeException` at vehicle construction |
| Multiple calendar dates | `ArgumentException`; callers calculate each day separately |
| Year not covered by the calendar | `ArgumentOutOfRangeException` |
| Unordered or duplicate passages | Accepted; a sorted copy is used |

Passages must be supplied as **city-local dates and times**. The calculator uses
these values as supplied, without checking `DateTime.Kind` or converting time zones.
The caller is responsible for converting any UTC timestamps to the city's local
time before calling the calculator. Window lengths are measured in local wall-clock time.

Validation happens before exemptions, so invalid input is rejected consistently
for exempt vehicles too. The caller's passage array is never changed.

## Design decisions

| Decision | Reason |
|---|---|
| Small class library | The assignment is a calculation problem; no transport or persistence requirements were specified. |
| One aggregation loop | Tracks window start, current maximum and completed-window total explicitly. |
| Separate tariff and calendar | Pricing boundaries and calendar data can be reviewed and tested independently from grouping. |
| Enum and immutable vehicle | Removes spelling/casing bugs without a hierarchy of otherwise identical vehicle classes. |
| Explicit calendar supplied through the constructor | New approved calendar data does not require changing the aggregation algorithm. No DI framework is needed. |
| Whole-number SEK | All supplied fees are whole kronor, so floating point and rounding are unnecessary. |
| Input validation | Invalid or unsupported input produces a visible failure rather than a plausible but incorrect charge. |

Sorting costs O(n log n), followed by an O(n) scan, using O(n) space for the copied
array. Inputs are restricted to one day, so the number of chargeable windows is
bounded. The calculator keeps no mutable per-call state and can be reused across calls.

There is no API, EF Core/PostgreSQL storage, CQRS or MediatR: none is needed to
calculate these fees. Those can be added by a consuming application if it needs
passage ingestion, persistence, authentication or billing.

The public vehicle model deliberately changed from a string-returning interface
and `Car`/`Motorbike` classes to `new Vehicle(VehicleType.Car)` and
`new Vehicle(VehicleType.Motorbike)`. `TollCalculator` now shares the
`TollFeeCalculator` namespace with the other library types. Existing callers would
need these small source changes; the two calculation method signatures remain.

## Verification and maintenance

The tests cover every tariff transition immediately before and at the boundary,
window maximums, fixed-window behavior, exact elapsed-time boundaries, multiple
windows, the daily cap, input ordering and duplicates, all exemption types, the
supplied calendar dates, custom calendars, and validation failures.

```sh
dotnet format TollCalculator.sln --verify-no-changes
dotnet list TollCalculator.sln package --vulnerable --include-transitive
```

GitHub Actions builds and tests on Linux and Windows, and checks formatting on
Linux. The workflow uses read-only repository permissions and actions pinned to
commit hashes. A clean local run verifies the code; the hosted workflow will run when the branch is pushed.

Before real deployment, the requirements owner needs to approve the calendar,
tariff, exemption categories and window interpretations for the target city/year.
The consuming application also owns time-zone conversion and error handling.

The original [license](C%23/LICENSE) is retained.
