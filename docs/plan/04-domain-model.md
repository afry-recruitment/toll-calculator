# 4. Domain model and API

The public surface is intentionally small. The library exposes:

- A pure factory `createTollCalculator(options)` that produces a calculator.
- A typed `TollCalculatorOptions` shape that callers fill in.
- A `TollCalculatorPreset` — a named, ready-to-use options bundle.
- One bundled preset (`REFERENCE_PRESET`) that mirrors the data baked into
  the original Java / C# reference code.
- A `combineCalendars` helper for composing holiday calendars without
  re-implementing the union logic.
- Plain types for `Vehicle`, `FeeBand`, `FeeSchedule`, `HolidayCalendar`.

Every callable is declared as a `readonly` property with an arrow function
type — see plan §2.2.1 for the rationale.

## 4.1 Vehicle

```ts
// vehicle.ts
export type VehicleType =
  | "Car"
  | "Motorbike"
  | "Tractor"
  | "Emergency"
  | "Diplomat"
  | "Foreign"
  | "Military";

export interface Vehicle {
  readonly type: VehicleType;
}

const TOLL_FREE_TYPES = new Set<VehicleType>([
  "Motorbike",
  "Tractor",
  "Emergency",
  "Diplomat",
  "Foreign",
  "Military",
]);

export const isTollFreeVehicle = (v: Vehicle): boolean =>
  TOLL_FREE_TYPES.has(v.type);
```

`VehicleType` is a **string literal union**, not an enum. Three reasons:

1. Cheap interop — callers can write `{ type: "Car" }` without importing.
2. Exhaustiveness via `never` in `switch` blocks.
3. TypeScript enums emit runtime objects; we do not need that.

Killing bug `[D§4.7]`: exemption is a `Set` lookup, not a chain of string
equality checks.

## 4.2 Fee schedule

```ts
// fee-schedule.ts
export interface FeeBand {
  /** Inclusive lower bound as "HH:MM". */
  readonly from: string;
  /** Inclusive upper bound as "HH:MM". */
  readonly to: string;
  /** Fee in the schedule's currency. */
  readonly fee: number;
}

export type FeeSchedule = readonly FeeBand[];
```

The default data lives next to the preset:

```ts
// presets/reference.ts
export const REFERENCE_FEE_SCHEDULE: FeeSchedule = [
  { from: "06:00", to: "06:29", fee: 8 },
  { from: "06:30", to: "06:59", fee: 13 },
  { from: "07:00", to: "07:59", fee: 18 },
  { from: "08:00", to: "08:29", fee: 13 },
  { from: "08:30", to: "14:59", fee: 8 },
  { from: "15:00", to: "15:29", fee: 13 },
  { from: "15:30", to: "16:59", fee: 18 },
  { from: "17:00", to: "17:59", fee: 13 },
  { from: "18:00", to: "18:29", fee: 8 },
];
```

A `feeAt(schedule, hour, minute)` helper lives in `fee-schedule.ts`:

```ts
export const feeAt = (
  schedule: FeeSchedule,
  hour: number,
  minute: number,
): number => {
  const m = hour * 60 + minute;
  for (const band of schedule) {
    const [fh, fm] = band.from.split(":").map(Number);
    const [th, tm] = band.to.split(":").map(Number);
    if (m >= fh * 60 + fm && m <= th * 60 + tm) return band.fee;
  }
  return 0;
};
```

Killing bug `[D§4.3]`: the fee table is data, and `feeAt` covers every
minute of the day by definition (any minute not inside a band returns 0).

A schedule-validation test will assert there are **no overlaps** and **no
gaps inside 06:00–18:29** — meta-tests on the data.

## 4.3 Holiday calendar

```ts
// calendar/index.ts
export interface HolidayCalendar {
  readonly isHoliday: (date: Date) => boolean;
}
```

The default implementation is a **factory function**, not a class — to
match the no-classes house style. It lives in `src/calendar/swedish.ts`
and is wired into the preset via `src/presets/reference.ts`.

> Full primary-source ledger — verbatim Swedish excerpts, divergences
> from the Java reference's 2013 list, and the mapping from each
> statute clause to a line in `swedish.ts` — lives in
> [`legal-references.md`](legal-references.md).

The factory derives every toll-free day from primary Swedish
legislation, not from a hard-coded list:

- **SFS 2004:629** (Lag om trängselskatt), Bilaga 1 — the law that
  governs Stockholm's congestion tax. Source:
  https://www.riksdagen.se/sv/dokument-och-lagar/dokument/svensk-forfattningssamling/lag-2004629-om-trangselskatt_sfs-2004-629/
- **SFS 1989:253** (Lag om allmänna helgdagar), §2 — the enumeration of
  *helgdagar* the previous law refers to. Source:
  https://www.riksdagen.se/sv/dokument-och-lagar/dokument/svensk-forfattningssamling/lag-1989253-om-allmanna-helgdagar_sfs-1989-253/

The algorithm (verbatim from `src/calendar/swedish.ts`):

```ts
const isHoliday = (date: Date): boolean => {
  const p = parts(date, "Europe/Stockholm");
  // 1. Weekend.
  if (p.weekday >= 6) return true;
  // 2. Public holiday per SFS 1989:253 §2.
  if (isPublicHoliday(p.year, p.month, p.day)) return true;
  // 3. July: free EXCEPT first five non-Saturday weekdays (Bilaga 1 carve-out).
  if (p.month === 7) return !julyTaxedDays.has(p.day);
  // 4. Day before a public holiday, unless one of the three explicit
  //    Bilaga 1 carve-outs: Maundy Thursday, Ascension Eve, All Saints' Eve.
  if (isCarveOutTaxed(p.year, p.month, p.day)) return false;
  return isPublicHoliday(tomorrow(p));
};
```

Helpers:

- `easterSunday(year)` from `calendar/easter.ts` computes Easter via
  Gauss's algorithm (Gauss 1800, *Berechnung des Osterfestes*; pure
  integer arithmetic, valid 1583+).
- Easter-derived helgdagar offsets: −2 (Good Friday), 0 (Easter Sunday),
  +1 (Easter Monday), +39 (Ascension Day), +49 (Whit Sunday).
- Bilaga 1 carve-outs: Maundy Thursday (Easter −3), Ascension Eve
  (Easter +38), All Saints' Eve (Friday before All Saints' Day).
- Midsummer Day: Saturday between Jun 20 and Jun 26 (definition in SFS
  1989:253 §2).
- All Saints' Day: Saturday between Oct 31 and Nov 6 (same source).

Killing bug `[D§4.6]`: the reference Java's 2013-hard-coded list is
both year-limited and **factually wrong about three days** (Maundy
Thursday, Ascension Eve, Nov 1) per SFS 2004:629 Bilaga 1. Implementing
the law directly fixes both at once.

### Composing calendars

```ts
// calendar/combine.ts
export const combineCalendars = (
  ...calendars: readonly HolidayCalendar[]
): HolidayCalendar => ({
  isHoliday: (date) => calendars.some((c) => c.isHoliday(date)),
});
```

The expected use case is stacking sources of holiday data — e.g. a
jurisdiction calendar plus a company-specific calendar plus a one-off
exception list — without callers re-implementing the union.

## 4.4 Calculator

```ts
// calculator.ts
export interface TollCalculatorOptions {
  readonly schedule: FeeSchedule;
  readonly calendar: HolidayCalendar;
  readonly dailyCap: number;
  readonly timeZone: string;
}

export interface TollCalculatorPreset extends TollCalculatorOptions {
  readonly name: string;
}

export interface TollCalculator {
  readonly feeFor: (vehicle: Vehicle, passes: readonly Date[]) => number;
}

export const createTollCalculator = (
  options: TollCalculatorOptions,
): TollCalculator => ({
  feeFor: (vehicle, passes) => {
    if (isTollFreeVehicle(vehicle)) return 0;
    if (passes.length === 0) return 0;

    const sorted = [...passes].sort((a, b) => a.getTime() - b.getTime());

    let total = 0;
    let windowStart = sorted[0];
    let windowMax = feeForPass(windowStart);

    for (let i = 1; i < sorted.length; i++) {
      const pass = sorted[i];
      const minutes = (pass.getTime() - windowStart.getTime()) / 60_000;

      if (minutes <= 60) {
        windowMax = Math.max(windowMax, feeForPass(pass));
      } else {
        total += windowMax;
        windowStart = pass;
        windowMax = feeForPass(pass);
      }
    }

    total += windowMax;
    return Math.min(total, options.dailyCap);

    function feeForPass(date: Date): number {
      if (options.calendar.isHoliday(date)) return 0;
      const { hour, minute } = parts(date, options.timeZone);
      return feeAt(options.schedule, hour, minute);
    }
  },
});
```

Killing bugs `[D§4.1]`, `[D§4.2]`, `[D§4.5]`, `[D§4.8]` in one pass:

- Time diff is `getTime()` in ms → exact minutes, no `Millisecond`-component
  trap.
- `windowStart` is reassigned when the window flips → correct windowing.
- `total` is never decremented → no negative totals.
- The hourly fee function is **private to the closure** → no public bypass
  of the daily logic.

## 4.5 Reference preset

```ts
// presets/reference.ts
export const REFERENCE_PRESET: TollCalculatorPreset = {
  name: "reference",
  schedule: REFERENCE_FEE_SCHEDULE,
  calendar: createSwedishCongestionTaxCalendar(),
  dailyCap: 60,
  timeZone: "Europe/Stockholm",
};
```

The IANA time zone `Europe/Stockholm` is a **data property** of the preset,
not part of the API surface. It is the zone for which the bundled schedule's
wall-clock semantics were defined; consumers using a different rate table
override `timeZone` (and the other fields) alongside it.

## 4.6 Public surface (consumer view)

```ts
// The 90% case:
import { createTollCalculator, REFERENCE_PRESET } from "toll-calculator";
const calc = createTollCalculator(REFERENCE_PRESET);

// Custom jurisdiction — keep the calendar, override schedule and zone:
import { createTollCalculator, REFERENCE_PRESET } from "toll-calculator";
const calc = createTollCalculator({
  ...REFERENCE_PRESET,
  schedule: MY_CITY_SCHEDULE,
  timeZone: "Europe/Oslo",
});

// Stacking calendars:
import {
  createTollCalculator,
  combineCalendars,
  REFERENCE_PRESET,
} from "toll-calculator";
const calc = createTollCalculator({
  ...REFERENCE_PRESET,
  calendar: combineCalendars(
    REFERENCE_PRESET.calendar,
    myCompanyHolidayCalendar,
  ),
});
```

The library never names a specific city in any identifier. Geography lives
in the data of named presets and in IANA time-zone strings.
