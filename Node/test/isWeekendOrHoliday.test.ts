import { isWeekendOrHoliday } from "../src/lib/tollExemptions.js";

describe("Weekends and holidays are fee-free", () => {
  test.each([
    ["Saturday", new Date(2026, 8, 5)],
    ["Sunday", new Date(2026, 8, 6)],
    ["New Year's Day 2026", new Date(2026, 0, 1)],
    ["New Year's Day 2027", new Date(2027, 0, 1)],
    ["Christmas Day 2026", new Date(2026, 11, 25)],
    ["Christmas Day 2027", new Date(2027, 11, 25)],
  ])("returns true for %s", (_, date) => {
    // Act
    const actual = isWeekendOrHoliday(date);

    // Assert
    expect(actual).toBe(true);
  });

  test.each([
    ["Monday, September 7, 2026", new Date(2026, 8, 7)],
    ["Tuesday, October 6, 2026", new Date(2026, 9, 6)],
    ["Monday, January 4, 2027", new Date(2027, 0, 4)],
  ])("returns false for an ordinary weekday: %s", (_, date) => {
    // Act
    const actual = isWeekendOrHoliday(date);

    // Assert
    expect(actual).toBe(false);
  });
});
