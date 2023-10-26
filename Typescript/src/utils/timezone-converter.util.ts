import { DateTime } from 'luxon';

export const convertToStockholmTime = (date: Date): Date => {
  return DateTime.fromJSDate(date).setZone('Europe/Stockholm').toJSDate();
};
// only for ensuring that the date is in the correct timezone - Stockholm/Sweden
