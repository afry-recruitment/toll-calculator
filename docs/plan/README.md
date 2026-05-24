# Plan — TypeScript implementation

Concrete plan for the deliverable. This is the document I would hand a
collaborator (human or AI) before writing any code, so that the rewrite is
**production-grade** instead of a literal translation of the reference.

## Index

1. [Context and goals](01-context-and-goals.md)
2. [Tech stack](02-tech-stack.md)
3. [Architecture](03-architecture.md)
4. [Domain model and API](04-domain-model.md)
5. [Testing strategy](05-testing-strategy.md)
6. [Implementation phases](06-implementation-phases.md)
7. [Delivery and PR plan](07-delivery-and-pr.md)
8. [GitHub Actions — automation layer](08-github-automations.md)

Supporting reference:

- [Legal references for the Swedish calendar](legal-references.md) —
  primary-source ledger (SFS 2004:629 + SFS 1989:253) for the rules
  implemented in `src/calendar/swedish.ts`.

The methodology that produced this plan — context-first AI
collaboration, harness components, and operating rules — lives one
level up in [`../HARNESS.md`](../HARNESS.md), because it transcends
this specific module and would apply to any other.

Cross-references to discovery findings use `[D§n.m]` notation —
e.g. `[D§4.1]` is bug 4.1 in
[`../discovery/04-bugs.md`](../discovery/04-bugs.md).
