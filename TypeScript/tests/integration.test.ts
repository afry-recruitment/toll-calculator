import { describe, it, expect } from "vitest";
import { createTollCalculator, REFERENCE_PRESET } from "../src/index.js";
import type { Vehicle } from "../src/index.js";

/**
 * Integration tests — exercise the public API through `createTollCalculator`
 * configured with the bundled `REFERENCE_PRESET`. These tests prove that
 * the preset's schedule, holiday calendar, time zone and daily cap are
 * wired together correctly when consumed as documented.
 *
 * Unit tests (see `docs/plan/05-testing-strategy.md`) cover the internals
 * of each module in isolation. These three tests cover the wiring — each
 * exercises a distinct path through the calculator.
 */

describe("integration: createTollCalculator + REFERENCE_PRESET", () => {
  it("charges 57 across 6 passes on a regular weekday (windowing + multi-band, below cap)", () => {
    const calc = createTollCalculator(REFERENCE_PRESET);
    const car: Vehicle = { type: "Car" };

    // Wednesday 2025-09-17, interpreted in REFERENCE_PRESET.timeZone
    // ("Europe/Stockholm" → CEST = UTC+2 on this date).
    // Chosen because: no holiday in the reference calendar in September, no
    // day-before-holiday, DST already settled in spring and not flipping
    // until late October, and no proximity to Easter (movable).
    const passes = [
      new Date("2025-09-17T06:25:00+02:00"), //  8 — band 06:00–06:29
      new Date("2025-09-17T06:50:00+02:00"), // 13 — same 60-min window as #1
      new Date("2025-09-17T07:30:00+02:00"), // 18 — new window (Δ from #1 = 65 min)
      new Date("2025-09-17T12:15:00+02:00"), //  8 — midday off-peak, new window
      new Date("2025-09-17T16:00:00+02:00"), // 18 — evening rush, new window
      new Date("2025-09-17T16:45:00+02:00"), // 18 — same window as #5
    ];

    // Hand-calculation of the windowing algorithm:
    //   window 1 (anchor 06:25): max(8, 13)   = 13
    //   window 2 (anchor 07:30): 18           = 18
    //   window 3 (anchor 12:15): 8            =  8
    //   window 4 (anchor 16:00): max(18, 18)  = 18
    //                                   total = 57   (below 60 cap)
    expect(calc.feeFor(car, passes)).toBe(57);
  });

  it("returns 0 for a Car on a holiday recognized by the bundled calendar", () => {
    const calc = createTollCalculator(REFERENCE_PRESET);
    const car: Vehicle = { type: "Car" };

    // Easter Sunday 2025 is April 20 — a movable feast. Proves the preset's
    // calendar resolves Easter dynamically (Anonymous Gregorian algorithm),
    // not via a hard-coded year-specific list.
    const passes = [
      new Date("2025-04-20T07:30:00+02:00"), // would be 18 on a weekday
      new Date("2025-04-20T08:15:00+02:00"), // would be 13 on a weekday
      new Date("2025-04-20T16:00:00+02:00"), // would be 18 on a weekday
    ];

    expect(calc.feeFor(car, passes)).toBe(0);
  });

  it("mixes free hours and paid hours, below the cap", () => {
    const calc = createTollCalculator(REFERENCE_PRESET);
    const car: Vehicle = { type: "Car" };

    // A single weekday with passes spread across hours that are both
    // inside fee bands (paid) and outside them (free — before 06:00,
    // midday "lull" days off, after 18:29).
    //
    //   W1 (anchor 05:00): max(0, 0)   = 0   — pre-band, two passes in window
    //   W2 (anchor 07:30): max(18, 13) = 18  — peak + same-window 08:00 band
    //   W3 (anchor 13:00): 8           = 8   — midday band
    //   W4 (anchor 20:00): 0           = 0   — after 18:29, post-band
    //                                  ----
    //                            total = 26  — below the 60 cap
    const passes = [
      new Date("2025-09-17T05:00:00+02:00"), //  0  — before 06:00
      new Date("2025-09-17T05:30:00+02:00"), //  0  — still pre-band, in W1
      new Date("2025-09-17T07:30:00+02:00"), // 18  — peak, new window
      new Date("2025-09-17T08:00:00+02:00"), // 13  — same window as 07:30
      new Date("2025-09-17T13:00:00+02:00"), //  8  — midday off-peak
      new Date("2025-09-17T20:00:00+02:00"), //  0  — after 18:29
    ];

    expect(calc.feeFor(car, passes)).toBe(26);
  });

  it("clamps the total to dailyCap (60 SEK) when the uncapped sum exceeds it", () => {
    const calc = createTollCalculator(REFERENCE_PRESET);
    const car: Vehicle = { type: "Car" };

    // Five separate 60-min windows on a single weekday. Each pass lands
    // in a different window because consecutive passes are 65 min apart,
    // so the windowing algorithm cannot consolidate them.
    //
    //   W1 (anchor 07:00): 18  — peak 07:00–07:59
    //   W2 (anchor 08:05): 13  — band 08:00–08:29
    //   W3 (anchor 15:30): 18  — peak 15:30–16:59
    //   W4 (anchor 16:35): 18  — still inside 15:30–16:59
    //   W5 (anchor 17:40): 13  — band 17:00–17:59
    //                     ----
    //         uncapped:    80   →  clamped to dailyCap = 60
    const passes = [
      new Date("2025-09-17T07:00:00+02:00"),
      new Date("2025-09-17T08:05:00+02:00"),
      new Date("2025-09-17T15:30:00+02:00"),
      new Date("2025-09-17T16:35:00+02:00"),
      new Date("2025-09-17T17:40:00+02:00"),
    ];

    expect(calc.feeFor(car, passes)).toBe(60);
  });

  it("returns 0 on a Saturday regardless of time (weekend rule)", () => {
    const calc = createTollCalculator(REFERENCE_PRESET);
    const car: Vehicle = { type: "Car" };

    // Saturday 2025-09-13: no public holiday in September, not adjacent to
    // Easter or any movable feast. Pure weekend rule. Three passes that
    // would each be a paid band on a weekday must all resolve to 0.
    const passes = [
      new Date("2025-09-13T07:30:00+02:00"), // would be 18 on a weekday
      new Date("2025-09-13T08:15:00+02:00"), // would be 13
      new Date("2025-09-13T16:00:00+02:00"), // would be 18
    ];

    expect(calc.feeFor(car, passes)).toBe(0);
  });

  it("returns 0 on the day before a public holiday (Christmas Eve)", () => {
    const calc = createTollCalculator(REFERENCE_PRESET);
    const car: Vehicle = { type: "Car" };

    // Wednesday 2025-12-24 — eve of Christmas Day (Dec 25, a fixed
    // helgdag per SFS 1989:253 §2). Not one of the three SFS 2004:629
    // Bilaga 1 carve-outs (Maundy Thursday, Ascension Eve, All Saints'
    // Eve), so the day-before-holiday rule applies and every pass must
    // be free. Offset is +01:00 because Stockholm is on CET in December.
    const passes = [
      new Date("2025-12-24T07:30:00+01:00"), // would be 18 on a regular weekday
      new Date("2025-12-24T16:00:00+01:00"), // would be 18
    ];

    expect(calc.feeFor(car, passes)).toBe(0);
  });

  it("applies the July clause: first five weekdays taxed, rest of month free", () => {
    const calc = createTollCalculator(REFERENCE_PRESET);
    const car: Vehicle = { type: "Car" };

    // SFS 2004:629 Bilaga 1: July is toll-free EXCEPT for the first five
    // non-Saturday weekdays. In 2025 Jul 1 is a Tuesday, so those five
    // days are Jul 1, 2, 3, 4 (Tue–Fri) and Jul 7 (Mon, skipping the
    // Saturday Jul 5). Same wall-clock peak-hour pass on a "taxed" early
    // weekday vs. a "free" mid-month weekday must resolve to different
    // fees — the difference proves the carve-out is wired through the
    // calendar to the calculator.
    const taxedEarlyJuly = calc.feeFor(car, [
      new Date("2025-07-03T07:30:00+02:00"), // Thursday, third taxed weekday
    ]);
    const freeMidJuly = calc.feeFor(car, [
      new Date("2025-07-15T07:30:00+02:00"), // Tuesday, past the carve-out
    ]);

    expect(taxedEarlyJuly).toBe(18);
    expect(freeMidJuly).toBe(0);
  });

  it("sorts passes internally regardless of input order", () => {
    const calc = createTollCalculator(REFERENCE_PRESET);
    const car: Vehicle = { type: "Car" };

    // Public contract: "Pass order is not significant; the calculator
    // sorts internally and does not mutate the caller's array." Same
    // three passes in three different orders must yield the same total.
    //
    //   sorted: 06:25 (8) → 06:50 (13) → 07:30 (18)
    //   W1 (anchor 06:25): max(8, 13) = 13
    //   W2 (anchor 07:30): 18
    //                total = 31
    const a = new Date("2025-09-17T06:25:00+02:00");
    const b = new Date("2025-09-17T06:50:00+02:00");
    const c = new Date("2025-09-17T07:30:00+02:00");

    expect(calc.feeFor(car, [a, b, c])).toBe(31);
    expect(calc.feeFor(car, [c, a, b])).toBe(31);
    expect(calc.feeFor(car, [c, b, a])).toBe(31);
  });

  it("interprets wall-clock times in the configured time zone across DST transitions", () => {
    const calc = createTollCalculator(REFERENCE_PRESET);
    const car: Vehicle = { type: "Car" };

    // The schedule's bands are wall-clock times in Europe/Stockholm.
    // Stockholm flips CET (UTC+1) → CEST (UTC+2) on the last Sunday of
    // March, so the same wall-clock instant has different UTC encodings
    // across the year. Two passes encoded as UTC `Z` timestamps that
    // *both* resolve to 07:30 Stockholm wall-clock must both fall in the
    // 07:00–07:59 = 18 SEK band. If the calculator were using UTC or a
    // fixed offset, the winter case would land outside the band.
    //
    //   Mon 2025-01-13 07:30 Stockholm CET  = 06:30 UTC
    //   Mon 2025-04-28 07:30 Stockholm CEST = 05:30 UTC
    const winter = new Date("2025-01-13T06:30:00Z");
    const summer = new Date("2025-04-28T05:30:00Z");

    expect(calc.feeFor(car, [winter])).toBe(18);
    expect(calc.feeFor(car, [summer])).toBe(18);
  });

  it("returns 0 for a Car with no passes (empty input is the identity case)", () => {
    const calc = createTollCalculator(REFERENCE_PRESET);
    const car: Vehicle = { type: "Car" };

    expect(calc.feeFor(car, [])).toBe(0);
  });

  it("throws TypeError when vehicle is null or undefined", () => {
    const calc = createTollCalculator(REFERENCE_PRESET);

    expect(() => calc.feeFor(null as unknown as Vehicle, [])).toThrow(
      TypeError,
    );
    expect(() => calc.feeFor(undefined as unknown as Vehicle, [])).toThrow(
      /is required/,
    );
  });

  it("throws TypeError when vehicle is not an object with a `type` property", () => {
    const calc = createTollCalculator(REFERENCE_PRESET);

    expect(() => calc.feeFor("Car" as unknown as Vehicle, [])).toThrow(
      /must be an object/,
    );
    expect(() => calc.feeFor({} as unknown as Vehicle, [])).toThrow(
      /must be an object/,
    );
  });

  it("throws TypeError when passes is not an array", () => {
    const calc = createTollCalculator(REFERENCE_PRESET);
    const car: Vehicle = { type: "Car" };

    expect(() =>
      calc.feeFor(car, "not an array" as unknown as readonly Date[]),
    ).toThrow(/must be an array/);
  });

  it("throws RangeError when any pass is not a valid Date", () => {
    const calc = createTollCalculator(REFERENCE_PRESET);
    const car: Vehicle = { type: "Car" };

    // new Date("not a date") returns a Date whose getTime() is NaN — TS
    // types it as Date but it's invalid at runtime.
    expect(() => calc.feeFor(car, [new Date("not a date")])).toThrow(
      RangeError,
    );

    // null inside the array — bypasses TS, real-world JSON.parse output.
    expect(() =>
      calc.feeFor(car, [
        new Date("2025-09-17T07:00:00+02:00"),
        null as unknown as Date,
      ]),
    ).toThrow(/passes\[1\]/);

    // String where a Date is expected — typical post-JSON.parse mistake.
    expect(() =>
      calc.feeFor(car, ["2025-09-17T07:00:00+02:00" as unknown as Date]),
    ).toThrow(/passes\[0\]/);
  });

  it("returns 0 for a toll-free vehicle regardless of time or date", () => {
    const calc = createTollCalculator(REFERENCE_PRESET);
    const motorbike: Vehicle = { type: "Motorbike" };

    // Same passes as the commuter day above — would be 57 for a Car. For a
    // toll-free vehicle the result must be 0 regardless of times or dates.
    const passes = [
      new Date("2025-09-17T06:25:00+02:00"),
      new Date("2025-09-17T06:50:00+02:00"),
      new Date("2025-09-17T07:30:00+02:00"),
      new Date("2025-09-17T12:15:00+02:00"),
      new Date("2025-09-17T16:00:00+02:00"),
      new Date("2025-09-17T16:45:00+02:00"),
    ];

    expect(calc.feeFor(motorbike, passes)).toBe(0);
  });
});
