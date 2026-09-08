import { calculateOneHourChargeWindowFees } from "./lib/chargeWindow.js";
import { MAX_TOTAL_FEE } from "./lib/tollFeeSchedule.js";

import type { VehicleType } from "./type/vehicle.js";

type Props = {
  vehicle: VehicleType;
  dates: Date[];
};
/**
 * Calculate the total toll fee for one day
 *
 * @param vehicle - the vehicle
 * @param dates   - date and time of all passes on one day
 * @return - the total toll fee for that day
 */

export const GetTollFeeForDates = ({ vehicle, dates }: Props): number => {
  const sortedDates = [...dates].sort((dateA, dateB) => dateA.getTime() - dateB.getTime());
  const totalFee = calculateOneHourChargeWindowFees({ vehicle, dates: sortedDates });

  return Math.min(totalFee, MAX_TOTAL_FEE);
};
