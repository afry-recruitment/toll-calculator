import { tollFees } from "../src/lib/tollFeeSchedule.js";
import { GetTollFeeForDates } from "../src/tollCalculator.js";
import { car } from "./helper/helper.js";

describe("Weekends and holidays are fee-free", () => {
  test.each([
    ["Saturday", new Date(2026, 8, 5, 7)],
    ["Sunday", new Date(2026, 8, 6, 7)],
    ["New Year's Day 2026", new Date(2026, 0, 1, 7)],
    ["Christmas Day 2026", new Date(2026, 11, 25, 7)],
    ["Christmas Day 2027", new Date(2027, 11, 25, 7)],
  ])(`A passage at 07:00 on %s costs ${tollFees.feeFree} SEK`, (_, date) => {
    // Act
    const actual = GetTollFeeForDates({ vehicle: car, dates: [date] });

    // Assert
    expect(actual).toBe(tollFees.feeFree);
  });
});
