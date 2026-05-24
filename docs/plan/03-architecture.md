# 3. Architecture

The library is small enough that "architecture" is mostly **where the
boundaries are**. The non-negotiable boundaries are:

- The **fee schedule** is data, not a chain of `if`/`else` — kills `[D§4.3]`.
- The **holiday calendar** is injectable — kills `[D§4.6]`.
- The **vehicle exemption** is a property of the vehicle, not a string
  comparison — kills `[D§4.7]`.
- The **time zone** is an explicit option, not inferred from the runtime
  — kills `[D§4.1]`.

## 3.1 Module layout

```
TypeScript/
├── src/
│   ├── index.ts                # public barrel — the only consumer entry point
│   ├── calculator.ts           # createTollCalculator + the public types
│   ├── vehicle.ts              # Vehicle / VehicleType / isTollFreeVehicle
│   ├── fee-schedule.ts         # FeeBand / FeeSchedule / feeAt
│   ├── calendar/
│   │   ├── index.ts            # HolidayCalendar interface
│   │   ├── combine.ts          # combineCalendars helper
│   │   ├── easter.ts           # Gauss's algorithm
│   │   └── swedish.ts          # createSwedishCongestionTaxCalendar — SFS 2004:629 + SFS 1989:253
│   ├── presets/
│   │   └── reference.ts        # REFERENCE_PRESET + REFERENCE_FEE_SCHEDULE (wires swedish.ts in)
│   └── time/
│       ├── zoned-time.ts       # IANA-zone-aware hour/minute/weekday helpers
│       └── diff.ts             # millisecond -> minute helpers
├── tests/
│   ├── integration.test.ts
│   ├── calculator.test.ts
│   ├── vehicle.test.ts
│   ├── fee-schedule.test.ts
│   ├── easter.test.ts
│   ├── reference-calendar.test.ts
│   ├── combine-calendars.test.ts
│   └── zoned-time.test.ts
├── package.json
├── tsconfig.json
├── vitest.config.ts
├── eslint.config.js
└── README.md
```

The repo-root `Java/` and `C#/` folders stay in place — they are the
reference fixture; removing them would lose context for the reviewer.

## 3.2 Dependency direction

```
index.ts (barrel)
  ├── calculator.ts
  │     ├── vehicle.ts
  │     ├── fee-schedule.ts
  │     └── calendar/index.ts        (interface only)
  ├── calendar/combine.ts ── depends on ── calendar/index.ts
  └── presets/reference.ts
        ├── calendar/index.ts
        ├── calendar/easter.ts
        ├── fee-schedule.ts
        ├── calculator.ts            (for TollCalculatorPreset type)
        └── time/zoned-time.ts
```

- `calculator.ts` knows nothing about any specific jurisdiction — it talks
  to the `HolidayCalendar` interface and the `FeeSchedule` data shape.
  Everything jurisdiction-specific lives in `presets/`.
- A new jurisdiction adds a new file under `presets/` and changes nothing
  else.

## 3.3 Pure core, thin shell

- `calculator.ts` is **pure**: no clock, no globals, no I/O. Given the
  same inputs, it returns the same output.
- The only mutable state in the system is whatever the caller chooses to
  hold — the library does not own any.

## 3.4 No classes

The implementation does not use `class`. Holiday calendars are factory
functions that return object literals satisfying `HolidayCalendar`. The
calculator is the same. This matches the convention in plan §2.2.1
(property-syntax callables, no `this` semantics, no inheritance).

## 3.5 Error policy

### Two contracts, one entry point

The public method `feeFor` enforces **two contracts** simultaneously:

1. **Compile-time contract** — the TypeScript signature.
   `feeFor(vehicle: Vehicle, passes: readonly Date[]) => number` rejects
   `null`, `undefined`, or non-`Date` inputs at the type level for any
   caller compiled through `tsc`. In-process TypeScript code cannot
   misuse the API.

2. **Runtime contract** — boundary validation in
   [`src/validation.ts`](../../TypeScript/src/validation.ts).
   TypeScript guarantees evaporate when input crosses a non-TS boundary:
   `JSON.parse` from an HTTP response, a row from an under-typed ORM, a
   plain-JS consumer, anything traversing `as any`. At runtime the
   function executes against whatever shape the caller actually passed.

The runtime layer is implemented as assertion functions
(`assertVehicle`, `assertPasses`) that take `unknown` and use TypeScript's
`asserts value is X` narrowing. Two consequences:

- ESLint's `no-unnecessary-condition` rule is satisfied **without
  suppressing it**: the checks happen against `unknown`, which legitimately
  includes `null`, `undefined`, strings, and so on.
- The public signature stays strict (`Vehicle`, not `Vehicle | null`), so
  honest TypeScript callers pay no ergonomic cost.

### Behavior

| Input                                       | Outcome                                                 |
| ------------------------------------------- | ------------------------------------------------------- |
| `vehicle` is `null` or `undefined`          | `TypeError("feeFor: \`vehicle\` is required …")`        |
| `vehicle` is not an object with a `type` property | `TypeError("feeFor: \`vehicle\` must be an object …")` |
| `passes` is not an array                    | `TypeError("feeFor: \`passes\` must be an array")`      |
| Any element of `passes` is not a valid `Date` (incl. `Date` with `NaN` time, `null`, `undefined`, a string) | `RangeError("feeFor: passes[i] is not a valid Date")` (with the index `i` of the offending element) |
| Empty `passes` array                        | Returns `0` (identity case, not an error)              |
| Unsorted `passes`                           | Sorted internally; the caller's array is **not** mutated |

Validation runs **once**, at the first lines of `feeFor`. After it
returns, the rest of the function works with narrowed, trusted types —
internal helpers never see malformed input.

### Defense if challenged

> *"Why validate at runtime when TypeScript already guarantees the
> shapes?"* — Because the realistic source of bad input is exactly the
> place where TypeScript stopped helping: a remote API returning strings
> instead of timestamps, an ORM row whose nullability lied, a JS caller
> bypassing types entirely. A clear `TypeError` at the boundary turns
> hours of stack-trace archaeology into one line of "ah, my upstream
> contract changed".

Errors are thrown, not returned. The library is small enough that a
`Result`-style API would be ceremony without benefit, and the boundary
validation catches the cases where `Result` would shine (untrusted
input) at the cost of one assertion call per entry.

## 3.6 Public surface

```ts
export type { Vehicle, VehicleType } from "./vehicle.js";
export type { FeeBand, FeeSchedule } from "./fee-schedule.js";
export type { HolidayCalendar } from "./calendar/index.js";
export { combineCalendars } from "./calendar/combine.js";
export type {
  TollCalculatorOptions,
  TollCalculatorPreset,
  TollCalculator,
} from "./calculator.js";
export { createTollCalculator } from "./calculator.js";
export {
  REFERENCE_PRESET,
  REFERENCE_FEE_SCHEDULE,
} from "./presets/reference.js";
```

Full per-module details and rationale are in
[`04-domain-model.md`](04-domain-model.md).
