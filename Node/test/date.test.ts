import { isWithinOneHour } from "../src/lib/date.js";

describe("isWithinOneHour", () => {
  test.each([
    ["the same time", "2026-09-07T06:00:00.000Z", true],
    ["30 minutes later", "2026-09-07T06:30:00.000Z", true],
    ["just under one hour later", "2026-09-07T06:59:59.999Z", true],
    ["exactly one hour later", "2026-09-07T07:00:00.000Z", true],
    ["one hour and one millisecond later", "2026-09-07T07:00:00.001Z", false],
    ["two hours later", "2026-09-07T08:00:00.000Z", false],
    ["the same time the next day", "2026-09-08T06:00:00.000Z", false],
  ])("handles a passage at %s", (_, timestamp, expected) => {
    // Arrange
    const start = new Date("2026-09-07T06:00:00.000Z");
    const date = new Date(timestamp);

    // Act
    const actual = isWithinOneHour(start, date);

    // Assert
    expect(actual).toBe(expected);
  });

  test("returns true for exactly one hour across midnight", () => {
    // Arrange
    const start = new Date("2026-09-07T23:30:00.000Z");
    const date = new Date("2026-09-08T00:30:00.000Z");

    // Act
    const actual = isWithinOneHour(start, date);

    // Assert
    expect(actual).toBe(true);
  });
});
