# 5. Code smells and minor issues

These are not functional bugs, but they harm readability, extensibility, or
correctness across edge cases.

## Java

- **Legacy date API.** Uses `java.util.Date` + `Calendar` instead of
  `java.time.*` (which has been the recommended API since Java 8).
- **Useless import.** `java.util.concurrent.*` is imported only for
  `TimeUnit`, which is itself unnecessary — `Duration.between(...)` would be
  cleaner.
- **Boxed `Boolean` return.** `private Boolean isTollFreeDate(...)` returns
  the wrapper instead of the primitive `boolean` — risks accidental NPE.
- **No package declaration.** Files live in the default package.
- **Mutable `Date`.** `java.util.Date` is mutable, which makes the
  `Date... dates` varargs caller-vulnerable.

## C#

- **No timezone awareness.** Uses `DateTime` without zone info; any
  rush-hour-based toll system needs explicit time-zone handling so that
  fees are computed against wall-clock times in the operator's zone and
  DST transitions are not silently mis-billed — `DateTimeOffset` or
  explicit `TimeZoneInfo` use is required.
- **Namespace coupling is fragile.** `Vehicle.cs` declares
  `namespace TollFeeCalculator`, but `TollCalculator.cs` is **outside** any
  namespace and imports it via `using`. Inconsistent.
- **Useless `using` directives.** `Car.cs` and `Vehicle.cs` import
  `System.Linq`, `System.Threading.Tasks`, etc. — nothing is used.
- **Enum used for string compare.** `TollFreeVehicles` is an `enum` whose
  string name is compared via `.ToString()` — same stringly-typed problem as
  the Java version.

## Both

- **No tests.** A single happy-path unit test would have caught most of the
  bugs in [04-bugs.md](04-bugs.md).
- **No `.gitignore`.** Build artifacts would be versioned by accident.
- **Public single-pass overload.** See bug §4.8.
- **No input validation.** `null`, empty list, unsorted list — all undefined
  behavior.
- **Stringly-typed vehicle exemption.** See bug §4.7.
- **Hard-coded constants.** Fees (8, 13, 18), cap (60), the 2013 holiday set
  are all magic numbers inlined in the calculator — no configuration
  boundary.
- **Calculator owns too much.** `TollCalculator` is both the fee table, the
  holiday calendar, and the windowing logic. These should be split.
- **Inconsistent indentation.** Two spaces in Java, four in C# (acceptable
  per-language, but no `.editorconfig`).
- **README mixes languages and tone.** Includes a giphy link and an ironic
  "production-ready" claim — intentional for the exercise, but not something
  to mirror in the deliverable.
