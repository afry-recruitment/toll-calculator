import { tollFees } from "../src/lib/tollFeeSchedule.js";
import { GetTollFeeForDates } from "../src/tollCalculator.js";
import { passageTime, car } from "./helper/helper.js";

describe("Rush-hour toll fees", () => {
  test.each(["07:00", "07:30", "07:59", "15:30", "16:00", "16:59"])(
    `A passage at %s during rush hour costs ${tollFees.feeHighest} SEK`,
    (time) => {
      // Arrange
      const passage = passageTime({ time, day: 8, month: 8, year: 2026 });

      // Act
      const totalFee = GetTollFeeForDates({ vehicle: car, dates: [passage] });

      // Assert
      expect(totalFee).toBe(tollFees.feeHighest);
    },
  );
});
