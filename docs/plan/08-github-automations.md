# 8. GitHub Actions — automation layer

A dedicated automation surface lives in `.github/`. It is treated as
**product code**, not as an afterthought: workflows are linted, pinned, and
documented. The goal is that the repo polices itself so reviewers only
ever look at *intentional* signal.

## 8.1 What "automation layer" means here

Five concerns, each with one workflow file:

| File                              | Purpose                                                |
| --------------------------------- | ------------------------------------------------------ |
| `.github/workflows/ci.yml`        | Lint + typecheck + test on every push / PR             |
| `.github/workflows/coverage.yml`  | Coverage report as PR comment + summary                |
| `.github/workflows/codeql.yml`    | Static security analysis (GitHub-hosted)               |
| `.github/workflows/release.yml`   | Tag + GitHub Release on `v*` push (manual cut)         |
| `.github/workflows/stale.yml`     | Close abandoned PRs/issues after N days                |

Plus three configuration files:

| File                              | Purpose                                                |
| --------------------------------- | ------------------------------------------------------ |
| `.github/dependabot.yml`          | Weekly bumps for npm + actions                         |
| `.github/CODEOWNERS`              | Auto-request review from me on any src/ change         |
| `.github/pull_request_template.md`| Forces the PR description from plan §7.3               |

## 8.2 Conventions for every workflow

- **Pin actions by SHA**, not by tag. Tags are mutable, SHAs are not. Both
  are kept fresh by Dependabot.
- **Set `permissions:` at the workflow level** to the minimum. Default is
  `read`; specific jobs widen as needed.
- **Cache `~/.npm`** keyed by `package-lock.json` hash.
- **Concurrency group** per ref so a new push cancels the previous run.
- **Single Node version** (20) — matches the runtime declared in
  `package.json` `engines`.

## 8.3 `ci.yml` — the gate

Reflects what actually ships in `.github/workflows/ci.yml`. The job
working directory is `TypeScript/` because the package lives in a
sub-folder of the repo (the `Java/` and `C#/` reference folders share
the same root).

```yaml
name: CI

on:
  push:
    branches: [master]
  pull_request:
    branches: [master]

concurrency:
  group: ci-${{ github.ref }}
  cancel-in-progress: true

jobs:
  check:
    name: Type-check, lint, format and test
    runs-on: ubuntu-latest
    defaults:
      run:
        working-directory: TypeScript
    steps:
      - uses: actions/checkout@v4
      - run: corepack enable                    # provisions pnpm 10
      - uses: actions/setup-node@v4
        with:
          node-version: 20
          cache: pnpm
          cache-dependency-path: TypeScript/pnpm-lock.yaml
      - run: pnpm install --frozen-lockfile
      - run: pnpm run typecheck
      - run: pnpm run lint
      - run: pnpm run format:check
      - run: pnpm run test:coverage
      - uses: actions/upload-artifact@v4
        if: always()
        with:
          name: coverage-report
          path: TypeScript/coverage/
          retention-days: 7
```

The CI job is **the** required status check for branch protection.

## 8.4 `coverage.yml` — visible feedback

- Triggered by `workflow_run` after `ci.yml` succeeds on a PR.
- Downloads the `coverage/` artifact.
- Posts (or updates) a single PR comment with a summary table.
- Fails the workflow if line coverage drops below the threshold declared in
  `vitest.config.ts` (90%).

Why a separate workflow: keeps `ci.yml` fast and read-only; the comment
needs `pull-requests: write`, which we do not want in the gate.

## 8.5 `codeql.yml` — security baseline

- GitHub's default JS/TS CodeQL template.
- Runs on PR + weekly cron.
- Out-of-the-box value; zero maintenance.

## 8.6 `release.yml` — manual cut

- Triggered on tag push matching `v*`.
- Builds the package, runs the full check, creates a GitHub Release with the
  changelog generated from commits since the previous tag (Conventional
  Commits already in place from plan §7.2).
- No npm publish — this is a single deliverable, not a published package.
  Adding the publish step later is a one-liner once an npm token is
  available.

## 8.7 `stale.yml` — hygiene

- Marks PRs/issues stale after 30 days of inactivity, closes after 7 more.
- Skipped for anything labeled `pinned` or `security`.
- Almost certainly never fires during the review cycle, but it costs
  nothing and shows the discipline.

## 8.8 `dependabot.yml`

```yaml
version: 2
updates:
  - package-ecosystem: npm
    directory: "/"
    schedule: { interval: weekly }
    open-pull-requests-limit: 5
    groups:
      dev-deps:
        dependency-type: development
  - package-ecosystem: github-actions
    directory: "/"
    schedule: { interval: weekly }
```

Dev deps are grouped to avoid PR noise.

## 8.9 Branch protection (configured in repo settings)

Documented here so the configuration is reproducible:

- `master` requires:
  - PR before merge.
  - 1 approval from a reviewer.
  - `CI / check` status green.
  - `CodeQL` analysis green.
  - Linear history (no merge commits).
  - Dismiss stale reviews on new commit.
- Force pushes: disabled.
- Deletions: disabled.

These settings are not in code (GitHub doesn't expose them in workflow
files), but they are documented and can be replayed via the `gh` CLI.

## 8.10 `CODEOWNERS`

```
# Anything in src/ requires my review.
/src/        @<my-github-handle>
/.github/    @<my-github-handle>
/docs/       @<my-github-handle>
```

## 8.11 PR template

`.github/pull_request_template.md` mirrors the structure from
[`07-delivery-and-pr.md`](07-delivery-and-pr.md) §7.3 so every future PR
shows: **What**, **Why**, **Bugs in reference (if any)**, **Decisions**,
**Assumptions**, **How to run**.

## 8.12 Local mirror of CI

`pnpm check` runs the **exact** sequence CI runs:
`typecheck → lint → format:check → test:coverage`. No "works on my
machine" surprises. The `check` script in `package.json` is intentionally
the same composition CI invokes step-by-step, so local green ⇒ CI green.

## 8.13 What this layer is *not*

- It is not a Kubernetes pipeline. No deploy step, no env matrix.
- It is not a release-please / changesets setup. The project is a single
  package, one release per tag, no semver automation.
- It is not exhaustive — additions (Slack notifications, preview deploys,
  performance benchmarks) are deliberately out of scope for this
  deliverable.

## 8.14 Cost vs. value

| Workflow      | Setup cost | Long-term value | Decision |
| ------------- | ---------- | --------------- | -------- |
| `ci.yml`      | 15 min     | Mandatory       | **In**   |
| `coverage.yml`| 20 min     | High signal     | **In**   |
| `codeql.yml`  | 5 min      | Free baseline   | **In**   |
| `release.yml` | 20 min     | Real when shipping | **In** (light version) |
| `stale.yml`   | 5 min      | Low but free    | **In**   |
| `dependabot`  | 5 min      | High            | **In**   |
| `CODEOWNERS`  | 2 min      | Required for review automation | **In** |
| PR template   | 5 min      | Sets the tone   | **In**   |

Total: ~80 minutes of setup, mostly during phase 6 of
[`06-implementation-phases.md`](06-implementation-phases.md). I will fold
this work into that phase and bump its budget from 45 minutes to ~1h45m.

## 8.15 What shipped vs. what stayed as roadmap

Sections 9.1–9.14 describe the **target** automation surface. For
this deliverable, only the load-bearing piece shipped:

| File                                | Shipped? | Note                                                                |
| ----------------------------------- | -------- | ------------------------------------------------------------------- |
| `.github/workflows/ci.yml`          | ✅       | The required gate. Mirrors `pnpm check` exactly.                    |
| `.github/workflows/coverage.yml`    | ❌ roadmap | CI already uploads the coverage artifact (§8.3); a separate PR-comment workflow is the next step but not needed to merge. |
| `.github/workflows/codeql.yml`      | ❌ roadmap | One-liner enable in repo settings; left out to keep PR diff focused on the deliverable. |
| `.github/workflows/release.yml`     | ❌ roadmap | Not a published package today (`package.json` has no `dist/` build). |
| `.github/workflows/stale.yml`       | ❌ roadmap | Cosmetic; this is a single PR, no backlog hygiene need.             |
| `.github/dependabot.yml`            | ❌ roadmap | Sensible default but out of scope for this review.                   |
| `.github/CODEOWNERS`                | ❌ roadmap | Single-author repo; nobody to route reviews to.                      |
| `.github/pull_request_template.md`  | ❌ roadmap | The PR body uses the template from §7.3 manually; auto-enforcing it is a follow-up. |

The split is deliberate: §8.1–§8.13 documents the **discipline** I would
bring to a production repo; §8.15 documents what was load-bearing for
*this* deliverable. The reviewer can read either as the level of
ambition matches the conversation we have in the follow-up meeting.
