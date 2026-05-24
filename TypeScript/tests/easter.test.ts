import { describe, it, expect } from "vitest";
import { easterSunday } from "../src/calendar/easter.js";

/**
 * Reference Easter dates (Gregorian). Cross-checked against:
 *   - U.S. Naval Observatory, "Dates of Easter Sunday"
 *   - Meeus, J. (1998). Astronomical Algorithms, 2nd ed., ch. 8.
 */
const KNOWN_EASTERS: readonly (readonly [number, number, number])[] = [
  [1961, 4, 2], // Meeus's worked example
  [1981, 4, 19], // exercises the april_easter==26 → 19 correction
  [2000, 4, 23],
  [2013, 3, 31], // the year of the bundled reference data
  [2024, 3, 31],
  [2025, 4, 20],
  [2026, 4, 5],
  [2038, 4, 25], // exercises the april_easter==25 boundary
];

describe("easterSunday", () => {
  for (const [year, month, day] of KNOWN_EASTERS) {
    it(`returns ${String(year)}-${String(month)}-${String(day)} for ${String(year)}`, () => {
      expect(easterSunday(year)).toEqual({ month, day });
    });
  }

  it("falls within March 22 – April 25 inclusive for every year 1583–2500", () => {
    for (let year = 1583; year <= 2500; year++) {
      const { month, day } = easterSunday(year);
      const ordinal = month === 3 ? day : day + 31;
      expect(ordinal).toBeGreaterThanOrEqual(22);
      expect(ordinal).toBeLessThanOrEqual(56);
    }
  });
});
