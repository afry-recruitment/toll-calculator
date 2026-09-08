import { isWithinOneHour } from "../src/lib/date.js";

describe("isWithinOneHour", () => {
  test.each([
    { time: "06:30:00.000", expected: true },
    { time: "07:00:00.000", expected: true },
    { time: "07:00:00.001", expected: false },
  ])("06:00 to $time returns $expected", ({ time, expected }) => {
    const start = new Date("2026-09-07T06:00:00.000Z");
    const date = new Date(`2026-09-07T${time}Z`);

    expect(isWithinOneHour(start, date)).toBe(expected);
  });
});
