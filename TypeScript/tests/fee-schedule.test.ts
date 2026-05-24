import { describe, it, expect } from "vitest";
import { feeAt } from "../src/fee-schedule.js";
import type { FeeSchedule } from "../src/fee-schedule.js";
import { REFERENCE_FEE_SCHEDULE } from "../src/index.js";

/**
 * `feeAt` tests use a minimal, hand-rolled schedule so that failures point
 * at the lookup logic, not at the reference data. The reference schedule
 * is exercised separately in the section below.
 */
describe("feeAt", () => {
  const schedule: FeeSchedule = [
    { from: "06:00", to: "06:29", fee: 8 },
    { from: "07:00", to: "07:59", fee: 18 },
  ];

  it("returns the band fee at its from boundary (inclusive)", () => {
    expect(feeAt(schedule, 6, 0)).toBe(8);
    expect(feeAt(schedule, 7, 0)).toBe(18);
  });

  it("returns the band fee at its to boundary (inclusive)", () => {
    expect(feeAt(schedule, 6, 29)).toBe(8);
    expect(feeAt(schedule, 7, 59)).toBe(18);
  });

  it("returns the band fee at the midpoint", () => {
    expect(feeAt(schedule, 6, 15)).toBe(8);
    expect(feeAt(schedule, 7, 30)).toBe(18);
  });

  it("returns 0 in the gap between bands", () => {
    expect(feeAt(schedule, 6, 30)).toBe(0);
    expect(feeAt(schedule, 6, 45)).toBe(0);
    expect(feeAt(schedule, 6, 59)).toBe(0);
  });

  it("returns 0 before the first band", () => {
    expect(feeAt(schedule, 0, 0)).toBe(0);
    expect(feeAt(schedule, 5, 59)).toBe(0);
  });

  it("returns 0 after the last band", () => {
    expect(feeAt(schedule, 8, 0)).toBe(0);
    expect(feeAt(schedule, 23, 59)).toBe(0);
  });

  it("returns 0 on an empty schedule", () => {
    expect(feeAt([], 12, 0)).toBe(0);
  });

  it("rejects malformed band times with a clear error", () => {
    const bad: FeeSchedule = [{ from: "06", to: "06:29", fee: 8 }];
    expect(() => feeAt(bad, 6, 0)).toThrow(/Invalid time format/);
  });
});

/**
 * Meta-tests on REFERENCE_FEE_SCHEDULE. These run against the actual data
 * shipped with the library, not a sample. Together they guarantee:
 *   • No two bands overlap (would be a data bug).
 *   • Every minute in 06:00–18:29 is covered by exactly one band
 *     (specifically catches the "missing 09:00–14:29 hole" from bug
 *     `[D§4.3]` in the reference Java code).
 *   • Every minute outside 06:00–18:29 is covered by zero bands
 *     (no surprise fees at 03:00 or 22:00).
 */
describe("REFERENCE_FEE_SCHEDULE (meta-tests on the bundled data)", () => {
  const ACTIVE_START = 6 * 60; // 06:00
  const ACTIVE_END = 18 * 60 + 29; // 18:29

  const bandMatchCount = (minuteOfDay: number): number => {
    let matches = 0;
    for (const band of REFERENCE_FEE_SCHEDULE) {
      const from = toMinutes(band.from);
      const to = toMinutes(band.to);
      if (minuteOfDay >= from && minuteOfDay <= to) matches++;
    }
    return matches;
  };

  it("covers every minute in 06:00–18:29 with exactly one band", () => {
    const uncovered: number[] = [];
    for (let m = ACTIVE_START; m <= ACTIVE_END; m++) {
      if (bandMatchCount(m) !== 1) uncovered.push(m);
    }
    expect(uncovered).toEqual([]);
  });

  it("covers no minute outside 06:00–18:29", () => {
    const surprises: number[] = [];
    for (let m = 0; m < 24 * 60; m++) {
      if (m >= ACTIVE_START && m <= ACTIVE_END) continue;
      if (bandMatchCount(m) !== 0) surprises.push(m);
    }
    expect(surprises).toEqual([]);
  });

  it.each([
    { hour: 6, minute: 0, fee: 8 },
    { hour: 6, minute: 29, fee: 8 },
    { hour: 6, minute: 30, fee: 13 },
    { hour: 7, minute: 0, fee: 18 },
    { hour: 7, minute: 59, fee: 18 },
    { hour: 8, minute: 0, fee: 13 },
    { hour: 8, minute: 30, fee: 8 }, // would be 0 in the buggy Java reference
    { hour: 12, minute: 0, fee: 8 }, // would be 0 in the buggy Java reference
    { hour: 14, minute: 29, fee: 8 }, // would be 0 in the buggy Java reference
    { hour: 15, minute: 0, fee: 13 },
    { hour: 15, minute: 30, fee: 18 },
    { hour: 16, minute: 59, fee: 18 },
    { hour: 17, minute: 0, fee: 13 },
    { hour: 18, minute: 0, fee: 8 },
    { hour: 18, minute: 29, fee: 8 },
    { hour: 18, minute: 30, fee: 0 },
    { hour: 5, minute: 59, fee: 0 },
    { hour: 23, minute: 0, fee: 0 },
  ])("feeAt $hour:$minute → $fee", ({ hour, minute, fee }) => {
    expect(feeAt(REFERENCE_FEE_SCHEDULE, hour, minute)).toBe(fee);
  });
});

const toMinutes = (hhmm: string): number => {
  const [hh, mm] = hhmm.split(":");
  if (hh === undefined || mm === undefined) {
    throw new Error(`Bad band time in test: ${hhmm}`);
  }
  return Number(hh) * 60 + Number(mm);
};
