import { describe, it, expect } from "vitest";
import { combineCalendars } from "../src/calendar/combine.js";
import type { HolidayCalendar } from "../src/calendar/index.js";

const calendarOf = (predicate: (date: Date) => boolean): HolidayCalendar => ({
  isHoliday: predicate,
});

const ANY_DATE = new Date("2025-09-17T12:00:00Z");

describe("combineCalendars", () => {
  it("returns a calendar that flags nothing when given no inputs", () => {
    const combined = combineCalendars();
    expect(combined.isHoliday(ANY_DATE)).toBe(false);
  });

  it("delegates to a single calendar (pass-through)", () => {
    const always = calendarOf(() => true);
    const never = calendarOf(() => false);

    expect(combineCalendars(always).isHoliday(ANY_DATE)).toBe(true);
    expect(combineCalendars(never).isHoliday(ANY_DATE)).toBe(false);
  });

  it("returns true if ANY input calendar reports the date as a holiday (OR semantics)", () => {
    const never = calendarOf(() => false);
    const always = calendarOf(() => true);

    expect(combineCalendars(never, never, always).isHoliday(ANY_DATE)).toBe(
      true,
    );
    expect(combineCalendars(never, never, never).isHoliday(ANY_DATE)).toBe(
      false,
    );
  });

  it("evaluates each input independently per date", () => {
    const dayA = new Date("2025-01-01T12:00:00Z");
    const dayB = new Date("2025-02-02T12:00:00Z");
    const dayC = new Date("2025-03-03T12:00:00Z");

    const onlyA = calendarOf((d) => d.getTime() === dayA.getTime());
    const onlyB = calendarOf((d) => d.getTime() === dayB.getTime());

    const combined = combineCalendars(onlyA, onlyB);

    expect(combined.isHoliday(dayA)).toBe(true);
    expect(combined.isHoliday(dayB)).toBe(true);
    expect(combined.isHoliday(dayC)).toBe(false);
  });
});
