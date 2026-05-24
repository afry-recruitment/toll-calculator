# 4. Bugs detected

Each bug is referenced by file and line number for direct navigation.

---

## 4.1 CRITICAL — Time diff calculation in C# is broken

**Location:** `C#/TollCalculator.cs:25-26`

```csharp
long diffInMillies = date.Millisecond - intervalStart.Millisecond;
long minutes = diffInMillies/1000/60;
```

`DateTime.Millisecond` returns **only the millisecond component (0–999)**, not
the epoch timestamp. The subtraction can never exceed ±999 ms, so `minutes`
is **always 0**, and the "once per hour" rule **never triggers correctly** —
the code always falls into the `minutes <= 60` branch.

**Fix:** use `(date - intervalStart).TotalMinutes`.

**Note:** The Java version (`Java/TollCalculator.java:22-23`) uses
`date.getTime()` correctly. The bug exists only in C#.

---

## 4.2 BUG — `intervalStart` is never advanced

**Location:** `Java/TollCalculator.java:15-32`, `C#/TollCalculator.cs:18-38`

`intervalStart` is set to `dates[0]` once and **never reassigned**. This means
the "one-hour window" is always measured against the **first pass of the
day**, not the last billed pass. For a day with passes at 06:15, 07:30, and
09:00, every comparison is against 06:15 — the logic is completely skewed.

**Fix:** when `minutes > 60`, set `intervalStart = date` to start a new window.

---

## 4.3 BUG — Fee table conditions are broken

**Location:** `Java/TollCalculator.java:59`, `C#/TollCalculator.cs:66`

```java
else if (hour >= 8 && hour <= 14 && minute >= 30 && minute <= 59) return 8;
```

This forgets the **00–29 minute slots of hours 9–14**, falling through to
`return 0`. So a pass at 10:15 — which should cost 8 SEK — is billed as free.

**Location:** `Java/TollCalculator.java:61`, `C#/TollCalculator.cs:68`

```java
else if (hour == 15 && minute >= 0 || hour == 16 && minute <= 59) return 18;
```

Operator precedence parses this as
`(hour == 15 && minute >= 0) || (hour == 16 && minute <= 59)`. The `15:xx`
side overlaps with the previous branch (already returned 13 for 15:00–15:29)
and is unreachable. The intent was almost certainly
`(hour == 15 && minute >= 30) || hour == 16`.

---

## 4.4 BUG — No defensive handling for `dates`

**Location:** `Java/TollCalculator.java:15`, `C#/TollCalculator.cs:18`

If `dates` is empty, `dates[0]` throws `ArrayIndexOutOfBoundsException` /
`IndexOutOfRangeException`. If `dates` is not sorted chronologically, the
algorithm produces meaningless results. There is no null check, no empty
check, no sort.

---

## 4.5 BUG — Totals can go negative

**Location:** `Java/TollCalculator.java:26`, `C#/TollCalculator.cs:30`

```java
if (totalFee > 0) totalFee -= tempFee;
```

When `tempFee > totalFee` (e.g. the first pass of the day was expensive and a
later window contains a different combination), `totalFee` becomes negative.
The `> 0` guard is insufficient — it should clamp:
`totalFee = Math.max(0, totalFee - tempFee)`.

---

## 4.6 BUG — Holiday calendar hard-coded to year 2013

**Location:** `Java/TollCalculator.java:77`, `C#/TollCalculator.cs:82`

```java
if (year == 2013) { ... }
```

The reference's `isTollFreeDate` only knows the toll-free days for year
2013. For any other year, **Easter, Pentecost, Midsummer, and every
other movable feast are billed as normal days**.

The list is also a flat enumeration of specific calendar dates —
including eve-of-holiday entries such as Apr 30, Jun 5, Nov 1, Dec 24,
Dec 31 as individual hard-coded entries rather than derived from any
rule. So even if the `year == 2013` guard were lifted, there is no
algorithm underneath to generalize.

**Fix direction:** drop the year guard and compute toll-free days from
a general rule for any Gregorian year. The concrete rule set — which
holidays apply, which eves are taxed, the special July clause — is a
question about Swedish law that this implementation answers in the
plan, not here.

---

## 4.7 BUG — Vehicle exemption by stringly-typed comparison

**Location:** `Java/TollCalculator.java:37-46`, `C#/TollCalculator.cs:43-53`

```java
vehicleType.equals(TollFreeVehicles.MOTORBIKE.getType())
```

The exemption list compares the `Vehicle.getType()` return value (a `String`)
against enum-derived strings. Any typo in a new subclass (e.g. `"motorbike"`
in lowercase) silently breaks the exemption without any compile-time signal.

**Fix:** make "toll-free" a property of the `Vehicle` (e.g.
`vehicle.isTollFree()`) or model the vehicle type as a typed enum.

---

## 4.8 BUG — Public `getTollFee(Date, Vehicle)` bypasses the daily logic

**Location:** `Java/TollCalculator.java:48`, `C#/TollCalculator.cs:55`

The single-pass overload is `public`. Callers can invoke it directly and
completely skip the once-per-hour and 60-SEK-cap rules. It should be
`private`.

---

## 4.9 Combined effect

Bugs 4.1, 4.2, and 4.5 interact: the daily cap of 60 SEK happens to clamp the
output, which means the function "looks right" on the happy path but the
intermediate totals are wrong, and certain inputs produce **negative fees**
or **uncapped windows**. A trivial unit test on a multi-pass day would have
exposed all three.
