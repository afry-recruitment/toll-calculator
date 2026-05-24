/**
 * Difference between two moments, in whole-or-fractional minutes.
 *
 * The arithmetic is on UTC milliseconds, so the result is independent of
 * any time zone and unaffected by DST transitions — exactly what the
 * 60-minute-window rule needs. A pass at 02:30 CET and a pass at 03:30
 * CEST on a spring-forward Sunday are *one hour apart* in UTC even
 * though the wall clock advanced two hours; this function returns 60.
 *
 * We use raw `Date.getTime()` rather than `Temporal.Duration` because the
 * computation is a single subtraction; reaching for Temporal here would
 * be ceremony without benefit. The DST-correctness story lives in
 * `zoned-time.ts`, not here.
 *
 * Killing bug `[D§4.1]`: the C# reference computed minutes by subtracting
 * `DateTime.Millisecond` (the ms-within-second component) rather than the
 * raw tick value. `getTime()` returns the raw ms since epoch — no trap.
 */
export const minutesBetween = (earlier: Date, later: Date): number =>
  (later.getTime() - earlier.getTime()) / 60_000;
