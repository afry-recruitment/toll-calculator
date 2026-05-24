import { describe, expect } from "vitest";
import { fc, test } from "@fast-check/vitest";
import { createTollCalculator } from "../src/calculator.js";
import type { Vehicle, VehicleType } from "../src/vehicle.js";
import type { FeeSchedule } from "../src/fee-schedule.js";
import type { HolidayCalendar } from "../src/calendar/index.js";

/**
 * Property-based tests via fast-check. Each property covers an infinite
 * family of inputs and asserts an invariant the calculator must satisfy
 * for *every* member of the family. A counter-example shrinks to the
 * minimal failing case automatically.
 *
 * Properties live here, not in `calculator.test.ts`, so a reviewer can
 * see at a glance which invariants are tested generatively vs by
 * example. The four properties match `docs/plan/05-testing-strategy.md`
 * §5.3.
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

const DAILY_CAP = 60;
const NEVER_HOLIDAY: HolidayCalendar = { isHoliday: () => false };

const calc = createTollCalculator({
  schedule: TEST_SCHEDULE,
  calendar: NEVER_HOLIDAY,
  dailyCap: DAILY_CAP,
  timeZone: "UTC",
});

const CAR: Vehicle = { type: "Car" };

const TOLL_FREE_TYPES: readonly VehicleType[] = [
  "Motorbike",
  "Tractor",
  "Emergency",
  "Diplomat",
  "Foreign",
  "Military",
];

/**
 * Arbitrary date within a sensible range. `noInvalidDate: true` ensures
 * no `NaN`-time Date objects (which `assertPasses` would reject and
 * derail the property).
 */
const dateArb = fc.date({
  min: new Date("2024-01-01T00:00:00Z"),
  max: new Date("2030-12-31T23:59:59Z"),
  noInvalidDate: true,
});

describe("calculator — properties (fast-check)", () => {
  test.prop([fc.array(dateArb, { maxLength: 50 })])(
    "(1) `feeFor(Car, passes)` never exceeds `dailyCap`",
    (passes) => {
      expect(calc.feeFor(CAR, passes)).toBeLessThanOrEqual(DAILY_CAP);
    },
  );

  test.prop([
    fc.constantFrom(...TOLL_FREE_TYPES),
    fc.array(dateArb, { maxLength: 50 }),
  ])(
    "(2) toll-free vehicles always return 0, regardless of passes",
    (type, passes) => {
      expect(calc.feeFor({ type }, passes)).toBe(0);
    },
  );

  test.prop([fc.array(dateArb, { maxLength: 30 })])(
    "(3) `feeFor` is order-independent — `f(passes) === f(shuffle(passes))`",
    (passes) => {
      const shuffled = [...passes].reverse();
      expect(calc.feeFor(CAR, passes)).toBe(calc.feeFor(CAR, shuffled));
    },
  );

  test.prop([fc.array(dateArb, { maxLength: 20 }), dateArb])(
    "(4) `feeFor` is monotone — adding a pass never decreases the total",
    (passes, extra) => {
      const before = calc.feeFor(CAR, passes);
      const after = calc.feeFor(CAR, [...passes, extra]);
      expect(after).toBeGreaterThanOrEqual(before);
    },
  );
});
