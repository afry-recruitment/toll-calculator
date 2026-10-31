import { describe, expect, it } from "vitest";
import { getTollFeeForTime } from "../src/tariff";

function time(hour: number, minute: number): Date {
  return new Date(2026, 9, 30, hour, minute);
}

describe("getTollFeeForTime", () => {
    it.each([
        [5, 59, 0],
        [6, 0, 8],
        [6, 29, 8],
        [6, 30, 13],
        [6, 59, 13],
        [7, 0, 18],
        [7, 59, 18],
        [8, 0, 13],
        [8, 29, 13],
        [8, 30, 8],
        [14, 59, 8],
        [15, 0, 13],
        [15, 29, 13],
        [15, 30, 18],
        [16, 59, 18],
        [17, 0, 13],
        [17, 59, 13],
        [18, 0, 8],
        [18, 29, 8],
        [18, 30, 0],
    ])(
        "%02i:%02i should cost %i SEK",
        (hour, minute, expectedFee) => {
        expect(getTollFeeForTime(time(hour, minute))).toBe(expectedFee);
        },
    );
});