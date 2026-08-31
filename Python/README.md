# Toll fee calculator - Python implementation

A rewrite of the toll fee calculator in Python.

## Running 

    python -m pip install -r Python/requirements.txt
    python -m pytest Python -v

Run from the repository root. 27 tests.


## Usage

```python
from datetime import datetime
from toll_calculator import VehicleType, calculate_daily_fee

passages = [
    datetime(2026, 8, 31, 6, 15),
    datetime(2026, 8, 31, 6, 45),
    datetime(2026, 8, 31, 9, 0),
]

calculate_daily_fee(VehicleType.CAR, passages)   # 21

```
The first two passages fall within the same hour, so only the higher of the
two is charged: 13 kr. The passage at 09:00 opens a new window and adds 8 kr.


## Fee schedule

| From | To | Fee |
|------|------|-----|
| 06:00 | 06:30 | 8 kr |
| 06:30 | 07:00 | 13 kr |
| 07:00 | 08:00 | 18 kr |
| 08:00 | 08:30 | 13 kr |
| 08:30 | 15:00 | 8 kr |
| 15:00 | 15:30 | 13 kr |
| 15:30 | 17:00 | 18 kr |
| 17:00 | 18:00 | 13 kr |
| 18:00 | 18:30 | 8 kr |
| all other times | | 0 kr |

Intervals are half-open: start time is included, the end time belongs to the next interval. A passage at 06:30:00 costs 13 kr, not 8 kr.

## Changes from the original

A detailed walkthrough with line numbers is in [NOTES.md](../NOTES.md).

**The fee schedule has a gap.** Line 66 allows hours 8–14 but only minutes
30–59, so 09:00–09:29, 10:00–10:29 and so on cost nothing at all. I made
08:30–15:00 one continuous interval at 8 kr — off-peak daytime traffic gets
the lowest rate.

**Line 68 gives the right answer by accident.** `minute >= 0` is always true,
so the condition means the whole of hour 15 or the whole of hour 16. It only
works because line 67 sits above it and catches 15:00–15:29 first. Remove or
move that line and 15:00–15:29 silently changes from 13 kr to 18 kr. I wrote
the schedule as data — a list of intervals — rather than a chain of conditions.

**Vehicle types are compared as strings.** A lower-case letter gives the wrong
fee with no warning, and the enum and the vehicle classes are two lists that
must be kept in sync by hand. I used a `VehicleType` enum and a frozenset of
the exempt types, so there is one list instead of two.

**The time between passages is never measured.** Line 25 uses `.Millisecond`,
which is only the thousandths of a second, so the difference is always 0 and
every passage is treated as being within the same hour. I subtract the two
datetimes and compare against `timedelta(hours=1)`.

**The hour window never moves.** `intervalStart` is set on line 18 and never
updated inside the loop. In my version the window start is reset each time a
new window opens.

**The passage order is assumed, not checked.** I sort the passages before
calculating.

**An empty list crashes** on `dates[0]`, line 18, before any check runs. Mine
returns 0.

**Holidays are hardcoded to 2013**, line 82, so every public holiday since
then is charged as a normal day. I use the `holidays` package, which covers
every year.

For the passages 06:15, 06:45, 07:10 and 09:00 the original returns 23 kr.
The correct total is 26 kr — 18 kr for the hour window starting at 06:15,
plus 8 kr for the passage at 09:00.

## Assumptions

- All passages given to `calculate_daily_fee` are from the same day, as the requirements state. An hour window that would cross midnight is therefore not handled. With the current schedule that cannot produce a wrong fee,
since everything between 18:30 and 06:00 is free.
- The hour window runs from the first passage, not from the top of the clock hour. "Once an hour" is read literally.
- Timestamps may include seconds. Half-open intervals handle that without discarding data.
- Public holidays come from the `holidays` package rather than a list maintained in this repository.
- Fees are whole kronor, as in the original.

## Improvements
- The original also exempts all of July (line 89), which matches how the real Stockholm congestion tax works. The requirements I was given do not mention it, so I have not implemented it. This is a question for the customer rather than a decision for me to make. But this can be implemented and improve the system in the future.

- I installed the dependencies globally. In a real project I would use a virtual environment so they stay isolated to this project.