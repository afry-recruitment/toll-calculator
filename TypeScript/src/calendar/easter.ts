/**
 * Returns the Gregorian date of Easter Sunday for the given year,
 * computed via Gauss's algorithm with the 1807 and 1816 corrections.
 * Pure integer arithmetic, valid for any Gregorian year (1583 onward).
 *
 * Algorithm:
 *   Gauss, C. F. (1800). "Berechnung des Osterfestes."
 *   Monatliche Correspondenz zur Beförderung der Erd- und Himmelskunde,
 *   vol. 2, pp. 121–130.
 *
 * Transcribed from:
 *   https://es.wikipedia.org/wiki/Computus (consulted 2026-05-23)
 *
 * Test fixtures cross-checked against the U.S. Naval Observatory's
 * published table of Easter dates.
 */
export const easterSunday = (year: number): { month: number; day: number } => {
  const a = year % 19;
  const b = year % 4;
  const c = year % 7;

  const k = Math.floor(year / 100);
  const p = Math.floor((13 + 8 * k) / 25);
  const q = Math.floor(k / 4);
  const M = (15 - p + k - q) % 30;
  const N = (4 + k - q) % 7;

  const d = (19 * a + M) % 30;
  const e = (2 * b + 4 * c + 6 * d + N) % 7;

  let day = d + e + 22;
  let month = 3;
  if (day > 31) {
    day = d + e - 9;
    month = 4;
    if (day === 26) day = 19;
    if (day === 25 && d === 28 && e === 6 && a > 10) day = 18;
  }
  return { month, day };
};
