# `toll-calculator` — documentation

This folder is organised in three layers, listed in order of decreasing
generality. A reader can stop at any level and have a coherent picture.

## [`HARNESS.md`](HARNESS.md) — methodology (cross-module)

The context-first AI-collaboration methodology that produced this
repo. Components, operating rules, what is and is not delegated. This
file would apply identically to any other module produced under this
harness.

## [`discovery/`](discovery/) — analysis of the reference code

Static analysis of the C# / Java reference shipped at the repository
root: project overview, business rules, bugs (`D§4.1`–`D§4.8`), code
smells, and a high-level recommendation for a rewrite.

## [`plan/`](plan/) — implementation plan for THIS module

Concrete plan for the TypeScript library that the PR delivers. Covers
the context behind the deliverable, tech-stack decisions, architecture,
domain model, testing strategy, implementation phases, delivery, and
the CI / GitHub Actions layer.

## [`follow-ups.md`](follow-ups.md) — dynamic agent log (gitignored)

Working punch list maintained by the agent across sessions. Rewritten
at the end of each task so the next session can recover context cold.
Intentionally not versioned — see [`.gitignore`](.gitignore) and the
rationale in [`HARNESS.md`](HARNESS.md) §7.

---

## Context

The repository is the deliverable for a hiring conversation for an
**AI engineer role**. The brief specified:

- **TypeScript** as the target language.
- **Production-grade code**.
- **Quality through tests** (unit and integration).
- **AI-augmented development** with a strong focus on *context*, aligned
  with the [harness engineering](https://walkinglabs.github.io/learn-harness-engineering/en/)
  approach.

Timeline: one week from brief to PR delivery.
