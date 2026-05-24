import { describe, it, expect } from "vitest";
import { REFERENCE_PRESET } from "../src/presets/reference.js";

/**
 * The bundled calendar follows SFS 2004:629 (Lag om trängselskatt)
 * Bilaga 1 and SFS 1989:253 (Lag om allmänna helgdagar). See the
 * citation block in `src/calendar/swedish.ts`. These tests verify each
 * rule of the law, including the three day-before-holiday carve-outs
 * and the July first-five-weekdays carve-out — both areas where the
 * bundled Java/C# reference at the repo root deviates from the law.
 */

const cal = REFERENCE_PRESET.calendar;

const stockholmNoon = (year: number, month: number, day: number): Date =>
  // Noon Stockholm time is unambiguous regardless of DST.
  new Date(Date.UTC(year, month - 1, day, 10, 0, 0));

/**
 * Days listed by the Java/C# reference for 2013 that ARE also toll-free
 * per SFS 2004:629. The three days the Java reference incorrectly marked
 * free (Maundy Thursday, Ascension Eve, All Saints' Eve) live in their
 * own describe block below.
 */
const LAW_2013_HOLIDAYS: readonly (readonly [number, number])[] = [
  [1, 1], // Nyårsdagen — public holiday
  [3, 29], // Långfredagen — Good Friday
  [4, 1], // Annandag påsk — Easter Monday
  [4, 30], // Valborgsmässoafton — day before May 1
  [5, 1], // Första maj — public holiday
  [5, 9], // Kristi himmelsfärdsdag — Ascension Day
  [6, 5], // Dag före nationaldagen — day before Jun 6
  [6, 6], // Sveriges nationaldag — public holiday
  [6, 21], // Midsommarafton — day before Midsummer Day (Sat Jun 22)
  [12, 24], // Julafton — day before Christmas Day
  [12, 25], // Juldagen
  [12, 26], // Annandag jul
  [12, 31], // Nyårsafton — day before New Year's Day
];

describe("Swedish congestion-tax calendar — SFS 2004:629 fidelity (2013)", () => {
  for (const [month, day] of LAW_2013_HOLIDAYS) {
    it(`marks 2013-${String(month)}-${String(day)} as toll-free`, () => {
      expect(cal.isHoliday(stockholmNoon(2013, month, day))).toBe(true);
    });
  }

  it("marks every Saturday and Sunday in 2013 as toll-free", () => {
    let saturdays = 0;
    let sundays = 0;
    for (let m = 1; m <= 12; m++) {
      const daysInMonth = new Date(Date.UTC(2013, m, 0)).getUTCDate();
      for (let d = 1; d <= daysInMonth; d++) {
        const dow = new Date(Date.UTC(2013, m - 1, d)).getUTCDay();
        if (dow === 0 || dow === 6) {
          expect(cal.isHoliday(stockholmNoon(2013, m, d))).toBe(true);
          if (dow === 6) saturdays++;
          else sundays++;
        }
      }
    }
    expect(saturdays + sundays).toBe(104); // 52 of each in 2013
  });

  it("does NOT mark ordinary weekdays in 2013 (Feb 5, Mar 12, Sep 17, Oct 22, Nov 26, Dec 17)", () => {
    const nonHolidays: readonly (readonly [number, number])[] = [
      [2, 5],
      [3, 12],
      [9, 17],
      [10, 22],
      [11, 26],
      [12, 17],
    ];
    for (const [month, day] of nonHolidays) {
      expect(cal.isHoliday(stockholmNoon(2013, month, day))).toBe(false);
    }
  });
});

describe("SFS 2004:629 Bilaga 1 — three day-before-holiday carve-outs are TAXED", () => {
  // The Java/C# reference flags these as toll-free; the law explicitly
  // says they ARE charged. We follow the law.

  it("Maundy Thursday (day before Good Friday) is taxed", () => {
    expect(cal.isHoliday(stockholmNoon(2013, 3, 28))).toBe(false); // Mar 28 2013
    expect(cal.isHoliday(stockholmNoon(2025, 4, 17))).toBe(false); // Apr 17 2025
  });

  it("Ascension Eve (day before Ascension Day) is taxed", () => {
    expect(cal.isHoliday(stockholmNoon(2013, 5, 8))).toBe(false); // May 8 2013
    expect(cal.isHoliday(stockholmNoon(2025, 5, 28))).toBe(false); // May 28 2025
  });

  it("All Saints' Eve (Friday before All Saints' Day) is taxed", () => {
    // 2013: All Saints' Day = Sat Nov 2 → Eve = Fri Nov 1
    expect(cal.isHoliday(stockholmNoon(2013, 11, 1))).toBe(false);
    // 2025: All Saints' Day = Sat Nov 1 → Eve = Fri Oct 31
    expect(cal.isHoliday(stockholmNoon(2025, 10, 31))).toBe(false);
    // 2027: All Saints' Day = Sat Nov 6 → Eve = Fri Nov 5
    expect(cal.isHoliday(stockholmNoon(2027, 11, 5))).toBe(false);
  });
});

describe("SFS 2004:629 Bilaga 1 — July first-five-weekdays carve-out", () => {
  it("2013: Jul 1-5 (Mon-Fri) are taxed; Jul 8+ are free", () => {
    // Jul 1 Mon, 2 Tue, 3 Wed, 4 Thu, 5 Fri — all taxed.
    expect(cal.isHoliday(stockholmNoon(2013, 7, 1))).toBe(false);
    expect(cal.isHoliday(stockholmNoon(2013, 7, 2))).toBe(false);
    expect(cal.isHoliday(stockholmNoon(2013, 7, 3))).toBe(false);
    expect(cal.isHoliday(stockholmNoon(2013, 7, 4))).toBe(false);
    expect(cal.isHoliday(stockholmNoon(2013, 7, 5))).toBe(false);
    // Jul 6, 7 are weekend → free.
    expect(cal.isHoliday(stockholmNoon(2013, 7, 6))).toBe(true);
    expect(cal.isHoliday(stockholmNoon(2013, 7, 7))).toBe(true);
    // Jul 8 Mon onwards → free.
    expect(cal.isHoliday(stockholmNoon(2013, 7, 8))).toBe(true);
    expect(cal.isHoliday(stockholmNoon(2013, 7, 15))).toBe(true);
    expect(cal.isHoliday(stockholmNoon(2013, 7, 31))).toBe(true);
  });

  it("2025: skips Sat/Sun when counting the first 5 weekdays (Jul 1-4 + Jul 7)", () => {
    // Jul 1 Tue, 2 Wed, 3 Thu, 4 Fri → taxed (4 of the 5).
    expect(cal.isHoliday(stockholmNoon(2025, 7, 1))).toBe(false);
    expect(cal.isHoliday(stockholmNoon(2025, 7, 4))).toBe(false);
    // Jul 5, 6 weekend.
    expect(cal.isHoliday(stockholmNoon(2025, 7, 5))).toBe(true);
    // Jul 7 Mon → taxed (5th).
    expect(cal.isHoliday(stockholmNoon(2025, 7, 7))).toBe(false);
    // Jul 8 Tue → free.
    expect(cal.isHoliday(stockholmNoon(2025, 7, 8))).toBe(true);
  });

  it("2023: July starts on Saturday — first 5 weekdays are Jul 3-7", () => {
    // Jul 1 Sat, 2 Sun → weekend (free).
    expect(cal.isHoliday(stockholmNoon(2023, 7, 1))).toBe(true);
    expect(cal.isHoliday(stockholmNoon(2023, 7, 2))).toBe(true);
    // Jul 3 Mon, 4 Tue, 5 Wed, 6 Thu, 7 Fri → all 5 taxed.
    expect(cal.isHoliday(stockholmNoon(2023, 7, 3))).toBe(false);
    expect(cal.isHoliday(stockholmNoon(2023, 7, 7))).toBe(false);
    // Jul 10 Mon onwards → free.
    expect(cal.isHoliday(stockholmNoon(2023, 7, 10))).toBe(true);
  });
});

describe("Epiphany and its eve (SFS 1989:253 §2 holiday absent from Java reference)", () => {
  it("Epiphany (Jan 6) is toll-free on every weekday year", () => {
    // 2025-01-06 Monday → free (new vs Java).
    expect(cal.isHoliday(stockholmNoon(2025, 1, 6))).toBe(true);
    // 2026-01-06 Tuesday → free.
    expect(cal.isHoliday(stockholmNoon(2026, 1, 6))).toBe(true);
  });

  it("Epiphany Eve (Jan 5) is toll-free via day-before-holiday rule", () => {
    // 2026-01-05 Monday → free (day before Jan 6 Tuesday holiday).
    expect(cal.isHoliday(stockholmNoon(2026, 1, 5))).toBe(true);
    // 2027-01-05 Tuesday → free (day before Jan 6 Wednesday holiday).
    expect(cal.isHoliday(stockholmNoon(2027, 1, 5))).toBe(true);
  });
});

describe("Movable holidays — generalized across years", () => {
  it("derives Easter Monday correctly across years", () => {
    expect(cal.isHoliday(stockholmNoon(2024, 4, 1))).toBe(true); // 2024 Apr 1
    expect(cal.isHoliday(stockholmNoon(2025, 4, 21))).toBe(true); // 2025 Apr 21
  });

  it("derives Good Friday correctly across years", () => {
    expect(cal.isHoliday(stockholmNoon(2024, 3, 29))).toBe(true);
    expect(cal.isHoliday(stockholmNoon(2025, 4, 18))).toBe(true);
  });

  it("derives Ascension Day correctly across years (Easter + 39)", () => {
    expect(cal.isHoliday(stockholmNoon(2024, 5, 9))).toBe(true);
    expect(cal.isHoliday(stockholmNoon(2025, 5, 29))).toBe(true);
  });

  it("derives Midsummer Day (Saturday Jun 20-26) across years", () => {
    // 2024: Sat 22 Jun is already covered by weekend; also a helgdag.
    expect(cal.isHoliday(stockholmNoon(2024, 6, 22))).toBe(true);
    // 2025: Midsummer Day = Sat 21 Jun.
    expect(cal.isHoliday(stockholmNoon(2025, 6, 21))).toBe(true);
  });

  it("derives Midsummer Eve (Friday before Midsummer Day) as toll-free", () => {
    expect(cal.isHoliday(stockholmNoon(2024, 6, 21))).toBe(true); // Fri Jun 21
    expect(cal.isHoliday(stockholmNoon(2025, 6, 20))).toBe(true); // Fri Jun 20
  });

  it("derives All Saints' Day (Saturday Oct 31-Nov 6) across years", () => {
    // 2025: All Saints' Day = Sat Nov 1.
    expect(cal.isHoliday(stockholmNoon(2025, 11, 1))).toBe(true);
    // 2027: All Saints' Day = Sat Nov 6.
    expect(cal.isHoliday(stockholmNoon(2027, 11, 6))).toBe(true);
  });
});

describe("Day-before-holiday rule — implicit, not hardcoded", () => {
  it("flags Apr 30 as free across years (day before May 1)", () => {
    expect(cal.isHoliday(stockholmNoon(2024, 4, 30))).toBe(true); // Tue
    expect(cal.isHoliday(stockholmNoon(2025, 4, 30))).toBe(true); // Wed
  });

  it("flags Jun 5 as free across years (day before National Day)", () => {
    expect(cal.isHoliday(stockholmNoon(2024, 6, 5))).toBe(true); // Wed
    expect(cal.isHoliday(stockholmNoon(2025, 6, 5))).toBe(true); // Thu
  });

  it("flags Dec 24 as free across years (day before Christmas Day)", () => {
    expect(cal.isHoliday(stockholmNoon(2024, 12, 24))).toBe(true);
    expect(cal.isHoliday(stockholmNoon(2025, 12, 24))).toBe(true);
  });

  it("flags Dec 31 as free across years (day before New Year)", () => {
    expect(cal.isHoliday(stockholmNoon(2024, 12, 31))).toBe(true);
    expect(cal.isHoliday(stockholmNoon(2025, 12, 31))).toBe(true);
  });

  it("does NOT flag the day before a day-before-holiday (Dec 23, Apr 29, etc.)", () => {
    // Dec 24 is free (day before Dec 25), but Dec 23 is just a Tuesday.
    expect(cal.isHoliday(stockholmNoon(2024, 12, 23))).toBe(false); // Mon
    expect(cal.isHoliday(stockholmNoon(2025, 12, 23))).toBe(false); // Tue
    // Apr 30 is free, but Apr 29 is just a Monday/Tuesday.
    expect(cal.isHoliday(stockholmNoon(2024, 4, 29))).toBe(false); // Mon
    expect(cal.isHoliday(stockholmNoon(2025, 4, 29))).toBe(false); // Tue
  });

  it("does NOT generally flag Nov 1 (Java's quirk) when it is not All Saints' Eve", () => {
    // 2027: All Saints' Day = Sat Nov 6 → Eve = Fri Nov 5.
    // Nov 1 2027 = Monday — not the eve, not a helgdag, just a weekday.
    expect(cal.isHoliday(stockholmNoon(2027, 11, 1))).toBe(false);
  });
});
