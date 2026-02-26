# Bug Fixes & Tests

## What I Fixed

Found and fixed some bugs in the existing toll calculator code. Added tests to make sure everything works.

### Bug 1: Time calculation was wrong
The code was using `.Millisecond` which only gives you 0-999, not the actual time between two dates. Changed it to use `TimeSpan` to get the real difference in minutes.

### Bug 2: Forgot to update the time window
When starting a new 60-minute window, the code never updated `intervalStart`. This made it compare everything to the very first passage instead of starting fresh windows.

### Bug 3: Wrong time range for 8:30-14:59
The if-statement logic was checking both hour AND minute ranges incorrectly, so times like 9:00 or 10:30 didn't match and returned 0 instead of 8 SEK.

### Bug 4: Wrong fee for 15:00-15:29
Operator precedence issue - it was charging 18 SEK when it should charge 13 SEK for this time slot.

### Bug 5: Fees adding up wrong
The logic for keeping track of fees within the same window wasn't working right. Fixed it to properly track the highest fee per window and only add to total when the window closes.

## Tests I Added

Created `TollCalculatorLegacyTests.cs` with tests for:
- All the bugs I fixed
- Empty and null inputs
- Weekends and holidays
- Daily cap (60 SEK max)
- Different times of day
- Multiple passages scenarios

About 30 tests total covering the main cases.
