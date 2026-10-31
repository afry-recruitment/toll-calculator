import { describe, expect, it } from "vitest";
import { isTollFreeDate } from "../src/calendar";

describe("isTollFreeDate", () => {
    it("returns true on weekends", () => {
      expect(isTollFreeDate(new Date(2026, 9, 31))).toBe(true);
    });

    it("returns true on a Swedish public holiday", () => {
      expect(isTollFreeDate(new Date(2026, 11, 25))).toBe(true);
    });

    it("returns true on midsummer day 2026", () => {
      expect(isTollFreeDate(new Date(2026, 5, 20))).toBe(true);
    });

    it("returns true on midsummer day 2027", () => {
      expect(isTollFreeDate(new Date(2027, 5, 26))).toBe(true);
    });

    it("returns false on a regular weekday", () => {
      expect(isTollFreeDate(new Date(2026, 9, 30))).toBe(false);
    });
  
});