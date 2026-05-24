# 6. Reimplementation recommendations

The reference code is not worth patching. A clean rewrite is faster and
shows the design intent more clearly. The recommendations below are
language-agnostic.

## 6.1 Domain model

```
Pass        { vehicle, timestamp }
Vehicle     { type, isTollFree }
FeeSchedule { ranges: [(from, to, fee)] }
HolidayCalendar { isHoliday(date) }
TollCalculator(schedule, calendar, dailyCap)
    .feeFor(vehicle, passes: List[DateTime]) -> int
```

- Make "toll-free" a **property of the vehicle**, not a string lookup. Bug
  §4.7 disappears.
- Inject the holiday calendar. Bug §4.6 disappears, and the system becomes
  testable for any year.
- Inject the fee schedule. The fee table moves from `if`/`else` to data; bug
  §4.3 disappears and the code becomes table-driven.

## 6.2 Algorithm (correct version of the windowing logic)

```
fee_for(vehicle, passes):
    if vehicle.is_toll_free: return 0
    if not passes: return 0

    passes = sorted(passes)
    total = 0
    window_start = passes[0]
    window_max   = fee_at(window_start)

    for p in passes[1:]:
        if (p - window_start) <= 1 hour:
            window_max = max(window_max, fee_at(p))
        else:
            total += window_max
            window_start = p
            window_max   = fee_at(p)

    total += window_max
    return min(total, DAILY_CAP)
```

This handles bugs §4.2 (window restart), §4.5 (no negative totals), §4.4
(empty input), and the daily cap in a single pass.

## 6.3 Time handling

- **Java:** use `java.time.LocalDateTime` / `ZonedDateTime` with
  `ZoneId.of("Europe/Stockholm")` and `Duration.between(a, b).toMinutes()`.
- **C#:** use `DateTimeOffset` and `TimeZoneInfo.FindSystemTimeZoneById(...)`
  or NodaTime.
- Never subtract `DateTime.Millisecond` — see bug §4.1.

## 6.4 Holiday calendar

The reference enumerates a flat list of dates for year 2013. To
generalize, the rewrite needs:
- A `HolidayCalendar` abstraction injected into the calculator, so any
  year and any jurisdiction can be plugged in.
- A computed Easter date (the **Anonymous Gregorian algorithm** is the
  standard choice), since most movable Swedish holidays derive from it.
- A concrete calendar implementation that reproduces the toll-free
  days the reference encodes for 2013 and extends to other years —
  the actual rule set is a separate investigation handled in the plan.

## 6.5 Tests to include

At minimum, parametrized tests covering:

1. Single pass at each fee tier (8, 13, 18, free).
2. Multiple passes within the same hour — highest applies.
3. Multiple passes across hour boundaries — each charged separately.
4. Daily total exceeding 60 SEK — capped at 60.
5. Toll-free vehicle on a fee-time pass.
6. Toll-free date (Saturday, Sunday, holiday).
7. Empty pass list.
8. Unordered pass list.
9. Boundary minutes: 06:00, 06:29, 06:30, 18:29, 18:30.
10. DST transition day (Europe/Stockholm flips in late March / late October).

## 6.6 Recommended language choice

Per the README, the language is open. Practical picks:

- **Kotlin / Python:** fastest to write, easiest test ergonomics.
- **Go:** tidy, no dependencies, builtin testing.
- **TypeScript:** good for showing typed domain modelling.

Whichever you pick, keep the public API to ~3 functions and let the tests be
the documentation.