# `toll-calculator` — TypeScript implementation

A pure TypeScript library for computing daily toll fees with hourly
windowing, a daily cap, configurable fee bands and a pluggable holiday
calendar. No HTTP server, no framework, no runtime dependencies.

This folder is the TypeScript reimplementation of the reference Java / C#
code at the repository root. The why-and-how lives in
[`../docs/plan/`](../docs/plan/); the analysis of the reference code lives
in [`../docs/discovery/`](../docs/discovery/).

## Quick start

Requires Node.js ≥ 20. The package manager (pnpm 10) is provisioned
automatically by [Corepack][corepack], which ships with Node.

```bash
corepack enable        # one-time, no global install needed
pnpm install           # installs dev deps
pnpm test              # runs Vitest once
```

Other useful commands:

| Command              | What it does                                  |
| -------------------- | --------------------------------------------- |
| `pnpm test:watch`    | Re-runs tests on file change.                 |
| `pnpm test:coverage` | Runs tests and produces a coverage report.    |
| `pnpm typecheck`     | `tsc --noEmit`.                               |
| `pnpm lint`          | ESLint (flat config, type-aware rules).       |
| `pnpm format`        | Prettier `--write`.                           |
| `pnpm format:check`  | Prettier `--check` (used in CI).              |
| `pnpm check`         | One-button: typecheck + lint + format + test. |

## Using the library

The 90% case — build a calculator from the bundled preset:

```ts
import { createTollCalculator, REFERENCE_PRESET } from "toll-calculator";

const calc = createTollCalculator(REFERENCE_PRESET);

const fee = calc.feeFor({ type: "Car" }, [
  new Date("2025-09-17T06:25:00+02:00"),
  new Date("2025-09-17T07:30:00+02:00"),
  new Date("2025-09-17T16:00:00+02:00"),
]);
// fee === 44   (8 + 18 + 18)
```

The calculator returns `0` for toll-free vehicle types, for empty pass
lists and for any pass that falls on a day flagged by the configured
calendar. Within any rolling 60-minute window only the highest band fee
counts, and the daily total is clamped to `dailyCap`.

## Adding a new jurisdiction

The library is jurisdiction-agnostic. A new "city" is a new preset:

```ts
import {
  createTollCalculator,
  REFERENCE_PRESET,
  type TollCalculatorPreset,
} from "toll-calculator";

const MY_CITY_PRESET: TollCalculatorPreset = {
  name: "my-city",
  schedule: [
    { from: "07:00", to: "08:59", fee: 25 },
    { from: "16:00", to: "18:00", fee: 25 },
  ],
  calendar: { isHoliday: (date) => /* … */ false },
  dailyCap: 100,
  timeZone: "Europe/Oslo",
};

const calc = createTollCalculator(MY_CITY_PRESET);
```

Or keep the bundled calendar and override only what changes:

```ts
const calc = createTollCalculator({
  ...REFERENCE_PRESET,
  schedule: MY_CITY_SCHEDULE,
  timeZone: "Europe/Oslo",
});
```

Holiday sources can be stacked with `combineCalendars`:

```ts
import { combineCalendars, REFERENCE_PRESET } from "toll-calculator";

const calc = createTollCalculator({
  ...REFERENCE_PRESET,
  calendar: combineCalendars(REFERENCE_PRESET.calendar, companyHolidayCalendar),
});
```

## Public surface

All consumer-facing exports live in `src/index.ts`:

| Export                   | Kind        |
| ------------------------ | ----------- |
| `createTollCalculator`   | function    |
| `combineCalendars`       | function    |
| `REFERENCE_PRESET`       | const value |
| `REFERENCE_FEE_SCHEDULE` | const value |
| `Vehicle`, `VehicleType` | types       |
| `FeeBand`, `FeeSchedule` | types       |
| `HolidayCalendar`        | type        |
| `TollCalculatorOptions`  | type        |
| `TollCalculatorPreset`   | type        |
| `TollCalculator`         | type        |

See [`../docs/plan/04-domain-model.md`](../docs/plan/04-domain-model.md)
for the full domain model.

## Bugs in the reference code (absent in this implementation)

The Java / C# code at the repository root has a number of correctness
issues — documented in
[`../docs/discovery/04-bugs.md`](../docs/discovery/04-bugs.md). None of
them are reproduced here; the regression tests cover each one.

## Conventions

The codebase follows a small set of TypeScript conventions enforced by
the type-checker and ESLint:

- Callable interface members use **property syntax with arrow function
  types**, never method shorthand — see `docs/plan/02-tech-stack.md`
  §2.2.1 for the soundness rationale.
- No `class`, no inheritance, no `this`. Factory functions returning
  object literals only.
- Every member of every public shape is `readonly`.
- `interface` for object shapes; `type` for unions, arrays, tuples.

[corepack]: https://nodejs.org/api/corepack.html
