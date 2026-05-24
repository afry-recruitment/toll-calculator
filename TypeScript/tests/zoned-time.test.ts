import { describe, it, expect } from "vitest";
import { parts } from "../src/time/zoned-time.js";
import { minutesBetween } from "../src/time/diff.js";

describe("parts (wall-clock extraction across IANA zones)", () => {
  it("maps UTC 04:30 to 06:30 in Europe/Stockholm during summer (CEST = UTC+2)", () => {
    const result = parts(new Date("2025-09-17T04:30:00Z"), "Europe/Stockholm");
    expect(result).toEqual({
      year: 2025,
      month: 9,
      day: 17,
      hour: 6,
      minute: 30,
      weekday: 3, // Wednesday (ISO)
    });
  });

  it("maps UTC 05:30 to 06:30 in Europe/Stockholm during winter (CET = UTC+1)", () => {
    const result = parts(new Date("2025-12-10T05:30:00Z"), "Europe/Stockholm");
    expect(result).toEqual({
      year: 2025,
      month: 12,
      day: 10,
      hour: 6,
      minute: 30,
      weekday: 3, // Wednesday
    });
  });

  it("handles DST spring-forward correctly (Europe/Stockholm flips at 01:00 UTC on 2025-03-30)", () => {
    // 00:59 UTC → still CET (01:59 wall-clock)
    const before = parts(new Date("2025-03-30T00:59:00Z"), "Europe/Stockholm");
    expect(before.hour).toBe(1);
    expect(before.minute).toBe(59);

    // 01:00 UTC → CEST (03:00 wall-clock — the 02:xx hour never happens locally)
    const after = parts(new Date("2025-03-30T01:00:00Z"), "Europe/Stockholm");
    expect(after.hour).toBe(3);
    expect(after.minute).toBe(0);
  });

  it("handles DST fall-back correctly (Europe/Stockholm flips at 01:00 UTC on 2025-10-26)", () => {
    // 00:30 UTC → still CEST (02:30 wall-clock)
    const before = parts(new Date("2025-10-26T00:30:00Z"), "Europe/Stockholm");
    expect(before.hour).toBe(2);
    expect(before.minute).toBe(30);

    // 01:30 UTC → CET (02:30 wall-clock — same wall time, but 1 hour later in UTC)
    const after = parts(new Date("2025-10-26T01:30:00Z"), "Europe/Stockholm");
    expect(after.hour).toBe(2);
    expect(after.minute).toBe(30);
  });

  it("returns the same instant with different wall-clocks across zones", () => {
    const instant = new Date("2025-09-17T14:45:00Z");

    expect(parts(instant, "Europe/Stockholm")).toMatchObject({
      hour: 16,
      minute: 45,
    });
    expect(parts(instant, "America/New_York")).toMatchObject({
      hour: 10,
      minute: 45,
    });
    expect(parts(instant, "Asia/Tokyo")).toMatchObject({
      hour: 23,
      minute: 45,
    });
  });

  it("derives ISO weekday correctly across the week", () => {
    const zone = "Europe/Stockholm";
    // 2025-09-15 is a Monday in Stockholm.
    expect(parts(new Date("2025-09-15T10:00:00Z"), zone).weekday).toBe(1);
    expect(parts(new Date("2025-09-16T10:00:00Z"), zone).weekday).toBe(2);
    expect(parts(new Date("2025-09-17T10:00:00Z"), zone).weekday).toBe(3);
    expect(parts(new Date("2025-09-18T10:00:00Z"), zone).weekday).toBe(4);
    expect(parts(new Date("2025-09-19T10:00:00Z"), zone).weekday).toBe(5);
    expect(parts(new Date("2025-09-20T10:00:00Z"), zone).weekday).toBe(6);
    expect(parts(new Date("2025-09-21T10:00:00Z"), zone).weekday).toBe(7);
  });

  it("returns 1-12 month numbering, not 0-11", () => {
    // January
    expect(
      parts(new Date("2025-01-15T12:00:00Z"), "Europe/Stockholm").month,
    ).toBe(1);
    // December
    expect(
      parts(new Date("2025-12-15T12:00:00Z"), "Europe/Stockholm").month,
    ).toBe(12);
  });

  it("rejects invalid IANA zone with a clear error", () => {
    expect(() =>
      parts(new Date("2025-09-17T12:00:00Z"), "Europe/Stockholmo"),
    ).toThrow(RangeError);
    expect(() =>
      parts(new Date("2025-09-17T12:00:00Z"), "Mars/Olympus_Mons"),
    ).toThrow(RangeError);
  });
});

describe("minutesBetween (UTC ms diff in minutes)", () => {
  it("returns 0 for the same instant", () => {
    const t = new Date("2025-09-17T12:00:00Z");
    expect(minutesBetween(t, t)).toBe(0);
  });

  it("returns the positive difference when later > earlier", () => {
    const a = new Date("2025-09-17T12:00:00Z");
    const b = new Date("2025-09-17T13:30:00Z");
    expect(minutesBetween(a, b)).toBe(90);
  });

  it("returns the negative difference when later < earlier", () => {
    const a = new Date("2025-09-17T13:30:00Z");
    const b = new Date("2025-09-17T12:00:00Z");
    expect(minutesBetween(a, b)).toBe(-90);
  });

  it("returns fractional minutes for sub-minute differences", () => {
    const a = new Date("2025-09-17T12:00:00.000Z");
    const b = new Date("2025-09-17T12:00:30.000Z");
    expect(minutesBetween(a, b)).toBe(0.5);
  });

  it("is unaffected by DST — measures UTC duration, not wall-clock duration", () => {
    // Two passes 60 UTC-minutes apart across the spring-forward transition.
    // Wall clock moves 2 hours (00:59 CET → 03:00 CEST), but UTC moves 1 hour.
    // The 60-minute window rule must see 60, not 120.
    const before = new Date("2025-03-30T00:30:00Z");
    const after = new Date("2025-03-30T01:30:00Z");
    expect(minutesBetween(before, after)).toBe(60);
  });
});
