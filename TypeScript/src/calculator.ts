import type { Vehicle } from "./vehicle.js";
import type { FeeSchedule } from "./fee-schedule.js";
import type { HolidayCalendar } from "./calendar/index.js";
import { isTollFreeVehicle } from "./vehicle.js";
import { feeAt } from "./fee-schedule.js";
import { parts } from "./time/zoned-time.js";
import { assertVehicle, assertPasses } from "./validation.js";

/**
 * Configuration for {@link createTollCalculator}. Every field is required —
 * the library is data-driven and refuses to guess defaults. Use a preset
 * (e.g. `REFERENCE_PRESET`) and spread it to override only what differs.
 */
export interface TollCalculatorOptions {
  /** Fee bands; see {@link FeeSchedule}. */
  readonly schedule: FeeSchedule;
  /** Holiday source; see {@link HolidayCalendar}. */
  readonly calendar: HolidayCalendar;
  /** Maximum total fee per day, in the schedule's currency. */
  readonly dailyCap: number;
  /** IANA time zone (e.g. "Europe/Stockholm") in which the schedule's wall-clock times are interpreted. */
  readonly timeZone: string;
}

/**
 * A named, ready-to-use bundle of {@link TollCalculatorOptions}. Presets
 * are the suggested unit of distribution: each preset describes one
 * jurisdiction's complete configuration (rates, calendar, cap, time zone).
 *
 * To add a new jurisdiction, export a new preset — no library changes
 * required.
 */
export interface TollCalculatorPreset extends TollCalculatorOptions {
  /** Human-readable identifier, e.g. "reference". */
  readonly name: string;
}

/**
 * The public calculator contract.
 *
 * - An empty `passes` array returns `0`.
 * - Pass order is not significant; the calculator sorts internally and
 *   does not mutate the caller's array.
 * - Toll-free vehicles always return `0` regardless of passes.
 * - Holidays (per `calendar`) make every pass on that day fee-free.
 * - Within any 60-minute window, only the highest band fee counts.
 * - The total is clamped to `dailyCap`.
 */
export interface TollCalculator {
  readonly feeFor: (vehicle: Vehicle, passes: readonly Date[]) => number;
}

export const createTollCalculator = (
  options: TollCalculatorOptions,
): TollCalculator => {
  const feeForPass = (date: Date): number => {
    if (options.calendar.isHoliday(date)) return 0;
    const { hour, minute } = parts(date, options.timeZone);
    return feeAt(options.schedule, hour, minute);
  };

  const feeFor = (vehicle: Vehicle, passes: readonly Date[]): number => {
    // Boundary validation — see `src/validation.ts` for the rationale.
    // The TypeScript signature already forbids these shapes for in-process
    // callers, but TS guarantees evaporate when input crosses any non-TS
    // boundary (JSON.parse, ORM rows, plain-JS consumers). Validating
    // here once means internal helpers never see malformed input.
    assertVehicle(vehicle);
    assertPasses(passes);

    if (isTollFreeVehicle(vehicle)) return 0;

    const [first, ...rest] = [...passes].sort(
      (a, b) => a.getTime() - b.getTime(),
    );
    if (first === undefined) return 0;

    let total = 0;
    let windowStart = first;
    let windowMax = feeForPass(first);

    for (const pass of rest) {
      const minutesFromAnchor =
        (pass.getTime() - windowStart.getTime()) / 60_000;

      if (minutesFromAnchor <= 60) {
        windowMax = Math.max(windowMax, feeForPass(pass));
      } else {
        total += windowMax;
        windowStart = pass;
        windowMax = feeForPass(pass);
      }
    }
    total += windowMax;

    return Math.min(total, options.dailyCap);
  };

  return { feeFor };
};
