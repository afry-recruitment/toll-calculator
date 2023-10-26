import { convertToStockholmTime } from './timezone-converter.util';

// different fees for different times of the day - add more or change if needed
export const tollFeeTimes = [
  { start: '06:00', end: '06:29', fee: 8 },
  { start: '06:30', end: '06:59', fee: 13 },
  { start: '07:00', end: '07:59', fee: 18 },
  { start: '08:00', end: '08:29', fee: 13 },
  { start: '08:30', end: '14:59', fee: 8 },
  { start: '15:00', end: '15:29', fee: 13 },
  { start: '15:30', end: '16:59', fee: 18 },
  { start: '17:00', end: '17:59', fee: 13 },
  { start: '18:00', end: '18:29', fee: 8 },
  { start: '18:30', end: '05:59', fee: 0 },
];

// method for calculating toll fee for a specific time of the day.
export const tollFeeForTime = (date: Date): number => {
  date = convertToStockholmTime(date);
  const hour = date.getHours().toString().padStart(2, '0'); // for using eg. 06:05
  const minute = date.getMinutes().toString().padStart(2, '0'); // for using eg. 6:05
  const time = `${hour}:${minute}`;

  const tollFeeTime = tollFeeTimes.find((tollFeeTime) => {
    return tollFeeTime.start <= time && tollFeeTime.end >= time;
  });
  return tollFeeTime?.fee || 0;
};

// method for checking if there are multiple dates within the same hour
export const checkForSameHour = (dateTimes: Date[]): Date[] => {
  const uniqueDatesByHour: { [hourKey: string]: Date } = {};

  for (const dateTime of dateTimes) {
    const hourKey = `${dateTime.getFullYear()}-${dateTime.getMonth()}-${dateTime.getDate()}-${dateTime.getHours()}`;

    if (!uniqueDatesByHour[hourKey]) {
      uniqueDatesByHour[hourKey] = dateTime;
    }
  }
  return Object.values(uniqueDatesByHour);
};
