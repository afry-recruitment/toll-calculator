import { describe, expect, it } from "vitest";
import { calculateDailyTollFee } from "../src/calculator";

function passage(hour: number, minute: number): Date {
    return new Date(2026, 9, 30, hour, minute);
}

describe("calculateDailyTollFee", () => {
    it("returns 0 when there are no passages", () => {
        expect(calculateDailyTollFee("standard", [])).toBe(0);
    });

    it("returns 0 for a tollfree vehicle", () => {
        expect(
            calculateDailyTollFee("motorbike", [passage(7, 0)])
        ).toBe(0);
    });

    it("calculates the daily toll fee", () => {
        expect(
            calculateDailyTollFee("standard", [
                passage(6, 10),
                passage(7, 30),
            ])
        ).toBe(26);
    });

    it("caps the daily fee at 60 SEK", () => {
        expect(
            calculateDailyTollFee("standard", [
                passage(6, 0),
                passage(7, 1),
                passage(15, 30),
                passage(17, 0),
                passage(18, 10),
            ])
        ).toBe(60);
    });

    it("rejects passages from different days", () => {
        const firstDay = new Date(2026, 9, 30, 7, 0);
        const secondDay = new Date(2026, 9, 31, 7, 0);

        expect(() =>
            calculateDailyTollFee("standard", [firstDay, secondDay])
        ).toThrow();
    });

    it("rejects invalid passage dates", () => {
        expect(() =>
            calculateDailyTollFee("standard", [new Date("invalid")])
        ).toThrow();
    });

    it("does not mutate the input passages", () => {
        const passages = [
            new Date(2026, 9, 30, 7, 0),
            new Date(2026, 9, 30, 6, 30),
        ];

        const originalOrder = [...passages];

        calculateDailyTollFee("standard", passages);

        expect(passages).toEqual(originalOrder);
    });
});