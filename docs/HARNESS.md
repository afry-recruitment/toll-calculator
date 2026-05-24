# Harness — context-first AI collaboration in this repo

> Adapted from Walking Labs' [Harness Engineering][harness] framework.
> The harness is the methodology that produced the implementation plan
> and the code; it survives across modules and would apply identically
> to any other library produced under this harness. The module-specific
> plan
> lives in [`plan/`](plan/).

## What this document is

A reusable description of **how the human + AI collaborate on this
codebase**: the context layers, the briefing template, what is and is
not delegated, the anti-patterns avoided, the harness components that
live in this repo, and the operating rules followed during sessions.

If you remove every line of TypeScript from this repo and start a
different module tomorrow, this document still applies. The
implementation plan in [`plan/`](plan/) does not.

---

## 1. Why a harness

The brief emphasised that the way I use AI matters. The honest
answer is that I do not type prompts directly into a code-generation
model and accept what comes out — the gap between "looks right" and
"is right" is too large. Instead I **invest heavily in context** before
asking the AI to produce anything: design docs, discovery findings,
testing strategy, explicit phase deliverables. The AI is a junior with
infinite patience and zero memory; my job is to brief it well, verify
its outputs, and own every decision.

That practice has a name: **harness engineering** — designing the
working environment so the AI's behaviour is constrained by explicit
artifacts, not by hope. The `docs/` folder *is* that harness. A
reviewer can see exactly what the AI was told before any code was
written.

[harness]: https://walkinglabs.github.io/learn-harness-engineering/en/

---

## 2. The three layers of context I use

1. **Project context** — `docs/discovery/` and `docs/plan/`. Stable
   across sessions, version-controlled, English.
2. **Session context** — the conversation with the AI, scoped to one
   phase from `plan/06-implementation-phases.md`. Short sessions, one
   phase at a time.
3. **Working context** — the open file in the editor, the failing
   test, the exact error message. Always pasted verbatim.

If any of these three is missing, I do not type a prompt.

---

## 3. How I brief the AI per phase

Per phase, the prompt to the AI follows this template:

```
We are in phase N of docs/plan/06-implementation-phases.md.
Read docs/plan/04-domain-model.md §X.Y for the exact API.
Read docs/discovery/04-bugs.md §Z for what must NOT happen.
Write src/<file>.ts. No extra exports. No premature abstraction.
Then write tests/<file>.test.ts covering the cases in
docs/plan/05-testing-strategy.md §X.
Run `pnpm check`. If anything fails, do not patch the test — fix
the code.
```

---

## 4. What I will NOT delegate to the AI

- **Naming.** I pick the names. The AI can suggest, I decide.
- **Public API shape.** Frozen in `plan/04-domain-model.md` before any
  code is written.
- **Test cases.** I write the table; the AI fills the rows.
- **Architectural decisions.** Already made in `plan/`.

---

## 5. What I will delegate to the AI

- Implementing the body of a function whose signature and tests
  already exist.
- Translating a `plan/` §X.Y into a first-draft file.
- Drafting commit messages and PR bodies from the diff.
- Generating boilerplate (eslint config, tsconfig, CI yaml) from a
  verbal spec.

---

## 6. Anti-patterns I avoid

- Letting the AI "decide" the architecture by writing the first file
  from a one-line prompt.
- Asking the AI to "review" code it just wrote — the second pass adds
  nothing.
- Long, meandering chat sessions. I close the session when the phase
  is done.
- Pasting the whole codebase into the prompt. Phase-scoped context
  only.

---

## 7. Harness components in this project

Mapping the harness-engineering concepts to concrete artifacts in this
repo. Each row is a portable concept paired with the artifact that
implements it here.

| Harness concept                    | Artifact in this repo                                                                                                                                              |
| ---------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| **Repository as system of record** | Every architectural decision, legal citation, and tradeoff lives in `docs/`. Conversation history is ephemeral; only what is committed survives.                   |
| **Feature lists as primitives**    | `plan/06-implementation-phases.md` decomposes the work into seven enumerated phases, each with an explicit deliverable and a `pnpm check` gate.                    |
| **Initialization phase separate from execution** | `docs/discovery/` and `docs/plan/` exist before the first source file. The AI is never asked to write code without first being briefed via these files.            |
| **Victory conditions**             | Two layers: (a) `pnpm check` green locally and in CI; (b) every rule citing a primary source has its citation fetched and verified before commit.                  |
| **Verification inside the harness** | `vitest.config.ts` enforces ≥ 90 % global and 100 % `calculator.ts`/`fee-schedule.ts` coverage. ESLint flat config with `strictTypeChecked`. Prettier as format gate. |
| **One brief is not enough**        | The plan is ten files under `plan/`, each scoped to one concern. A single mega-README would be unreadable and untrustable.                                         |
| **Clean state at session end**     | No `wip` commits, no orphan branches, no skipped tests left in place. The dynamic punch list at `docs/follow-ups.md` (gitignored, agent-maintained) is rewritten at task end so the next session reads it first and picks up cold.  |
| **Observability**                  | `pnpm test:coverage` produces a per-file coverage report; CI uploads the artifact. Test output is the primary signal.                                              |

---

## 8. Operating rules I apply when working with the AI

Stated as portable instructions so they transfer to other projects.
The evidence that each rule was applied here lives in the git history
and in the docs themselves.

1. **Brief before generating.** No prompt-to-produce-code until the
   relevant `plan/` section is loaded into the model's context.
   *Reason:* the AI is excellent at executing a brief, mediocre at
   inventing one.

2. **Never accept a suppression as a fix.** If a linter, type-checker
   or test points at a conflict, the answer is realignment, not
   silencing. `eslint-disable`, `@ts-ignore`, and `xfail` are smells.
   *Reason:* the tool detected a real misalignment; hiding the signal
   leaves the misalignment in the code.

3. **Verify primary sources before committing citations.** Anything
   quoted in JSDoc (a law, an RFC, a spec) is fetched and pasted
   verbatim, not paraphrased from the model's training data.
   *Reason:* model citations can be plausible but wrong; the cost of
   one `WebFetch` is far less than the cost of a wrong-cited rule
   compounding across a year of operations.

4. **The AI proposes; the human picks the name.** Identifiers, file
   names, and public-API shapes are decided by the human. The AI
   offers options, not verdicts.
   *Reason:* naming sets vocabulary for everyone downstream; it
   deserves one human's judgment.

5. **Doc-and-code drift is a bug, not a footnote.** When the code
   evolves, the relevant `plan/` section is updated in the same PR.
   *Reason:* a future maintainer who reads outdated docs is worse off
   than one who has no docs.

6. **Commits are records, not diaries.** Each commit's body answers
   the questions a future `git log --grep` is asking: what changed,
   why, what bug it kills, where the citation lives. Iteration noise
   is squashed out.
   *Reason:* `git log` is consulted by both humans and LLMs; the cost
   of writing a good commit body is paid once, the cost of a bad one
   compounds.

7. **One brief, many phases.** Each phase of work gets a fresh session
   scoped to the relevant plan files only. The plan is the long-term
   memory; the conversation is short-term.
   *Reason:* long meandering chats produce long meandering code.

8. **Reject "good enough" when the law is one fetch away.** If a rule
   is encoded in a public document (legal text, spec, vendor docs),
   the implementation cites the document — not the closest hand-coded
   approximation.
   *Reason:* the next year's data will reveal which approximations
   were wrong; the documented rule will not.

---

## 9. Why this matters

The brief asked how I use AI. The answer in this folder is
concrete:

- No vibe-coding. Every file has a stated purpose before it is
  created.
- No "trust me" — the plan is in source control alongside the code.
- The discovery folder shows the AI was given **adversarial input**
  (the broken reference) and asked to enumerate the failure modes
  *before* writing a replacement.
- The plan folder shows that the design exists on paper before any
  line of TypeScript is written.

If a junior engineer was asked to take over the project tomorrow,
they would read these documents in order and be productive in an
hour. That is the test.

---

## 10. Reference

- Walking Labs — *Harness Engineering*: <https://walkinglabs.github.io/learn-harness-engineering/en/>
- This repo's plan: [`plan/`](plan/)
- This repo's discovery (analysis of the reference Java/C#): [`discovery/`](discovery/)
- Dynamic agent log (gitignored, regenerated each session):
  [`follow-ups.md`](follow-ups.md)
