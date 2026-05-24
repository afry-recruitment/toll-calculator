/**
 * A calendar that decides whether a given moment falls on a toll-free day.
 *
 * Implementations are free to consider weekends, fixed-date holidays,
 * movable feasts (Easter-derived), the day before a public holiday, or
 * any other rule the operating jurisdiction defines.
 *
 * The calculator treats holidays as a binary flag: if `isHoliday(date)`
 * returns `true`, the entire day is free regardless of the fee schedule.
 */
export interface HolidayCalendar {
  readonly isHoliday: (date: Date) => boolean;
}
