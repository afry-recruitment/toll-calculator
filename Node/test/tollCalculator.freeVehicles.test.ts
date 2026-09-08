import { tollFees } from "../src/lib/tollFeeSchedule.js";
import { tollFreeVehicles } from "../src/lib/tollExemptions.js";
import { GetTollFeeForDates } from "../src/tollCalculator.js";

import { passageTime, motorBike } from "./helper/helper.js";

describe("Some vehicle types are fee-free", () => {
  test(`Fee-free vehicle types are exempt: a motorbike returns ${tollFees.feeFree} SEK`, () => {
    // Arrange
    const passages = [
      passageTime({ time: "06:00", day: 8, month: 8, year: 2026 }),
      passageTime({ time: "09:00", day: 8, month: 8, year: 2026 }),
      passageTime({ time: "15:30", day: 8, month: 8, year: 2026 }),
    ];

    // Act
    const actual = GetTollFeeForDates({ vehicle: motorBike, dates: passages });

    // Assert
    expect(actual).toBe(tollFees.feeFree);
  });

  test.each(tollFreeVehicles)(
    `Fee-free vehicle types are exempt: a %s returns ${tollFees.feeFree} SEK`,
    (vehicle) => {
      // Arrange
      const passages = [passageTime({ time: "07:00", day: 8, month: 8, year: 2026 })];

      // Act
      const actual = GetTollFeeForDates({ vehicle, dates: passages });

      // Assert
      expect(actual).toBe(tollFees.feeFree);
    },
  );
});
