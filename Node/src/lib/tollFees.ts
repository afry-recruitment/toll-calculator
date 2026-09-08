import type { VehicleType } from "../type/vehicle.js";
import { formatTime } from "./date.js";
import { IsTollFreeVehicle, isWeekendOrHoliday } from "./tollExemptions.js";
import { tollFeeSchedule } from "./tollFeeSchedule.js";

export const GetTollFee = (date: Date, vehicle: VehicleType): number => {
  if (isWeekendOrHoliday(date) || IsTollFreeVehicle(vehicle)) {
    return 0;
  }
  const time = formatTime(date);
  const slot = tollFeeSchedule.find(({ start, end }) => time >= start && time <= end);

  if (!slot) {
    return 0;
  }
  return slot.fee;
};
