import { Temporal } from "@js-temporal/polyfill";
import type { HolidayCalendar } from "./index.js";
import { easterSunday } from "./easter.js";
import { parts } from "../time/zoned-time.js";

/**
 * Swedish congestion-tax (trängselskatt) calendar — generalized to any
 * Gregorian year by deriving the toll-free-day rules from primary Swedish
 * legislation rather than reproducing a hard-coded year list.
 *
 * ## Legal sources
 *
 * - **SFS 2004:629** — *Lag om trängselskatt* ("Law on congestion tax"),
 *   Bilaga 1 (the Stockholm-specific schedule). The relevant clause:
 *
 *   > "Skatt tas inte ut för fordon på lördagar, dag före söndag,
 *   > helgdagar, dag före helgdag samt under juli månad … Skatt ska tas
 *   > ut för dag före långfredagen, dag före Kristi himmelsfärdsdag, och
 *   > dag före alla helgons dag … Under juli månad ska ingen skatt tas
 *   > ut förutom för de fem första vardagarna utom lördag i den månaden."
 *
 *   English (informal): tax is *not* charged on Saturdays, the day before
 *   Sunday, public holidays, days before public holidays, and during the
 *   month of July — *except* that tax IS charged on the day before Good
 *   Friday (Maundy Thursday), the day before Ascension Day (Ascension
 *   Eve), the day before All Saints' Day (All Saints' Eve), and on the
 *   first five non-Saturday weekdays of July.
 *
 *   Source: https://www.riksdagen.se/sv/dokument-och-lagar/dokument/svensk-forfattningssamling/lag-2004629-om-trangselskatt_sfs-2004-629/
 *
 * - **SFS 1989:253** — *Lag om allmänna helgdagar* ("Law on general
 *   public holidays"), §2, which enumerates the *helgdagar* referenced by
 *   the previous law. Post-2005 reform: Sveriges nationaldag (June 6)
 *   was added to the list; Annandag pingst (Whit Monday) was removed.
 *
 *   Source: https://www.riksdagen.se/sv/dokument-och-lagar/dokument/svensk-forfattningssamling/lag-1989253-om-allmanna-helgdagar_sfs-1989-253/
 *
 * ## Algorithm
 *
 * For an instant interpreted in `Europe/Stockholm`, `isHoliday` returns
 * `true` iff any of these holds (evaluated in order):
 *
 *   1. **Weekend.** Saturday or Sunday (ISO weekday ≥ 6).
 *   2. **Public holiday** per SFS 1989:253 §2.
 *   3. **July**, with the exception of the first five non-Saturday
 *      weekdays of the month (SFS 2004:629 Bilaga 1 carve-out).
 *   4. **Day before a public holiday**, unless the day is one of the
 *      three explicitly taxed days listed in SFS 2004:629 Bilaga 1:
 *      Maundy Thursday, Ascension Eve, or All Saints' Eve.
 *
 * ## Divergence from the bundled Java/C# reference
 *
 * The reference at the repo root hard-codes the 2013 list of toll-free
 * days. That list flags Maundy Thursday, Ascension Eve, and Nov 1 (All
 * Saints' Eve 2013) as free — the *opposite* of what SFS 2004:629
 * mandates. It also marks the entire month of July as free, ignoring
 * the first-five-weekdays carve-out. This implementation follows the
 * law; see `docs/discovery/04-bugs.md` §4.6 for the trace.
 *
 * Per-year computations (Easter-derived dates, Midsummer Day, All Saints'
 * Day, July taxed days) are memoized so a calculator invoked across many
 * passes pays the cost once per distinct year.
 */

const STOCKHOLM = "Europe/Stockholm";

/** Fixed-date public holidays per SFS 1989:253 §2 (M-D, no zero padding). */
const FIXED_PUBLIC_HOLIDAYS: ReadonlySet<string> = new Set([
  "1-1", // Nyårsdagen — New Year's Day
  "1-6", // Trettondedag jul — Epiphany
  "5-1", // Första maj — Workers' Day
  "6-6", // Sveriges nationaldag — National Day (since 2005)
  "12-25", // Juldagen — Christmas Day
  "12-26", // Annandag jul — Boxing Day
]);

/** Day offsets from Easter Sunday for the Easter-derived helgdagar of SFS 1989:253 §2. */
const EASTER_PUBLIC_HOLIDAY_OFFSETS: readonly number[] = [
  -2, // Långfredagen — Good Friday
  0, // Påskdagen — Easter Sunday
  1, // Annandag påsk — Easter Monday
  39, // Kristi himmelsfärdsdag — Ascension Day
  49, // Pingstdagen — Whit Sunday
];

/**
 * Day offsets from Easter Sunday for the SFS 2004:629 Bilaga 1
 * day-before-holiday carve-outs (Maundy Thursday, Ascension Eve). The
 * third carve-out, All Saints' Eve, is the Friday immediately before
 * All Saints' Day and is handled separately because All Saints' Day is
 * itself movable.
 */
const TAXED_DAY_BEFORE_OFFSETS_FROM_EASTER: readonly number[] = [
  -3, // Skärtorsdagen — Maundy Thursday (day before Good Friday)
  38, // Kristi himmelsfärds afton — Ascension Eve (day before Ascension Day)
];

interface EasterDerived {
  readonly publicHolidays: ReadonlySet<string>;
  readonly taxedDayBefore: ReadonlySet<string>;
}

const computeEasterDerived = (year: number): EasterDerived => {
  const e = easterSunday(year);
  const sunday = Temporal.PlainDate.from({ year, month: e.month, day: e.day });

  const publicHolidays = new Set<string>();
  for (const offset of EASTER_PUBLIC_HOLIDAY_OFFSETS) {
    const d = sunday.add({ days: offset });
    publicHolidays.add(`${String(d.month)}-${String(d.day)}`);
  }

  const taxedDayBefore = new Set<string>();
  for (const offset of TAXED_DAY_BEFORE_OFFSETS_FROM_EASTER) {
    const d = sunday.add({ days: offset });
    taxedDayBefore.add(`${String(d.month)}-${String(d.day)}`);
  }

  return { publicHolidays, taxedDayBefore };
};

interface MovableSaturday {
  readonly month: number;
  readonly day: number;
}

/** Midsummer Day: Saturday between June 20 and June 26 (SFS 1989:253 §2). */
const computeMidsummerDay = (year: number): MovableSaturday => {
  for (let day = 20; day <= 26; day++) {
    const d = Temporal.PlainDate.from({ year, month: 6, day });
    if (d.dayOfWeek === 6) return { month: 6, day };
  }
  /* v8 ignore next 2 -- unreachable: any 7-day window contains exactly one Saturday */
  throw new Error("unreachable: Jun 20-26 contains exactly one Saturday");
};

/** All Saints' Day: Saturday between October 31 and November 6 (SFS 1989:253 §2). */
const computeAllSaintsDay = (year: number): MovableSaturday => {
  let d = Temporal.PlainDate.from({ year, month: 10, day: 31 });
  for (let i = 0; i < 7; i++) {
    if (d.dayOfWeek === 6) return { month: d.month, day: d.day };
    d = d.add({ days: 1 });
  }
  /* v8 ignore next 2 -- unreachable: Oct 31-Nov 6 contains exactly one Saturday */
  throw new Error("unreachable: Oct 31-Nov 6 contains exactly one Saturday");
};

/**
 * The first five non-Saturday weekdays (Mon–Fri) of July, per SFS
 * 2004:629 Bilaga 1. These days *are* taxed despite July being otherwise
 * toll-free.
 */
const computeJulyTaxedDays = (year: number): ReadonlySet<number> => {
  const days = new Set<number>();
  let d = Temporal.PlainDate.from({ year, month: 7, day: 1 });
  while (days.size < 5) {
    if (d.dayOfWeek >= 1 && d.dayOfWeek <= 5) {
      days.add(d.day);
    }
    d = d.add({ days: 1 });
  }
  return days;
};

const memo = <T>(compute: (year: number) => T): ((year: number) => T) => {
  const cache = new Map<number, T>();
  return (year) => {
    let value = cache.get(year);
    if (value === undefined) {
      value = compute(year);
      cache.set(year, value);
    }
    return value;
  };
};

export const createSwedishCongestionTaxCalendar = (): HolidayCalendar => {
  const easterFor = memo(computeEasterDerived);
  const midsummerFor = memo(computeMidsummerDay);
  const allSaintsFor = memo(computeAllSaintsDay);
  const julyTaxedFor = memo(computeJulyTaxedDays);

  const isPublicHoliday = (
    year: number,
    month: number,
    day: number,
  ): boolean => {
    const md = `${String(month)}-${String(day)}`;
    if (FIXED_PUBLIC_HOLIDAYS.has(md)) return true;
    if (easterFor(year).publicHolidays.has(md)) return true;
    const mid = midsummerFor(year);
    if (month === mid.month && day === mid.day) return true;
    const asd = allSaintsFor(year);
    if (month === asd.month && day === asd.day) return true;
    return false;
  };

  const isCarveOutTaxed = (
    year: number,
    month: number,
    day: number,
  ): boolean => {
    const md = `${String(month)}-${String(day)}`;
    if (easterFor(year).taxedDayBefore.has(md)) return true;
    const asd = allSaintsFor(year);
    const eve = Temporal.PlainDate.from({
      year,
      month: asd.month,
      day: asd.day,
    }).subtract({ days: 1 });
    if (month === eve.month && day === eve.day) return true;
    return false;
  };

  const isHoliday = (date: Date): boolean => {
    const p = parts(date, STOCKHOLM);

    // 1. Weekend.
    if (p.weekday >= 6) return true;

    // 2. Public holiday per SFS 1989:253.
    if (isPublicHoliday(p.year, p.month, p.day)) return true;

    // 3. July: toll-free EXCEPT first five non-Saturday weekdays.
    if (p.month === 7) {
      const taxed = julyTaxedFor(p.year);
      return !taxed.has(p.day);
    }

    // 4. Day before a public holiday — unless it is one of the three
    //    carve-outs explicitly taxed by SFS 2004:629 Bilaga 1.
    if (isCarveOutTaxed(p.year, p.month, p.day)) return false;

    const tomorrow = Temporal.PlainDate.from({
      year: p.year,
      month: p.month,
      day: p.day,
    }).add({ days: 1 });
    if (isPublicHoliday(tomorrow.year, tomorrow.month, tomorrow.day)) {
      return true;
    }

    return false;
  };

  return { isHoliday };
};
