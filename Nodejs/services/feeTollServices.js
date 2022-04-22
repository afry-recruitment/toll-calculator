import { feeTimes } from '../config/feeTimes.js';
const TEMP_DATE = '2013-01-05T00:00:00Z';
export function getTollFee(date) {
  const passingTime = getPassingTime(date);
  for (const feeTime of feeTimes) {
    const fee = getFeeForTime(passingTime, feeTime);
    if (fee !== false) return fee;
  }
  return 0;
}

function getPassingTime(date) {
  const currentHour = date.getUTCHours();
  const currentMinute = date.getUTCMinutes();

  const passingTime = convertToTime(
    currentHour.toString(),
    currentMinute.toString()
  );
  return passingTime;
}

function getFeeForTime(passingTime, feeTime) {
  for (const interval of feeTime.intervals) {
    const [fromHour, fromMinute] = getTimeFromInterval(interval.from);
    const [toHour, toMinute] = getTimeFromInterval(interval.to);
    const fromTime = convertToTime(fromHour, fromMinute);
    const toTime = addToTime(
      fromTime,
      Math.abs(fromHour - toHour),
      Math.abs(fromMinute - toMinute)
    );
    if (isBetweenIntervals(passingTime, fromTime, toTime))
      return feeTime.amount;
  }
  return false;
}

function addToTime(time, hour, minute) {
  const tempDate = new Date(time);
  tempDate.setTime(
    tempDate.getTime() + hour * 60 * 60 * 1000 + minute * 60 * 1000
  );
  return tempDate;
}
function convertToTime(hour, minute) {
  const tempDate = new Date(TEMP_DATE);
  tempDate.setHours(hour);
  tempDate.setMinutes(minute);
  const userTimezoneOffset = tempDate.getTimezoneOffset() * 60000;
  const actualDate = new Date(tempDate.getTime() - userTimezoneOffset);
  return actualDate;
}

function getTimeFromInterval(interval) {
  const time = interval.split(':');
  return [time[0], time[1]];
}

export function isBetweenIntervals(date, min, max) {
  return date.getTime() >= min.getTime() && date.getTime() <= max.getTime();
}
