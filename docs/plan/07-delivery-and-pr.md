# 7. Delivery and PR plan

## 7.1 Branch and PR strategy

- Work branch: `feat/typescript-implementation`.
- Base: `master` of the fork.
- One PR. Atomic commits per phase from
  [`06-implementation-phases.md`](06-implementation-phases.md).
- No merge commits in the PR — rebase locally before pushing.

## 7.2 Commit conventions

[Conventional Commits](https://www.conventionalcommits.org/) lite:

- `chore:` — scaffolding, deps, config.
- `feat:` — new behavior.
- `test:` — tests-only changes.
- `ci:` — pipeline / workflow.
- `docs:` — README, docs/.
- `fix:` — only if a previous commit on this branch is buggy.

No `wip` or `temp` commits. If a phase needs more than one commit, that is a
sign the phase is too large.

## 7.3 PR description template

```markdown
## What

TypeScript implementation of the toll-fee calculator described in `README.md`,
delivered as a Node 20 library.

## Why this exists / how to read this PR

Start here: [`docs/plan/`](docs/plan/) — the design exists before the code.
Then [`docs/discovery/`](docs/discovery/) — bugs found in the reference
C# / Java code.

## Bugs in the reference code (all absent in this PR)

- [`docs/discovery/04-bugs.md`](docs/discovery/04-bugs.md) §4.1 — `DateTime.Millisecond` time-diff bug in C#.
- §4.2 — `intervalStart` never advanced.
- §4.3 — fee table forgets minutes 00–29 of hours 9–14.
- §4.4 — no defensive handling of empty / unsorted input.
- §4.5 — totals can go negative.
- §4.6 — holidays hard-coded to 2013.
- §4.7 — stringly-typed vehicle exemption.
- §4.8 — single-pass overload is public and bypasses the daily rules.

## Design decisions

- TypeScript 6, Node 20, ESM, strict mode incl. `noUncheckedIndexedAccess`
  and `verbatimModuleSyntax`.
- Fee schedule is data, not control flow.
- Holiday calendar is an injectable `HolidayCalendar` interface; the
  bundled `createSwedishCongestionTaxCalendar` factory derives every
  toll-free day directly from Swedish primary legislation (SFS 2004:629
  Bilaga 1 + SFS 1989:253 §2), generalized to any Gregorian year.
  Easter is computed via Gauss's algorithm in `calendar/easter.ts`.
  No class, just an object literal returned by a factory.
- Public API is jurisdiction-agnostic. The bundled `REFERENCE_PRESET`
  carries the rates / calendar / cap / time zone derived from the
  reference Java / C# code; a new jurisdiction is a new preset file,
  zero library changes.
- `combineCalendars(...calendars)` composes holiday sources without
  callers re-implementing the union.
- All time arithmetic in UTC milliseconds; wall-clock interpretation in
  the preset-supplied IANA zone via the Temporal API (`@js-temporal/polyfill`,
  the library's only runtime dependency — drops out when V8 ships native
  Temporal).

## Assumptions where the README is silent

- The README mentions SEK and never names a country or city; the bundled
  rates and calendar reproduce what the Java / C# reference code
  contains. Other jurisdictions are supported by exporting new presets.
- Empty pass list → `0` (no error).
- Unsorted pass list → sorted internally; caller's array is not mutated.
- The day before a public holiday is also toll-free (reproduces the
  reference code's behavior).
- Daily cap of 60 applies after windowing (one-pass clamp).

## How to run

```
corepack enable        # provisions pnpm 10 (no global install needed)
pnpm install
pnpm test              # unit + property + integration
pnpm check             # typecheck + lint + format + test:coverage
```

## What I will discuss in the follow-up meeting

- Why I rewrote rather than translated.
- How `docs/` was used to brief the AI before writing code.
- Trade-offs around `Intl.DateTimeFormat` vs Temporal / Luxon.
- What I would add if this were a real product (CLI, REST adapter, telemetry).
```

## 7.4 Pre-push checklist

- [ ] `pnpm check` is green locally.
- [ ] Coverage report ≥ 90% lines / 100% branches on `calculator.ts`.
- [ ] No commented-out code.
- [ ] No `console.log` left in `src/`.
- [ ] `README.md` install/test commands actually work in a fresh clone.
- [ ] `git log --oneline` reads like a short story of the work.
- [ ] CI is green on the pushed branch before opening the PR.

## 7.5 Notifying the reviewers

After the PR is open:

- Notify the hiring manager and referrer with the PR URL and one line of context.
- Mention the `docs/` folder explicitly — that is the differentiator.
- Offer to walk through it live.

## 7.6 What I will not do

- Force-push after the PR is open. If a fix is needed, push a follow-up
  commit and let reviewers see the history.
- Open multiple PRs. One coherent deliverable.
- Squash on push. Reviewers can squash on merge if they prefer.
