# How to Run the Tests

## Quick Start

From the `C#` directory, run:

```bash
dotnet test TollCalculator.Tests/TollCalculator.Tests.csproj --logger "console;verbosity=detailed"
```

Or for a simpler output:

```bash
dotnet test TollCalculator.Tests/TollCalculator.Tests.csproj
```

## Test Results Summary

**Current Status: 6 out of 10 tests FAILING ❌**

### ❌ Failing Tests (Demonstrating Bugs):

1. **PassesAt61Minutes_ShouldStartNewWindow**
   - Expected: 26 SEK (8 + 18)
   - Actual: 18 SEK
   - **Issue**: Time difference calculation is broken

2. **PassesMoreThan60MinutesApart_ShouldChargeSeperately**
   - Expected: 26 SEK
   - Actual: 18 SEK
   - **Issue**: All passes treated as same window due to millisecond bug

3. **MultipleSlidingWindows_ShouldChargeCorrectly**
   - Expected: 26 SEK
   - Actual: 23 SEK
   - **Issue**: Incorrect window tracking logic

4. **PassesWithin60Minutes_DifferentFees_ShouldChargeHighest**
   - Expected: 13 SEK (highest in window)
   - Actual: 18 SEK
   - **Issue**: Logic incorrectly accumulates fees

5. **ComplexScenario_WithMultipleWindows**
   - Expected: 44 SEK
   - Actual: 38 SEK
   - **Issue**: Multiple windows not handled correctly

6. **MaximumFeePerDay_ShouldBeCappedAt60**
   - Expected: 60 SEK (capped)
   - Actual: 33 SEK
   - **Issue**: Total fees not calculated correctly due to time calculation bug

### ✅ Passing Tests (By Accident):

4 tests pass, but often for the wrong reasons due to the broken time calculation making all passes appear to be in the same window.

## The Critical Bug

**Lines 29-30 in TollCalculator.cs:**

```csharp
long diffInMillies = date.Millisecond - intervalStart.Millisecond;
long minutes = diffInMillies / 1000 / 60;
```

This code uses `.Millisecond` (which returns only 0-999) instead of calculating the full time difference. This means:
- A pass at 6:00:00.500 and 7:30:00.100 calculates as 500-100 = 400 milliseconds = 0 minutes
- The actual difference is 90 minutes!

## What the README Says

From the requirements:
- **"A vehicle should only be charged once an hour"**
- **"In the case of multiple fees in the same hour period, the highest one applies"**

This means within any 60-minute sliding window, only the highest toll fee should be charged.

## Correct Implementation Should:

1. Calculate the actual time difference between passes (not just milliseconds)
2. Group passes into 60-minute windows
3. Charge only the highest fee within each window
4. Start a new window when a pass is more than 60 minutes from the current window start
5. Cap the total daily fee at 60 SEK

