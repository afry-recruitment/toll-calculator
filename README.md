![here we are](https://media.giphy.com/media/FnGJfc18tDDHy/giphy.gif)

# Toll fee calculator 1.0
A calculator for vehicle toll fees.

* Make sure you read these instructions carefully
* The current code base is in Java and C#, but please make sure that you do an implementation in a language **you feel comfortable** in like Javascript, Python, Assembler or [ModiScript](https://en.wikipedia.org/wiki/ModiScript) (please don't choose ModiScript). 
* No requirement but bonus points if you know what movie is in the gif

## Background
Our city has decided to implement toll fees in order to reduce traffic congestion during rush hours.
This is the current draft of requirements:
 
* Fees will differ between 8 SEK and 18 SEK, depending on the time of day 
* Rush-hour traffic will render the highest fee
* The maximum fee for one day is 60 SEK
* A vehicle should only be charged once an hour
  * In the case of multiple fees in the same hour period, the highest one applies.
* Some vehicle types are fee-free
* Weekends and holidays are fee-free

## Your assignment
The last city-developer quit recently, claiming that this solution is production-ready. 
You are now the new developer for our city - congratulations! 

Your job is to deliver the code and from now on, you are the responsible go-to-person for this solution. This is a solution you will have to put your name on. 

## Instructions
You can make any modifications or suggestions for modifications that you see fit. Fork this repository and deliver your results via a pull-request. You could also create a gist, for privacy reasons, and send us the link.

## Help I dont know C# or Java
No worries! We accept submissions in other languages as well, why not try it in Go or nodejs.

# Solution
## Changes made
* `TollCalculator.GetTollFee(Vehicle, DateTime[])` now handles one day of passages. It sorts the passages, charges the highest fee within each one-hour interval starting from the first passage, and caps the day's total at 60 SEK.
* Passages on more than one day, or a `null` array, now throw an exception. An empty array returns 0.
* `Vehicle` is now an abstract base class that validates the license plate (exactly 6 characters) and stores it as `LicensePlate`. `Car` is charged. `Motorbike`, `Emergency`, `Tractor`, `Diplomat`, `Foreign`, and `Military` are toll-free.
* Fee-free dates: weekends and Swedish public holidays (including Midsummer Day, which is computed from the calendar since it is not fixed).
* Added `TollCalculator.csproj` (class library, `net11.0`) and `Program.cs` with example usage, including a fake in-memory database that stores license plate, date, and total fee.
* Added an xUnit test project in `C#/Tests` with 17 tests covering fees, the daily cap, input order, fee-free vehicles and dates, and validation.
* Added a `.gitignore` for .NET build output and Visual Studio user files.

## Open requirement question
* The requirements do not say whether the day before a holiday is fee-free. **This library assumes it is not** (it is charged). This needs to be clarified with the city before release.

## Running the tests (C#)
The tests use xUnit and target .NET 11, so you need the [.NET 11 SDK](https://dotnet.microsoft.com/download) installed.

From the repository root, run:

```
cd C#/Tests
dotnet test
```

## Running the example program (C#)
`C#/TollCalculator.csproj` is a class library (`<OutputType>Library</OutputType>`), so `dotnet run` won't run `Program.cs`. To run it, change the project type to an executable first:

1. In `C#/TollCalculator.csproj`, change `<OutputType>Library</OutputType>` to `<OutputType>Exe</OutputType>`.
2. From `C#`, run `dotnet run`.
3. Change it back to `Library` before running the tests or building the library for other use.


