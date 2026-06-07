

# Toll fee calculator 1.0.1
A calculator for vehicle toll fees.

## Library usage 
See in the tests:
```C#
var calculator = new TollCalculator(/*config*/);
_result = calc.GetTollFee(_vehicle, _dates);
```

or with the included `builder` for convenience, also see in tests.

## Testing:
See [reqnroll](https://docs.reqnroll.net/latest/quickstart/index.html) docs for details.

Otherwise run the tests with Visual Studio Test Explorer or `dotnet test`

---

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

