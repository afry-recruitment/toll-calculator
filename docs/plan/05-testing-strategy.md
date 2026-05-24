# 5. Testing strategy

Tests are the deliverable. They are also the proof that I do what I told
the hiring manager I do.

## 5.1 Test pyramid for this library

```
            ┌──────────────────┐
            │  integration (1) │   end-to-end smoke through the public API
            ├──────────────────┤
            │  property  (~3)  │   windowing + cap invariants via fast-check
            ├──────────────────┤
            │   unit    (~30)  │   fee table, calendar, vehicle, helpers
            └──────────────────┘
```

Unit-heavy by design. The integration test is the safety net: if every unit
passes but the wiring is wrong, the integration test fails.

## 5.2 Unit tests — per module

### `fee-schedule.test.ts`

- Each band returns the documented fee at its `from`, mid-point, and `to`.
- Minutes between bands return `0`.
- **Meta-test:** no two bands overlap.
- **Meta-test:** bands cover 06:00–18:29 with no gaps (specifically catches
  bug `[D§4.3]`, the "missing 09:00–14:29" hole).

### `vehicle.test.ts`

- Each of the six toll-free types returns `true` from `isTollFreeVehicle`.
- `Car` returns `false`.
- Type-level test (`expectTypeOf`) asserts the union is exhaustive.

### `easter.test.ts`

- Known Easter Sundays for 2013, 2024, 2025, 2030 are computed correctly.
- 2013 in particular matches the dates hard-coded in the original Java code
  (sanity check).

### `reference-calendar.test.ts`

- Every fixed holiday in 2025 (per the reference Java code) is flagged.
- Easter-derived holidays in 2025 are flagged.
- A normal Tuesday is not flagged.
- A Saturday and a Sunday are flagged.
- The **day before** a public holiday is flagged (reference-code rule).
- The 2013 set matches the original Java hard-coded list exactly —
  regression guarantee against the reference behavior.

### `combine-calendars.test.ts`

- A date flagged by **any** input calendar is flagged in the combined one.
- A date flagged by **no** input calendar is not flagged.
- `combineCalendars()` with zero arguments returns a calendar that flags
  nothing (vacuous truth on `[].some(...)`).

### `zoned-time.test.ts`

- A `Date` whose UTC hour is 04:30 maps to wall-clock 06:30 in
  `Europe/Stockholm` during summer time (DST).
- A `Date` whose UTC hour is 05:30 maps to 06:30 in winter time (no DST).
- Catches the implicit-timezone bug from the C# reference.

### `calculator.test.ts`

Parameterised cases ("table-driven tests"):

| # | Scenario                                              | Expected |
| - | ----------------------------------------------------- | -------- |
| 1 | Car, single pass at 07:15 (rush)                      | 18       |
| 2 | Car, two passes 07:15 and 07:45 (same window)         | 18       |
| 3 | Car, passes 07:15 and 08:45 (different windows)       | 18+13=31 |
| 4 | Car, passes 06:15, 07:30, 15:30, 17:30 (4 windows)    | 8+18+18+13=57 |
| 5 | Car, 5+ rush passes ⇒ total > 60                       | 60 (cap) |
| 6 | Motorbike, any time                                    | 0        |
| 7 | Car, Saturday                                          | 0        |
| 8 | Car, midsummer eve                                     | 0        |
| 9 | Empty pass list                                        | 0        |
| 10| Unordered pass list                                    | same as sorted |
| 11| Passes spanning 01:00–05:00 (no fee window)            | 0        |
| 12| Boundary minutes 06:00, 06:29, 06:30, 18:29, 18:30    | 8, 8, 13, 8, 0 |
| 13| Same-millisecond duplicate passes                      | charged once |

Each row is one test case via Vitest's `test.each`.

## 5.3 Property tests (`fast-check`)

Three properties — small, but they cover infinite cases:

1. **Daily cap holds.** For any vehicle and any non-empty pass list,
   `feeFor(v, passes) <= 60`.
2. **Toll-free dominates.** For any toll-free vehicle and any passes,
   `feeFor(v, passes) === 0`.
3. **Order-independence.** For any pass list,
   `feeFor(v, passes) === feeFor(v, shuffle(passes))`.
4. **Monotone over windows.** Adding a pass cannot decrease the total
   (catches a regression of `[D§4.5]`).

## 5.4 Integration test

One file, one test:

- Build `createTollCalculator(REFERENCE_PRESET)`.
- Three scenarios, each proving a distinct path through the preset's
  wiring:
  1. **Commuter day** — 6 passes on a regular weekday → 57 SEK (windowing
     + multi-band, hand-verified, below cap).
  2. **Holiday day** — Car on Easter Sunday → 0 (calendar wired,
     movable feast resolved).
  3. **Toll-free vehicle** — Motorbike on the commuter day → 0
     (early-exit via `isTollFreeVehicle`).

These tests prove the library *works as advertised* through the bundled
preset — not just that each unit works in isolation.

## 5.5 What I deliberately do not test

- The standard library (`Date`, `Math.min`).
- Internal helpers that are exercised through the public API and whose
  behavior is verified by the unit tests of their callers.
- Performance — irrelevant for this domain.

## 5.6 Coverage target

`90%+` lines, `100%` of branches in `calculator.ts` and `fee-schedule.ts`.
Coverage is a smell detector, not a goal — chasing 100% will produce noise
tests.

## 5.7 How tests get run

- `pnpm test` — runs Vitest once, exits.
- `pnpm test:watch` — local dev loop.
- `pnpm test:coverage` — used in CI; enforces the thresholds in `vitest.config.ts`.
- `pnpm check` — runs `tsc --noEmit`, lint, format-check, and
  `test:coverage`. The one-button "is the PR green?" command — identical
  to what CI runs, so local green ⇒ CI green.
