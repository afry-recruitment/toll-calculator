import { describe, expect, it } from "vitest";
import { calculatePassageFees } from "../src/chargingRule";

function passage(hour: number, minute: number): Date {
  return new Date(2026, 9, 30, hour, minute);
}

describe("calculatePassageFees", () => {
    it("returns 0 when there are no passages", () => {
        expect(calculatePassageFees([])).toBe(0);
    });

    it("returns the fee for a single passage", () => {
        expect(
        calculatePassageFees([
            passage(7, 0),
        ]),
        ).toBe(18);
    });

    it("only charges the highest fee within the same 60 minute window", () => {
        expect(
        calculatePassageFees([
            passage(6, 10), // 8
            passage(6, 40), // 13
        ]),
        ).toBe(13);
    });

    it("includes a passage exactly 60 minutes after the window starts", () => {
        expect(
        calculatePassageFees([
            passage(6, 0), // 8
            passage(7, 0), // 18
        ]),
        ).toBe(18);
    });

    it("starts a new charge window after more than 60 minutes", () => {
        expect(
        calculatePassageFees([
            passage(6, 0), 
            passage(7, 1),
        ]),
        ).toBe(26);
    });

    it("ignores no cost passages when creating charge windows", () => {
  expect(
    calculatePassageFees([
      new Date(2026, 9, 30, 5, 50),
      new Date(2026, 9, 30, 6, 40),
      new Date(2026, 9, 30, 7, 30),
    ])
  ).toBe(18);
});

    it("handles passages that are not supplied chronologically", () => {
        expect(
        calculatePassageFees([
            passage(7, 0),
            passage(6, 40),
        ]),
        ).toBe(18);
    });
});