# 2. Tech stack

Every choice here is justified in one line. If I cannot defend the choice in
a sentence, it does not go in.

## 2.0 Shape of the deliverable

A plain Node/TypeScript library — no HTTP server, no framework (Nest / Express /
Fastify), no CLI. The reference code is a class; the TS equivalent is a typed function.

## 2.1 Runtime and language

| Tool          | Choice                | Why                                                                         |
| ------------- | --------------------- | --------------------------------------------------------------------------- |
| Language      | **TypeScript 6.x**    | The brief was explicit: TypeScript. Using the latest stable line.           |
| Runtime       | **Node.js 20 LTS**    | Current LTS; safe default for an enterprise reviewer.                       |
| Module system | **ESM** (`"type": "module"`) | Modern default; no surprises with `import`/`export`.                  |
| Package mgr   | **pnpm 10** (via corepack) | Faster installs, content-addressed store, strict peer deps. Corepack ships with Node 20+, so the reviewer needs no global install — `corepack enable && pnpm install` is the whole setup. |

## 2.2 TypeScript configuration

`tsconfig.json` highlights:

- `"strict": true`
- `"noUncheckedIndexedAccess": true` — would have caught bug `[D§4.4]`
- `"exactOptionalPropertyTypes": true`
- `"verbatimModuleSyntax": true` — forces explicit `import type` / `export type`
- `"target": "ES2022"`, `"module": "NodeNext"`, `"moduleResolution": "NodeNext"`
- `"declaration": true`, `"sourceMap": true`

## 2.2.1 TypeScript coding conventions

A handful of style decisions that are enforced by the type-checker and / or
ESLint, recorded here so they can be defended in review.

### Function members in interfaces — property syntax, never method shorthand

```ts
// ✅ used everywhere
interface HolidayCalendar {
  readonly isHoliday: (date: Date) => boolean;
}

// ❌ never used
interface HolidayCalendar {
  isHoliday(date: Date): boolean;
}
```

Reasons:

1. **Stricter parameter checking.** With `strictFunctionTypes` (on by
   default in `strict` mode), property-style function types are checked
   **contravariantly**; method shorthand is checked **bivariantly** — a
   deliberate soundness hole TypeScript keeps for ergonomics with
   `Array.prototype` methods. Property syntax catches bugs the method
   form lets through.
2. **`readonly` works.** A `readonly` property cannot be reassigned;
   method shorthand has no equivalent modifier. Marking every callable
   `readonly` is consistent with the library's immutability stance.
3. **No-classes house style.** The implementation uses factory functions
   returning object literals (`createSwedishCongestionTaxCalendar(): HolidayCalendar`),
   never classes with methods. Property syntax matches that — method
   shorthand suggests OOP class methods.
4. **Lint-enforced.** `@typescript-eslint/method-signature-style` is on
   via the `stylisticTypeChecked` preset and defaults to `"property"`.

### `interface` vs `type` — interface for object shapes, type for everything else

| Shape                         | Tool          |
| ----------------------------- | ------------- |
| Object / function-as-property | `interface`   |
| String-literal union          | `type`        |
| Array / tuple                 | `type`        |
| Mapped / conditional          | `type`        |

Rationale: `interface` is slightly more ergonomic for consumers who want
to `extends` a public API shape, and has marginal compiler caching
benefits on large types. For unions, arrays and other non-object shapes,
`interface` is not expressible — `type` is required. This is a mechanical
rule, not a paradigm statement: the codebase is functional (no `class`,
no inheritance), and `interface` here describes structural shapes only.

### Immutability — `readonly` on every member of every public shape

Every interface in the public surface uses `readonly` on every member.
The library does not mutate inputs and does not expect callers to mutate
outputs; `readonly` makes that contract type-level rather than verbal.

### Array types — `readonly T[]`, not `ReadonlyArray<T>`

Both forms are semantically identical. `readonly T[]` is the lint default
(`@typescript-eslint/array-type`) and reads shorter at call sites.

## 2.3 Testing

| Tool      | Choice          | Why                                                                             |
| --------- | --------------- | ------------------------------------------------------------------------------- |
| Runner    | **Vitest**      | Fast, ESM-native, Jest-compatible API, no Babel needed.                         |
| Assertion | **vitest** built-in | One dep is better than two.                                                 |
| Coverage  | **`@vitest/coverage-v8`** | Native v8 coverage, no instrumentation overhead.                       |
| Property  | **`fast-check`** (optional) | Property-based tests for the windowing algorithm — see plan §5.3.    |

## 2.4 Quality gates

| Tool          | Choice               | Why                                                              |
| ------------- | -------------------- | ---------------------------------------------------------------- |
| Linter        | **ESLint** w/ `@typescript-eslint` | Catches what the type-checker doesn't.            |
| Formatter     | **Prettier**         | Removes style debates from the PR review.                        |
| Type-check    | **`tsc --noEmit`**   | Final correctness gate.                                          |
| Git hooks     | None / **husky** optional | Only add if it doesn't slow down `npm test`.                |

## 2.5 Date and time

### Decision: **`@js-temporal/polyfill`** for zone arithmetic

The library's only runtime dependency. The public API still accepts the
platform `Date` (so consumers don't have to import anything to use us);
internally we convert to `Temporal.Instant` to extract wall-clock parts
in a specific IANA zone.

| Layer                                 | Tool                              |
| ------------------------------------- | --------------------------------- |
| Input type from consumers             | `Date` (platform-native)          |
| `feeFor` 60-min-window arithmetic     | `Date.getTime()` ms subtraction   |
| Wall-clock extraction in a zone       | `Temporal.Instant.toZonedDateTimeISO(zone)` (via polyfill) |
| Future-proofing                       | Polyfill drops out when V8 ships Temporal natively — the call sites are spec-final |

### Why a library at all (and not native `Intl.DateTimeFormat`)

`Date` has no method that answers *"what wall clock does this instant
produce in city X?"*. The closest native option is
`Intl.DateTimeFormat`, but it is a **string-formatting** API — using it
for arithmetic means:

- Calling `formatToParts()` to get `{type, value}` tuples.
- Parsing string values back to numbers.
- Working around the `"24"` quirk of `hour12: false` in older engines.
- Deriving weekday manually because the formatted weekday string is
  locale-sensitive.
- Caching formatter instances (expensive to construct) yourself.

That works (a ~60-line wrapper does the job) but is a known rabbit hole
in code review. A dedicated library exposes the arithmetic directly.

### Alternatives considered

Evaluated before committing to Temporal:

| Library                           | Size       | Weekly downloads (2026) | Verdict      | Reason                                                                                                              |
| --------------------------------- | ---------- | ----------------------- | ------------ | ------------------------------------------------------------------------------------------------------------------- |
| **`@js-temporal/polyfill`**       | ~50 KB     | ~1.5 M                  | ✅ **chosen** | TC39 Stage 3 spec; will be native in V8 within 1–2 years; cleanest API; explicit DST-ambiguity handling.            |
| **Luxon**                         | ~75 KB     | ~30 M                   | runner-up    | Most battle-tested; same author lineage as moment.js; chosen if "max reviewer familiarity" outweighs future-proof.  |
| **`dayjs` + utc + timezone**      | ~7 KB + plugins | ~25 M             | rejected     | Smallest, but plugin-based timezone support is awkward; moment-style mutable-feel API.                              |
| **`date-fns-tz`**                 | ~30 KB     | ~7 M                    | rejected     | Functional + tree-shakable, but two packages (`date-fns` + `date-fns-tz`) for what one should do.                   |
| **`moment-timezone`**             | ~70 KB     | ~7 M (declining)        | rejected     | In maintenance mode; project itself recommends migration away.                                                      |
| **Native `Intl.DateTimeFormat`**  | 0 KB       | n/a                     | rejected     | Zero runtime deps but ~60 lines of glue code that becomes a review rabbit hole; doesn't help on DST ambiguity.      |

### Defense if challenged

- *"Why not Luxon, which has 20× more downloads?"* — Luxon is the safer
  conservative choice. Temporal is what the language is converging to;
  picking it now means our call sites become spec-final the moment V8
  ships it (Firefox 139 already does). The polyfill version follows the
  spec exactly; future code change is just dropping the import.
- *"Why a library at all?"* — see the rabbit-hole list above. The
  ~60-line `Intl.DateTimeFormat` wrapper works but invites questions
  about locale handling, DST ambiguity, formatter caching, and the
  `"24"` quirk. A dedicated time library answers all of those by design.
- *"Why a polyfill of an unfinished spec?"* — Temporal is Stage 3, which
  means the API is finalized in shape; only minor edge-case adjustments
  happen now. Risk is low; the polyfill tracks the spec.

## 2.6 CI

| Tool                 | Choice                                                |
| -------------------- | ----------------------------------------------------- |
| **GitHub Actions**   | Native to where the PR will live.                     |
| Matrix               | Node 20 only — single supported runtime.              |
| Jobs                 | `lint`, `typecheck`, `test` (with coverage artifact). |

## 2.7 Dependency budget

Production deps: **1** (`@js-temporal/polyfill`). Justified in §2.5.

Dev deps: ~9 — `typescript`, `vitest`, `@vitest/coverage-v8`,
`@fast-check/vitest`, `fast-check`, `eslint`, `@eslint/js`,
`typescript-eslint`, `prettier`, `@types/node`. Anything else needs
justification in the PR.
