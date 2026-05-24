# 1. Project overview

This repository is a small **coding exercise**: an urban toll-fee
calculator for an unnamed city. The only locale signal in the brief
is the currency — **fees in SEK**. The reference Java / C# code
hard-codes a 2013-specific holiday list whose dates match Swedish
public holidays (though the code never names a country or city).

## What the deliverable is

A working implementation — in the language of choice — delivered via
fork + pull request. The existing C# and Java code is provided as a
**deliberately flawed reference**: the README hints at this explicitly
("the last city-developer quit, claiming this solution is
production-ready").

## What the exercise really measures

This is not a clean starting base — it is a **critical-reading
exercise**. The reviewer is looking for the ability to:

- Detect the bugs that are obviously planted in the code.
- Restate the business rules in a clean implementation.
- Add tests that would have caught the bugs.
- Justify design decisions in the PR description.

## Headline numbers

| Metric                | Value                                    |
| --------------------- | ---------------------------------------- |
| Source files          | 9 (`.java` + `.cs`)                      |
| Lines of useful code  | ~210                                     |
| Test files            | 0                                        |
| Build files           | 0 (no `pom.xml`, `.csproj`, `Makefile`)  |
| Functional bugs found | At least 6 (see [04-bugs.md](04-bugs.md))|
