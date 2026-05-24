/**
 * Discriminator for {@link Vehicle}. A string literal union, not an enum:
 * enums emit a runtime object that we do not need, and a union plays well
 * with exhaustive `switch` checks via `never`.
 */
export type VehicleType =
  | "Car"
  | "Motorbike"
  | "Tractor"
  | "Emergency"
  | "Diplomat"
  | "Foreign"
  | "Military";

export interface Vehicle {
  readonly type: VehicleType;
}

/**
 * Set of vehicle types that are always exempt from tolls. Membership is
 * a `Set` lookup, not a chain of string equality checks — kills bug
 * `[D§4.7]` from the reference code.
 */
const TOLL_FREE_TYPES: ReadonlySet<VehicleType> = new Set<VehicleType>([
  "Motorbike",
  "Tractor",
  "Emergency",
  "Diplomat",
  "Foreign",
  "Military",
]);

export const isTollFreeVehicle = (vehicle: Vehicle): boolean =>
  TOLL_FREE_TYPES.has(vehicle.type);
