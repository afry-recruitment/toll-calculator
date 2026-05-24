import type { TollCalculatorPreset } from "../calculator.js";
import type { FeeSchedule } from "../fee-schedule.js";
import { createSwedishCongestionTaxCalendar } from "../calendar/swedish.js";

/**
 * Fee schedule derived from the reference Java / C# implementation shipped
 * in this repository. Amounts are in SEK (the only currency mentioned in
 * the assignment README).
 *
 * Wall-clock band boundaries are interpreted in the time zone supplied via
 * {@link REFERENCE_PRESET}.timeZone.
 *
 * Differences from the reference Java code:
 * - The 08:30–14:59 band correctly covers every minute (the reference
 *   only covered minutes 30–59 of hours 9–14, leaving 09:00–14:29
 *   accidentally free — see `docs/discovery/04-bugs.md` §4.3).
 * - The 15:30–16:59 band is expressed cleanly as a single range (the
 *   reference used a confusing `||` with a precedence bug).
 */
export const REFERENCE_FEE_SCHEDULE: FeeSchedule = [
  { from: "06:00", to: "06:29", fee: 8 },
  { from: "06:30", to: "06:59", fee: 13 },
  { from: "07:00", to: "07:59", fee: 18 },
  { from: "08:00", to: "08:29", fee: 13 },
  { from: "08:30", to: "14:59", fee: 8 },
  { from: "15:00", to: "15:29", fee: 13 },
  { from: "15:30", to: "16:59", fee: 18 },
  { from: "17:00", to: "17:59", fee: 13 },
  { from: "18:00", to: "18:29", fee: 8 },
];

/**
 * Default preset bundling the schedule, calendar, daily cap and time zone
 * derived from primary Swedish legislation:
 *
 * - **Schedule + cap** mirror the bundled Java / C# reference (SEK
 *   amounts, 60 SEK daily cap).
 * - **Calendar** is computed from SFS 2004:629 (Lag om trängselskatt)
 *   Bilaga 1 and SFS 1989:253 (Lag om allmänna helgdagar) — see
 *   `src/calendar/swedish.ts` for the citation block and algorithm.
 * - **Time zone** `Europe/Stockholm` is a data property of the preset
 *   because the schedule's wall-clock semantics were defined for that
 *   zone. Consumers using a different rate table override `timeZone`
 *   (and the other fields) by spreading this preset into their options.
 *
 * @example
 * import { createTollCalculator, REFERENCE_PRESET } from "toll-calculator";
 * const calc = createTollCalculator(REFERENCE_PRESET);
 *
 * @example
 * // Custom city — keep the calendar, override schedule and zone:
 * const calc = createTollCalculator({
 *   ...REFERENCE_PRESET,
 *   schedule: MY_CITY_SCHEDULE,
 *   timeZone: "Europe/Oslo",
 * });
 */
export const REFERENCE_PRESET: TollCalculatorPreset = {
  name: "reference",
  schedule: REFERENCE_FEE_SCHEDULE,
  calendar: createSwedishCongestionTaxCalendar(),
  dailyCap: 60,
  timeZone: "Europe/Stockholm",
};
