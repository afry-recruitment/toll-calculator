import { convertToStockholmTime } from './timezone-converter.util';

// toll free dates for 2023 - add more or change if needed
export const tollFreeDatesFor2023 = [
  { month: 1, days: [1] },
  { month: 3, days: [28, 29] },
  { month: 4, days: [1, 30] },
  { month: 5, days: [1, 8, 9] },
  { month: 6, days: [5, 6, 21] },
  { month: 7, days: Array.from({ length: 31 }, (_, i) => i + 1) },
  { month: 11, days: [1] },
  { month: 12, days: [24, 25, 26, 31] },
];

// method for checking if a date is a weekend
export const isWeekend = (date: Date): boolean => {
  const dayOfWeek = date.getDay();
  return dayOfWeek === 0 || dayOfWeek === 6;
};

// method for checking if a date is a toll free date
export function isTollFreeDate(date: Date): boolean {
  date = convertToStockholmTime(date);
  if (isWeekend(date)) {
    return true;
  }

  const month = date.getMonth() + 1;
  const dayOfMonth = date.getDate();

  return tollFreeDatesFor2023.some((tollFreeDate) => {
    if (tollFreeDate.month !== month) return false;
    return tollFreeDate.days.includes(dayOfMonth);
  });
}
