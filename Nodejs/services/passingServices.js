import { limits } from '../config/settings.js';
import { isBetweenIntervals } from './feeTollServices.js';

export function isMultiplePassage(currentDate, latestPassingTime) {
  if (latestPassageTime === undefined) return false;

  const minTime = new Date(latestPassingTime);
  const maxTime = new Date(
    minTime.setMinutes(minTime.getMinutes() + limits.multipleVehicleInterval)
  );

  return isBetweenIntervals(currentDate, minTime, maxTime);
}
