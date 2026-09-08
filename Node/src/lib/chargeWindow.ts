import type { VehicleType } from "../type/vehicle.js";
import { isWithinOneHour } from "./date.js";
import { GetTollFee } from "./tollFees.js";
type TollState = {
  totalFee: number;
  oneHourChargeWindowStart: Date;
  highestFeeInOneHourChargeWindow: number;
};

type Props = {
  vehicle: VehicleType;
  dates: Date[];
};
export const calculateOneHourChargeWindowFees = ({ vehicle, dates }: Props): number => {
  const firstDate = dates[0];
  if (!firstDate) {
    return 0;
  }

  let state: TollState = {
    totalFee: 0,
    oneHourChargeWindowStart: firstDate,
    highestFeeInOneHourChargeWindow: 0,
  };

  for (const date of dates) {
    const fee = GetTollFee(date, vehicle);

    if (!isWithinOneHour(state.oneHourChargeWindowStart, date)) {
      state = {
        totalFee: state.totalFee + state.highestFeeInOneHourChargeWindow,
        oneHourChargeWindowStart: date,
        highestFeeInOneHourChargeWindow: 0,
      };
    }

    state.highestFeeInOneHourChargeWindow = Math.max(state.highestFeeInOneHourChargeWindow, fee);
  }

  return state.totalFee + state.highestFeeInOneHourChargeWindow;
};
