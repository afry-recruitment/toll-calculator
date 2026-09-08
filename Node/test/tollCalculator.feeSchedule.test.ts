import { tollFees } from "../src/lib/tollFeeSchedule.js";
import { GetTollFeeForDates } from "../src/tollCalculator.js";
import { passageTime, car } from "./helper/helper.js";

describe("Toll fees outside rush hour", () => {
  test.each([
    ["00:00", tollFees.feeFree],
    ["05:59", tollFees.feeFree],

    ["06:00", tollFees.feeLowest],
    ["06:29", tollFees.feeLowest],
    ["06:30", tollFees.feeMiddle],
    ["06:59", tollFees.feeMiddle],
    ["08:00", tollFees.feeMiddle],
    ["08:29", tollFees.feeMiddle],

    ["08:30", tollFees.feeLowest],
    ["09:00", tollFees.feeLowest],
    ["10:15", tollFees.feeLowest],
    ["12:00", tollFees.feeLowest],
    ["14:59", tollFees.feeLowest],

    ["15:00", tollFees.feeMiddle],
    ["15:29", tollFees.feeMiddle],

    ["17:00", tollFees.feeMiddle],
    ["17:59", tollFees.feeMiddle],
    ["18:00", tollFees.feeLowest],
    ["18:29", tollFees.feeLowest],
    ["18:30", tollFees.feeFree],
    ["23:59", tollFees.feeFree],
  ])("A passage at %s costs %i SEK", (time, expectedFee) => {
    // 2026-09-08
    const passage = passageTime({ time, day: 8, month: 8, year: 2026 });

    const totalFee = GetTollFeeForDates({
      vehicle: car,
      dates: [passage],
    });

    expect(totalFee).toBe(expectedFee);
  });
});
