# 3. Business rules

Rules as declared in `README.md`:

| Rule                      | Detail                                                                                  |
| ------------------------- | --------------------------------------------------------------------------------------- |
| Fee range                 | Between **8 and 18 SEK**, depending on the time of day                                  |
| Rush hour                 | Renders the highest fee (18 SEK)                                                        |
| Daily cap                 | **60 SEK** maximum per day                                                              |
| Hourly window             | A vehicle is charged **at most once per hour**; the highest fee in the window applies   |
| Vehicle exemptions        | Some vehicle types are toll-free                                                        |
| Date exemptions           | Weekends and holidays are toll-free                                                     |

## Fee table inferred from the source

Reconstructed from the `if`/`else` chain in `TollCalculator.java:55-64` and
`TollCalculator.cs:62-71`:

| Time window     | Fee (SEK) |
| --------------- | --------- |
| 06:00 – 06:29   | 8         |
| 06:30 – 06:59   | 13        |
| 07:00 – 07:59   | 18        |
| 08:00 – 08:29   | 13        |
| 08:30 – 14:59   | 8         |
| 15:00 – 15:29   | 13        |
| 15:30 – 16:59   | 18        |
| 17:00 – 17:59   | 13        |
| 18:00 – 18:29   | 8         |
| Everything else | 0         |

> **Important:** The implementation does **not** match this table — see
> [04-bugs.md](04-bugs.md) §4.3.

## Toll-free vehicle types

- Motorbike
- Tractor
- Emergency
- Diplomat
- Foreign
- Military

## Toll-free dates (hard-coded for 2013 only)

Jan 1; Mar 28–29; Apr 1, 30; May 1, 8, 9; Jun 5, 6, 21; the whole of July;
Nov 1; Dec 24, 25, 26, 31; plus every Saturday and Sunday.

The 2013-only scope is itself a defect — see [04-bugs.md](04-bugs.md) §4.6.
