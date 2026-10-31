import type { VehicleType } from "./types";

const TOLL_FREE_VEHICLES: readonly VehicleType[] = ["motorbike", "tractor", "emergency", "diplomat", "foreign", "military"];

export function isTollFreeVehicle(type: VehicleType): boolean {
    return TOLL_FREE_VEHICLES.includes(type);
}