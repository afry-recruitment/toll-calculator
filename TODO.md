TODO - Toll Calculator Review (Java + C#)

General
- Validate input dates: guard against null/empty arrays and enforce single-day input; otherwise the daily max and holiday checks are undefined.
- Sort dates before calculation so the "one hour rule" is deterministic; current logic assumes ordered input.
- Reset the 60-minute interval when the gap is > 60 minutes; otherwise later passes are still compared to the first pass of the day.
- Move fee schedule and holiday rules into data/config to avoid hard-coded magic values.
- Replace the 2013-only holiday list with a reusable calendar per year (or injected holiday service).

Java (TollCalculator.java)
- Fix 8:30-14:59 rule: current condition only charges on minutes 30-59 for hours 8-14, leaving 9:00-9:29, 10:00-10:29, etc. as free.
- Make time-band logic explicit with inclusive ranges and parentheses to avoid precedence bugs (e.g., 15:30-16:59 rule).
- Update intervalStart when a new 60-minute window begins to correctly enforce "only charged once per hour."
- Consider using java.time (LocalDateTime/LocalTime) for clearer time math and avoiding mutable Date/Calendar.

C# (TollCalculator.cs)
- Fix time-diff calculation: `date.Millisecond - intervalStart.Millisecond` only compares the millisecond component; use `(date - intervalStart).TotalMinutes`.
- Fix 8:30-14:59 rule: same bug as Java.
- Make time-band logic explicit with parentheses (same as Java) to prevent precedence mistakes.
- Update intervalStart when a new 60-minute window begins, same as Java.
- Consider using `DateTimeOffset` or `TimeSpan` and avoid culture-dependent assumptions.
