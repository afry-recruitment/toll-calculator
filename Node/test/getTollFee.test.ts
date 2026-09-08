import { tollFees } from "../src/lib/tollFeeSchedule.js";
import { GetTollFee } from "../src/lib/tollFees.js";
import { car } from "./helper/helper.js";

describe("GetTollFee", () => {
  test.each([
    ["00:00:00.000", tollFees.feeFree],
    ["05:59:59.999", tollFees.feeFree],
    ["06:00:00.000", tollFees.feeLowest],
    ["06:29:59.999", tollFees.feeLowest],
    ["06:30:00.000", tollFees.feeMiddle],
    ["06:59:59.999", tollFees.feeMiddle],
    ["07:00:00.000", tollFees.feeHighest],
    ["07:59:59.999", tollFees.feeHighest],
    ["08:00:00.000", tollFees.feeMiddle],
    ["08:29:59.999", tollFees.feeMiddle],
    ["08:30:00.000", tollFees.feeLowest],
    ["09:15:00.000", tollFees.feeLowest],
    ["14:59:59.999", tollFees.feeLowest],
    ["15:00:00.000", tollFees.feeMiddle],
    ["15:29:59.999", tollFees.feeMiddle],
    ["15:30:00.000", tollFees.feeHighest],
    ["16:59:59.999", tollFees.feeHighest],
    ["17:00:00.000", tollFees.feeMiddle],
    ["17:59:59.999", tollFees.feeMiddle],
    ["18:00:00.000", tollFees.feeLowest],
    ["18:29:59.999", tollFees.feeLowest],
    ["18:30:00.000", tollFees.feeFree],
    ["23:59:59.999", tollFees.feeFree],
  ])("charges %s at the expected rate of %i SEK", (time, expectedFee) => {
    const date = new Date(`2026-09-07T${time}+02:00`);

    expect(GetTollFee(date, car)).toBe(expectedFee);
  });
});
