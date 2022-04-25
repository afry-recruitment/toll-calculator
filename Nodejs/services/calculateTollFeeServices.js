import { isMultiplePassage } from './passingServices.js';
import { limits } from '../config/settings.js';
import {isTollFreeDate} from './dateServices.js';
import { getTollFee } from './feeTollServices.js';

export function calculateTollFee(vehicleInstance, ...dates) {
  if (vehicleInstance.isTollFree()) return 0;
  const sortedDates = dates.sort((a, b) => a - b);

  let totalFee = 0;
  let latestPassageTime = undefined;
  let pendingFees = [];
  for (const date of sortedDates) {
    if (isTollFreeDate(date)) continue;

    let fee = getTollFee(date);

    if (isMultiplePassage(date, latestPassageTime)) {
      pendingFees.push(fee);
    } else {
      totalFee += getHighestFee(pendingFees);
      latestPassageTime = date.getTime();

      pendingFees = [];
      pendingFees.push(fee);
    }
  }

  totalFee += getHighestFee(pendingFees);
  if (totalFee >= limits.maxPricePerDay) return limits.maxPricePerDay;

  return totalFee;
}
function getHighestFee(pendingFees) {
  if (pendingFees.length >= 1) {
    return Math.max(...pendingFees);
  }

  return 0;
}
