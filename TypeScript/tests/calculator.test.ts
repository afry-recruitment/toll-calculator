import { describe, it, expect } from "vitest";
import { createTollCalculator } from "../src/calculator.js";
import type {
  TollCalculator,
  TollCalculatorOptions,
} from "../src/calculator.js";
import type { Vehicle } from "../src/vehicle.js";
import type { FeeSchedule } from "../src/fee-schedule.js";
import type { HolidayCalendar } from "../src/calendar/index.js";

/**
 * Unit tests for `createTollCalculator` — exercise the windowing, cap,
 * sorting and vehicle/calendar short-circuits with a **mocked** schedule
 * and calendar. The point is that a failure here points at the
 * calculator's mechanics, not at the bundled reference data or the
 * Swedish calendar. End-to-end behavior through the real preset is
 * covered by `integration.test.ts`.
 */

const TEST_SCHEDULE: FeeSchedule = [
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

const NEVER_HOLIDAY: HolidayCalendar = { isHoliday: () => false };
const ALWAYS_HOLIDAY: HolidayCalendar = { isHoliday: () => true };

const flagging = (...flagged: Date[]): HolidayCalendar => ({
  isHoliday: (date) => flagged.some((d) => d.getTime() === date.getTime()),
});

const calculatorWith = (
  calendar: HolidayCalendar,
  overrides: Partial<TollCalculatorOptions> = {},
): TollCalculator =>
  createTollCalculator({
    schedule: TEST_SCHEDULE,
    calendar,
    dailyCap: 60,
    timeZone: "UTC",
    ...overrides,
  });

/** Build a UTC instant whose wall-clock hour/minute matches the string. */
const at = (hhmm: string): Date => new Date(`2025-01-15T${hhmm}:00Z`);

const car: Vehicle = { type: "Car" };

describe("createTollCalculator — table-driven scenarios", () => {
  it("row 1 — single rush-hour pass charges the band fee", () => {
    const calc = calculatorWith(NEVER_HOLIDAY);
    expect(calc.feeFor(car, [at("07:15")])).toBe(18);
  });

  it("row 2 — two passes in the same 60-min window charge the window max", () => {
    const calc = calculatorWith(NEVER_HOLIDAY);
    // 07:15 (18) + 07:45 (18) — same band, same window.
    expect(calc.feeFor(car, [at("07:15"), at("07:45")])).toBe(18);
  });

  it("row 3 — passes in different windows are both charged", () => {
    const calc = calculatorWith(NEVER_HOLIDAY);
    // 07:15 (band 07:00–07:59 = 18) + 08:25 (band 08:00–08:29 = 13).
    // 70 min apart → two separate windows.
    expect(calc.feeFor(car, [at("07:15"), at("08:25")])).toBe(31);
  });

  it("row 4 — four windows across the day sum to 57", () => {
    const calc = calculatorWith(NEVER_HOLIDAY);
    // 06:15 (8) | 07:30 (18) | 15:30 (18) | 17:30 (13).
    expect(
      calc.feeFor(car, [at("06:15"), at("07:30"), at("15:30"), at("17:30")]),
    ).toBe(57);
  });

  it("row 5 — total above 60 is clamped to dailyCap", () => {
    const calc = calculatorWith(NEVER_HOLIDAY);
    // Five separate windows at 18 = 90 uncapped → clamped to 60.
    expect(
      calc.feeFor(car, [
        at("07:00"),
        at("08:05"),
        at("15:30"),
        at("16:35"),
        at("17:40"),
      ]),
    ).toBe(60);
  });

  it("row 6 — toll-free vehicle always returns 0", () => {
    const calc = calculatorWith(NEVER_HOLIDAY);
    const motorbike: Vehicle = { type: "Motorbike" };
    // Same passes that would be 57 for a Car.
    expect(
      calc.feeFor(motorbike, [
        at("06:15"),
        at("07:30"),
        at("15:30"),
        at("17:30"),
      ]),
    ).toBe(0);
  });

  it("row 7 — every pass on a calendar-flagged day returns 0", () => {
    const calc = calculatorWith(ALWAYS_HOLIDAY);
    expect(calc.feeFor(car, [at("07:00"), at("08:00"), at("16:00")])).toBe(0);
  });

  it("row 8 — calendar can flag specific instants only (others bill normally)", () => {
    const flaggedPass = at("07:30");
    const calc = calculatorWith(flagging(flaggedPass));
    // 07:30 is flagged → 0 inside the window.
    // 08:00 (13) is in the same 60-min window as 07:30; windowMax = max(0, 13) = 13.
    expect(calc.feeFor(car, [flaggedPass, at("08:00")])).toBe(13);
  });

  it("row 9 — empty pass list returns 0 without throwing", () => {
    const calc = calculatorWith(NEVER_HOLIDAY);
    expect(calc.feeFor(car, [])).toBe(0);
  });

  it("row 10 — unsorted passes yield the same total as the sorted equivalent", () => {
    const calc = calculatorWith(NEVER_HOLIDAY);
    const sorted = [at("06:15"), at("07:30"), at("15:30"), at("17:30")];
    const shuffled = [at("17:30"), at("06:15"), at("15:30"), at("07:30")];
    expect(calc.feeFor(car, shuffled)).toBe(calc.feeFor(car, sorted));
  });

  it("row 10b — feeFor does NOT mutate the caller's array", () => {
    const calc = calculatorWith(NEVER_HOLIDAY);
    const passes = [at("17:30"), at("06:15"), at("15:30"), at("07:30")];
    const snapshot = [...passes];
    calc.feeFor(car, passes);
    expect(passes).toEqual(snapshot);
  });

  it("row 11 — passes entirely outside any band return 0", () => {
    const calc = calculatorWith(NEVER_HOLIDAY);
    // 01:00, 03:30, 05:00 — all before 06:00, no fee band covers them.
    expect(calc.feeFor(car, [at("01:00"), at("03:30"), at("05:00")])).toBe(0);
  });

  it("row 12 — boundary minutes match the schedule exactly", () => {
    const calc = calculatorWith(NEVER_HOLIDAY);
    expect(calc.feeFor(car, [at("06:00")])).toBe(8); // band 06:00–06:29 from
    expect(calc.feeFor(car, [at("06:29")])).toBe(8); // band 06:00–06:29 to
    expect(calc.feeFor(car, [at("06:30")])).toBe(13); // band 06:30–06:59 from
    expect(calc.feeFor(car, [at("18:29")])).toBe(8); // last band's to
    expect(calc.feeFor(car, [at("18:30")])).toBe(0); // one minute past last band
  });

  it("row 13 — same-millisecond duplicate passes are charged once", () => {
    const calc = calculatorWith(NEVER_HOLIDAY);
    const instant = at("07:15"); // fee=18
    // Three identical instants — windowed together at max=18, charged once.
    expect(calc.feeFor(car, [instant, instant, instant])).toBe(18);
  });
});

describe("createTollCalculator — configurable behavior", () => {
  it("respects a custom dailyCap (lower)", () => {
    const calc = calculatorWith(NEVER_HOLIDAY, { dailyCap: 20 });
    // Two non-overlapping windows: 18 + 13 = 31 → clamped to 20.
    expect(calc.feeFor(car, [at("07:00"), at("08:30")])).toBe(20);
  });

  it("respects a custom dailyCap (higher)", () => {
    const calc = calculatorWith(NEVER_HOLIDAY, { dailyCap: 1000 });
    // Same scenario that capped at 60 above; no clamping at 1000.
    expect(
      calc.feeFor(car, [
        at("07:00"),
        at("08:05"),
        at("15:30"),
        at("16:35"),
        at("17:40"),
      ]),
    ).toBe(80);
  });

  it("respects a different fee schedule", () => {
    const calc = calculatorWith(NEVER_HOLIDAY, {
      schedule: [{ from: "00:00", to: "23:59", fee: 5 }],
    });
    expect(calc.feeFor(car, [at("12:00")])).toBe(5);
    expect(calc.feeFor(car, [at("23:59")])).toBe(5);
  });
});
