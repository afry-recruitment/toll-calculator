import { holidays, holiMonths } from '../config/holidays.js';

export function isTollFreeDate(date) {
  if(!date) return null;
  if (isWeekend(date)) return true;
  for (const holiMonth of holiMonths) {
    if (isSameMonth(new Date(holiMonth), date)) return true;
  }

  for (const holiday of holidays) {
    if (isSameDay(new Date(holiday), date)) return true;
  }

  return false;
}

// 6 = Saturday, 0 = Sunday
export function isWeekend(date) {
  return (date) ? [0, 6].includes(date.getDay()) : null;
}

export function isSameMonth(a, b) {
  return (a,b) ? (a.getFullYear() === b.getFullYear() && a.getMonth() === b.getMonth()) : null;
}

export function isSameDay(a, b) {
  return (a, b) ? (
    a.getFullYear() === b.getFullYear() &&
    a.getMonth() === b.getMonth() &&
    a.getDate() === b.getDate()
  ) : null;
}
