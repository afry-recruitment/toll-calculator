# 7. Delivery checklist

A practical checklist before delivering the PR.

## Before writing code

- [ ] Re-read `README.md` end-to-end.
- [ ] Confirm the fee table from [03-business-rules.md](03-business-rules.md).
- [ ] Decide language and stick with it.
- [ ] Decide whether to translate the reference code (do not) or rewrite from
      the README (do).

## Code

- [ ] Domain model: `Vehicle`, `Pass`, `FeeSchedule`, `HolidayCalendar`,
      `TollCalculator`.
- [ ] Vehicle exemption is a property of the vehicle, not a string lookup.
- [ ] Fee table is data, not a chain of `if`/`else`.
- [ ] Holiday calendar is injectable; supports any year.
- [ ] Time arithmetic uses the modern date/time API of your language with a
      timezone.
- [ ] Windowing logic restarts `window_start` correctly.
- [ ] Daily cap of 60 SEK applied at the end via `min`.
- [ ] No negative totals possible.
- [ ] Empty / null / unsorted input handled defensively.

## Tests

- [ ] At least the 10 scenarios listed in
      [06-recommendations.md](06-recommendations.md) §6.5.
- [ ] Tests pass locally with one command.
- [ ] Coverage report optional but appreciated.

## Repo hygiene

- [ ] `.gitignore` appropriate for the chosen language.
- [ ] Build/run instructions in your `README.md`.
- [ ] No build artifacts committed.
- [ ] One feature per commit, meaningful messages.

## PR description

In the PR, explicitly list:

1. The bugs you found in the reference code (link to lines).
2. The design decisions you made (why a strategy/table for fees, why a
   separate calendar, etc.).
3. How to run the tests.
4. Assumptions you made where the README is silent (e.g. timezone, behavior
   on empty input, treatment of the day before a holiday).

That last point is what most submissions skip — and the one the
reviewer is specifically looking for.
