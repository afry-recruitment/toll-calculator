import { describe, expect, it } from "vitest";
import { isTollFreeVehicle } from "../src/vehicle";
import type { VehicleType } from "../src/types";

describe("isTollFreeVehicle", () => {
    const cases: readonly [VehicleType, boolean][] = [
        ["standard", false],
        ["motorbike", true],
        ["tractor", true],
        ["emergency", true],
        ["diplomat", true],
        ["foreign", true],
        ["military", true],
    ];

    it.each(cases)("%s toll-free status should be %s", (vehicleType, expected) => {
        expect(isTollFreeVehicle(vehicleType)).toBe(expected);
    });
});