const DAY_MS = 24 * 60 * 60 * 1000;

const holidayCache = new Map<number, ReadonlySet<string>>();

function toUtcDate(isoDate: string): Date {
  return new Date(`${isoDate}T00:00:00Z`);
}

function toIsoDate(date: Date): string {
  return date.toISOString().slice(0, 10);
}

function addDays(date: Date, days: number): Date {
  return new Date(date.getTime() + days * DAY_MS);
}

function nextWeekday(from: Date, weekday: number): Date {
  return addDays(from, (weekday - from.getUTCDay() + 7) % 7);
}

function easterSunday(year: number): Date {
  const a = year % 19;
  const b = Math.floor(year / 100);
  const c = year % 100;
  const d = Math.floor(b / 4);
  const e = b % 4;
  const f = Math.floor((b + 8) / 25);
  const g = Math.floor((b - f + 1) / 3);
  const h = (19 * a + b - d - g + 15) % 30;
  const i = Math.floor(c / 4);
  const k = c % 4;
  const l = (32 + 2 * e + 2 * i - h - k) % 7;
  const m = Math.floor((a + 11 * h + 22 * l) / 451);
  const month = Math.floor((h + l - 7 * m + 114) / 31);
  const day = ((h + l - 7 * m + 114) % 31) + 1;
  return new Date(Date.UTC(year, month - 1, day));
}

function publicHolidays(year: number): ReadonlySet<string> {
  const cached = holidayCache.get(year);
  if (cached) return cached;

  const fixed = (month: number, day: number) =>
    new Date(Date.UTC(year, month - 1, day));
  const easter = easterSunday(year);
  const SATURDAY = 6;

  const holidays = new Set(
    [
      fixed(1, 1),
      fixed(1, 6),
      addDays(easter, -2),
      easter,
      addDays(easter, 1),
      fixed(5, 1),
      addDays(easter, 39),
      addDays(easter, 49),
      fixed(6, 6),
      nextWeekday(fixed(6, 20), SATURDAY),
      nextWeekday(fixed(10, 31), SATURDAY),
      fixed(12, 25),
      fixed(12, 26),
    ].map(toIsoDate),
  );
  holidayCache.set(year, holidays);
  return holidays;
}

export function isPublicHoliday(isoDate: string): boolean {
  return publicHolidays(Number(isoDate.slice(0, 4))).has(isoDate);
}

export function isTollFreeDate(isoDate: string): boolean {
  const date = toUtcDate(isoDate);
  const weekday = date.getUTCDay();
  if (weekday === 0 || weekday === 6) return true;
  if (date.getUTCMonth() === 6) return true;
  return (
    isPublicHoliday(isoDate) || isPublicHoliday(toIsoDate(addDays(date, 1)))
  );
}
