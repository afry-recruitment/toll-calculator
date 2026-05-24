# 6. Implementation phases

Work is split into phases. Each phase ends with a green `pnpm check` and
a clean commit. Anything that does not fit a phase is scope creep.

## Phase 0 — Scaffolding (≈ 30 min)

- `pnpm init`, set `"type": "module"`, Node 20 engine, `packageManager: pnpm@10` (provisioned via corepack).
- Install dev deps: typescript, vitest, @vitest/coverage-v8, eslint,
  @typescript-eslint/*, prettier, fast-check.
- `tsconfig.json` with the strict flags from plan §2.2.
- `vitest.config.ts` (default + coverage thresholds).
- `eslint.config.js` + `.prettierrc`.
- `.gitignore` for Node.
- Empty `src/index.ts` so `tsc --noEmit` passes.
- First commit: `chore: scaffold TypeScript project`.

## Phase 1 — Vehicle + fee schedule (≈ 45 min)

- `src/vehicle.ts` with `VehicleType`, `Vehicle`, `isTollFreeVehicle`.
- `src/fee-schedule.ts` with `FeeBand`, `FeeSchedule`, `feeAt`.
- Tests: `vehicle.test.ts`, `fee-schedule.test.ts` (incl. meta-tests for
  overlap/gap on the reference schedule).
- Commit: `feat: vehicle model and fee-schedule primitives`.

## Phase 2 — Zoned-time helpers (≈ 45 min)

- `src/time/zoned-time.ts` — given a `Date` and an IANA zone, return
  `{ year, month, day, hour, minute, weekday }`. Implementation uses the
  Temporal API via `@js-temporal/polyfill` (the only runtime dep).
- `src/time/diff.ts` — `minutesBetween(a, b)`.
- Tests: `zoned-time.test.ts` incl. DST transitions in `Europe/Stockholm`.
- Commit: `feat: timezone-aware date parts and minute diff`.

The decision between `Intl.DateTimeFormat` (zero deps, ~60 lines of glue)
and Temporal was settled in §2.5: Temporal wins on API clarity and
future-proofing (the polyfill drops out when V8 ships native Temporal).

## Phase 3 — Holiday calendar + combiner (≈ 1 h)

- `src/calendar/index.ts` — `HolidayCalendar` interface.
- `src/calendar/easter.ts` — Gauss's algorithm (1800) with the 1807 and 1816 corrections.
- `src/calendar/combine.ts` — `combineCalendars` helper.
- Tests: `easter.test.ts`, `combine-calendars.test.ts`.
- Commit: `feat: Easter computation and combineCalendars helper`.

## Phase 4 — Reference preset + calculator (≈ 1 h)

- `src/calendar/swedish.ts` — `createSwedishCongestionTaxCalendar`
  factory implementing SFS 2004:629 Bilaga 1 + SFS 1989:253 §2 with
  cited legal sources in the JSDoc.
- `src/presets/reference.ts` — `REFERENCE_FEE_SCHEDULE`, `REFERENCE_PRESET`
  (wires the Swedish calendar in).
- `src/calculator.ts` — finalize `createTollCalculator`.
- Tests: `reference-calendar.test.ts`, `calculator.test.ts` with the
  13-row table from plan §5.2.
- Commit: `feat: reference preset and calculator with windowing + cap`.

## Phase 5 — Property tests + integration (≈ 30 min)

- `tests/calculator.property.test.ts` — the four `fast-check` properties.
- `tests/integration.test.ts` — full-day replay.
- Commit: `test: property tests and end-to-end integration test`.

## Phase 6 — CI + README (≈ 45 min)

- `.github/workflows/ci.yml` — lint, typecheck, test, coverage.
- Root `README.md` — install, use, run tests, links to `docs/`.
- Commit: `ci: GitHub Actions pipeline and root README`.

## Phase 7 — PR (≈ 30 min)

- Push branch.
- Open PR with the body templated from
  [`07-delivery-and-pr.md`](07-delivery-and-pr.md).
- Notify the hiring manager and referrer by email/text.

## Total time budget

~6 hours of focused work. Given a week, this leaves plenty of slack for
revisions, README polish, and second-pass cleanup.

## What I will resist doing

- Adding a CLI. Out of scope, distracts from the library.
- Adding "future-proofing" config (city A vs city B). YAGNI.
- Renaming things twice. First name sticks unless a test forces a rename.
- Sneaking in commit messages that say "fix typo" — squash or amend instead.
