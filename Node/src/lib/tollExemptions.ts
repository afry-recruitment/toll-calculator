import { isPublicHoliday } from "swedish-holidays";
import type { VehicleType } from "../type/vehicle.js";

export const tollFreeVehicles: readonly VehicleType[] = [
  "Motorbike",
  "Tractor",
  "Emergency",
  "Diplomat",
  "Foreign",
  "Military",
];

// Sunday or Saturday
const isWeekend = (date: Date): boolean => {
  const day = date.getDay();
  return day === 0 || day === 6;
};

export const isWeekendOrHoliday = (date: Date): boolean => {
  return isWeekend(date) || isPublicHoliday(date) !== false;
};

export const IsTollFreeVehicle = (vehicle: VehicleType): boolean => {
  if (!vehicle) {
    return false;
  }
  return tollFreeVehicles.includes(vehicle);
};
