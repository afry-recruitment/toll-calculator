import { holidays, holiMonths } from '../config/holidays.js';
export default class dateServices {
  constructor() {}

  isTollFreeDate(date) {
    if (this.isWeekend(date)) return true;
    for (const holiMonth of holiMonths) {
      if (this.isSameMonth(new Date(holiMonth), date)) return true;
    }

    for (const holiday of holidays) {
      if (this.isSameDay(new Date(holiday), date)) return true;
    }

    return false;
  }

  // 6 = Saturday, 0 = Sunday
  isWeekend(date) {
    return [0, 6].includes(date.getDay());
  }

  isSameMonth(a, b) {
    return a.getFullYear() === b.getFullYear() && b.getMonth() === b.getMonth();
  }

  isSameDay(a, b) {
    return (
      a.getFullYear() === b.getFullYear() &&
      a.getMonth() === b.getMonth() &&
      a.getDate() === b.getDate()
    );
  }
}
