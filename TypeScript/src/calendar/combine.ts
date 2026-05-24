import type { HolidayCalendar } from "./index.js";

/**
 * Compose multiple calendars into one. The resulting calendar reports a
 * day as a holiday if *any* of the inputs does.
 *
 * Useful for stacking sources of holiday data — e.g. a national calendar
 * plus a company-specific calendar plus a local exception list — without
 * each caller re-implementing the union logic.
 *
 * @example
 * const calendar = combineCalendars(nationalHolidays, companyHolidays);
 */
export const combineCalendars = (
  ...calendars: readonly HolidayCalendar[]
): HolidayCalendar => ({
  isHoliday: (date) => calendars.some((c) => c.isHoliday(date)),
});
