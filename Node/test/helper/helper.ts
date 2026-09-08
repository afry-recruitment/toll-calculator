import type { VehicleType } from "../../src/type/vehicle.js";

type AtTime = {
  time: string;
  day: number;
  month: number;
  year: number;
};
export const car: VehicleType = "Car";
export const motorBike: VehicleType = "Motorbike";

export const passageTime = ({ time, day, month, year }: AtTime): Date => {
  if (!day || !month || !year) {
    throw new Error("Missing date");
  }
  const [hour, minute] = time.split(":").map(Number);
  return new Date(year, month, day, hour, minute, 0);
};
