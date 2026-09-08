import { MAX_TOTAL_FEE } from "../src/lib/tollFeeSchedule.js";
import { GetTollFeeForDates } from "../src/tollCalculator.js";
import { passageTime, car } from "./helper/helper.js";

//Fails
describe(`The maximum fee for one day is ${MAX_TOTAL_FEE} SEK`, () => {
  test(`The daily toll fee is capped at ${MAX_TOTAL_FEE} SEK even when passage fees exceed the limit`, () => {
    // Arrange
    const multiplePassages = [
      passageTime({ time: "06:30", day: 8, month: 8, year: 2026 }),
      passageTime({ time: "07:00", day: 8, month: 8, year: 2026 }),
      passageTime({ time: "08:30", day: 8, month: 8, year: 2026 }),
      passageTime({ time: "15:30", day: 8, month: 8, year: 2026 }),
      passageTime({ time: "17:00", day: 8, month: 8, year: 2026 }),
      passageTime({ time: "18:01", day: 8, month: 8, year: 2026 }),
    ];

    // Act
    const actualFee = GetTollFeeForDates({ vehicle: car, dates: multiplePassages });
    const expectedFee = MAX_TOTAL_FEE;
    // Assert
    expect(actualFee).toBe(expectedFee);
  });
});
