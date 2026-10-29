# Toll fee calculator (C#)

```
dotnet test
```

## Structure
- `src/TollFeeCalculator` – class library (.NET 10)
  - `TollCalculator` – total fee for a vehicle's passages (hourly rule + daily cap)
  - `TollFeeSchedule` – fee per time of day as a data table
  - `SwedishHolidays` – Swedish public holidays calculated for any year (incl. Easter)
  - `Vehicle` / `VehicleType` – vehicle type, and whether it is toll-free
- `tests/TollFeeCalculator.Tests` – xUnit tests

## Rules as implemented
- Fees 8–18 SEK by time of day; 0 SEK between 18:30 and 05:59.
- Toll-free: weekends, public holidays, the day before a public holiday, and all of July.
- Toll-free vehicles: Motorbike, Tractor, Emergency, Diplomat, Foreign, Military.
- Hourly rule: a window opens at the first chargeable passage and lasts 60 minutes
  (a passage exactly 60 minutes later opens a new window). Only the highest fee in each window is charged.
  Free passages (e.g. 05:50) do not open a window.
- Max 60 SEK per calendar day. Passages may be unordered and span several days; the cap applies per day.
- Passages are expected in local Swedish time.

## Bugs fixed from the previous version
1. **Time difference used `DateTime.Millisecond`** (the 0–999 ms component), so every passage on a day
   fell into the same "hour". Now uses the actual `TimeSpan` between passages.
2. **Hourly-window logic was wrong**: the window start was never moved forward, and the running total
   subtracted/added the wrong fee, which could over- or under-charge.
3. **Gap in the fee table**: `hour >= 8 && hour <= 14 && minute >= 30` returned 0 SEK for
   09:00–09:29, 10:00–10:29 … 14:00–14:29.
4. **Holidays hardcoded for 2013 only**: every other year only had weekends free. They are now calculated
   (the result matches the old 2013 list exactly – see tests).
5. **Vehicle types compared as strings** and `null` vehicles were silently charged. Now an enum, and `null`
   arguments throw `ArgumentNullException`.
6. **Empty input crashed** (`dates[0]`); unsorted input and passages from multiple days gave wrong results.
7. Project setup: no project/solution files and no tests; `TollCalculator` was in the global namespace.

## Suggestions / open questions for the city
- Should the fee schedule, holidays and toll-free vehicle types be configurable (e.g. from config/database)
  instead of being in code, so changes don't require a deploy?
- Confirm the hourly-window interpretation (window from first passage vs. fixed clock hours) and whether
  a passage exactly 60 minutes later counts as a new hour.
- `DateTime` without time zone is ambiguous around DST changes; consider `DateTimeOffset` at the API boundary.
