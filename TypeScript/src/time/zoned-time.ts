import { Temporal } from "@js-temporal/polyfill";

/**
 * Wall-clock parts of a moment in time, interpreted in a specific IANA
 * time zone. `month` is 1-12 (calendar convention, not the JS `Date`
 * 0-indexed convention), and `weekday` is ISO 1=Monday … 7=Sunday so
 * that weekend detection is a single `weekday >= 6` comparison.
 */
export interface ZonedParts {
  readonly year: number;
  readonly month: number;
  readonly day: number;
  readonly hour: number;
  readonly minute: number;
  readonly weekday: number;
}

/**
 * Returns the wall-clock parts of `date` as interpreted in `timeZone`.
 *
 * Implementation uses the TC39 Temporal API via `@js-temporal/polyfill`.
 * When Temporal ships natively in V8 / Node, the polyfill import becomes
 * the only line that needs to change — the call sites are spec-final.
 *
 * Why a library instead of `Intl.DateTimeFormat`: `Date` has no native
 * way to ask "what wall-clock does this instant produce in city X?";
 * `Intl.DateTimeFormat` only solves the string-formatting half of that
 * problem and leaves zone arithmetic (DST ambiguity, weekday derivation,
 * locale-sensitive parsing) to the caller. Temporal exposes the
 * arithmetic directly. See `docs/plan/02-tech-stack.md` §2.5 for the
 * alternatives evaluated.
 *
 * Killing bug `[D§4.1]`: the C# reference subtracts `DateTime.Millisecond`
 * and ignores zones entirely. Here every consumer must pass an explicit
 * IANA zone — there is no implicit "local time".
 *
 * @throws RangeError if `timeZone` is not a valid IANA zone identifier.
 */
export const parts = (date: Date, timeZone: string): ZonedParts => {
  const zonedDateTime = Temporal.Instant.fromEpochMilliseconds(
    date.getTime(),
  ).toZonedDateTimeISO(timeZone);

  return {
    year: zonedDateTime.year,
    month: zonedDateTime.month,
    day: zonedDateTime.day,
    hour: zonedDateTime.hour,
    minute: zonedDateTime.minute,
    weekday: zonedDateTime.dayOfWeek,
  };
};
