# Verification

Verified on **2026-11-07**, using the **.NET 8.0.427 SDK compiler** and
**.NET 8.0.33 runtime**.

## Calculator and tests

The production sources compiled into a separate library with nullable analysis
and warnings treated as errors. The actual test sources compiled against that
library and the real xUnit 2.9.2 assertion and core assemblies.

**61 expanded Fact/Theory cases passed; 0 failed.** These cases include:

- Every tariff boundary, one tick before, at, and one tick after the change.
- All 1,440 minutes, checked at the minute start and 59 seconds into it.
- All 365 dates in the supplied 2013 calendar.
- Fixed window anchoring, exactly 60 minutes, one tick beyond it, and finalization.
- 200 seeded random passage sets for ordering, duplication and daily-cap invariants.
- 200 seeded random passage sets for invariance when free passages are inserted.
- Invalid vehicles, null input, mixed days, timestamp kinds, unsupported years,
  input copying, single enumeration and validation before exemptions.

The exhaustive iterations above are inside the 61 test cases; they are not
additional independently discovered test cases.

## Runnable console

`src/TollFeeDemo/Program.cs` compiled as a separate executable referencing the
calculator library. All ten built-in examples ran successfully. Their daily
totals were checked against:

```text
36, 18, 26, 31, 60, 0, 0, 0, 0, 18 SEK
```

An additional **15 command-line checks passed**, covering custom times, fractional
seconds, case-insensitive vehicle names, duplicates, exempt vehicles/dates, help,
and invalid arguments. Invalid input returned exit code 2 and did not print a
partial daily total. All three project files were parsed as XML and their project
reference targets were checked.

## Environment limitation

The standard .NET CLI failed before building because it could not retrieve its
own process information from this execution environment. Disabling build servers
did not resolve that failure. Consequently, **standard `dotnet test` did not run**.

Compilation used the SDK's Roslyn compiler directly. Test execution used a
temporary reflection runner that discovered the supplied Fact/Theory tests,
expanded InlineData and MemberData, and invoked each case with a fresh test-class
instance. Assertions were provided by xUnit, not reimplemented. Compiler warnings
1701/1702 were suppressed for framework-reference unification, as in SDK builds.

This verifies the actual C# calculation, tests and executable behavior, but not
standard NuGet restore, MSBuild project integration, xUnit analyzer execution,
or Visual Studio test discovery. The README provides the normal `dotnet restore`,
`dotnet build` and `dotnet test` commands for a development machine.

The 96 external PRs were reviewed as source; they were not all built or tested.
