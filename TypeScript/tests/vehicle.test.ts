import { describe, it, expect, expectTypeOf } from "vitest";
import { isTollFreeVehicle } from "../src/vehicle.js";
import type { Vehicle, VehicleType } from "../src/vehicle.js";

describe("isTollFreeVehicle", () => {
  it.each<VehicleType>([
    "Motorbike",
    "Tractor",
    "Emergency",
    "Diplomat",
    "Foreign",
    "Military",
  ])("returns true for %s", (type) => {
    const vehicle: Vehicle = { type };
    expect(isTollFreeVehicle(vehicle)).toBe(true);
  });

  it("returns false for Car (the only chargeable type)", () => {
    expect(isTollFreeVehicle({ type: "Car" })).toBe(false);
  });
});

describe("VehicleType (type-level)", () => {
  it("is the closed union of the seven documented types", () => {
    expectTypeOf<VehicleType>().toEqualTypeOf<
      | "Car"
      | "Motorbike"
      | "Tractor"
      | "Emergency"
      | "Diplomat"
      | "Foreign"
      | "Military"
    >();
  });
});
