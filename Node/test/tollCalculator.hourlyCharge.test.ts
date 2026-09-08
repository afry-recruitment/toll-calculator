import { tollFees } from "../src/lib/tollFeeSchedule.js";
import { GetTollFeeForDates } from "../src/tollCalculator.js";
import { passageTime, car } from "./helper/helper.js";

describe("One-hour charge windows", () => {
  test.each([
    {
      description: "Multiple passages with the same fee within 60 minutes charges once",
      times: ["06:00", "06:10"],
      expectedFee: tollFees.feeLowest,
    },
    {
      description: "Passages exactly 60 minutes apart are charged once at the highest fee",
      times: ["06:10", "07:10"],
      expectedFee: tollFees.feeHighest,
    },
    {
      description: "Passages more than 60 minutes apart are charged separately",
      times: ["06:10", "07:20"],
      expectedFee: tollFees.feeLowest + tollFees.feeHighest,
    },
    {
      description: "Increasing fees within 60 minutes charges the higher fee",
      times: ["06:45", "07:10"],
      expectedFee: tollFees.feeHighest,
    },
    {
      description: "Decreasing fees within 60 minutes keep the highest fee",
      times: ["07:45", "08:00", "08:30"],
      expectedFee: tollFees.feeHighest,
    },
  ])("$description", ({ times, expectedFee }) => {
    // Arrange
    const passages = times.map((time) => passageTime({ time, day: 8, month: 8, year: 2026 }));

    // Act
    const actual = GetTollFeeForDates({ vehicle: car, dates: passages });

    // Assert
    expect(actual).toBe(expectedFee);
  });
});
