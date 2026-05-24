# 1. Context and goals

## 1.1 How I got here

- The deliverable is for a hiring conversation for an **AI engineer
  role** at a consultancy client.
- A scoping call with the hiring manager set the brief.
- I have **one week** from that call to deliver the PR.
- Successful delivery leads to a follow-up code-discussion session.

## 1.2 What the brief told me to optimize for

From the scoping call:

1. **TypeScript.** The whole call orbited TypeScript — that is the
   target language for the deliverable.
2. **Proud-to-ship-to-prod.** The brief was explicit: *"something you'd be
   proud to send to production"*. The test is not "make it pass" — it is
   "show how you build software".
3. **Tests are part of the deliverable.** I said in the call that I use unit
   and integration tests to lock down quality; I now have to back that up.
4. **AI usage is part of the deliverable.** I said I lean on the
   [harness engineering](https://walkinglabs.github.io/learn-harness-engineering/en/)
   philosophy — context first, AI second. This `docs/` folder is part of
   that demonstration: the AI was given clean context before any code was
   written.

## 1.3 Definition of done

The deliverable is "done" when the following are all true:

- [ ] All business rules from the README produce correct results.
- [ ] All bugs documented in `[D§4]` are absent in the new implementation.
- [ ] Unit tests cover every fee tier, every exemption, every edge case.
- [ ] At least one integration test exercises the public API end-to-end.
- [ ] CI runs lint + type-check + tests on every push.
- [ ] `README.md` (root) explains how to install, test, and use the library
      in under 60 seconds of reading.
- [ ] The PR description lists the bugs found, the design decisions made,
      and the assumptions taken where the original README is silent.
- [ ] Repo has a clean `.gitignore` and zero build artifacts committed.
- [ ] `npm test` is the only command a reviewer needs to validate the work.

## 1.4 Non-goals

- A web UI, REST API, persistence layer, or backend framework
  (Nest / Express / Fastify). The deliverable is a plain Node library.
- Bundling more than one preset. The library is designed so that new
  jurisdictions are a single new file under `presets/` — but I only ship
  the reference preset derived from the Java / C# code in this repo.
- Supporting languages other than TypeScript.
- Translating the original C# / Java code line-for-line.

## 1.5 Constraints I am imposing on myself

- **No premature abstraction.** The code must not look like a framework. A
  toll calculator does not need a plugin system.
- **No mocks of the SUT.** Tests exercise real code paths; only the clock /
  holiday calendar are injectable, and the test doubles for those are
  trivial.
- **No dependencies I cannot justify in a sentence.** Each entry in
  `package.json` must earn its place.
