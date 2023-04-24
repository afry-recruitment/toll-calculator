## Assumptions
This code is never going to see the light of day, so I will just keep it simple for all our sakes, ie. not making it a big project such as a WebApi or adding databases with service and repositores etc.

Instead I will focus on the actual functions of calculating the toll-fees, keeping them as accurate to a real-life scenario where databases would be involved and prove the use-cases and stability through testing

Being able to run tests should showcase that the code is functional

## Imagined scenario
Every time a vehicle passes a toll, a record of that moment is saved containing the toll fee amount, the time and the vehicles license plate.

This record would be saved somewhere appropriate, and once the tolls close for the day at 18:30 the system starts to calculate the total daily toll fee amounts for all vehicles.

## Requirements and line of thinking

### 1. Fees will differ between 8 SEK and 18 SEK, depending on the time of day 

- Since requirment explicitly states that SEK will be the currency used, I will not consider other currencies or cultureinfo

### 2. Rush-hour traffic will render the highest fee

- Will use the toll fees for hours that already exists in the GetTollFee function in TollCalculator.cs

### 3. The maximum fee for one day is 60 SEK

### 4. A vehicle should only be charged once an hour

### 4.1 In the case of multiple fees in the same hour period, the highest one applies.

- The solution here will be the core logic problem to solve in this assignment, spend extra time to make it readable and pretty

### 5. Some vehicle types are fee-free

- Attach a bool value for different vehicle types

### 6. Weekends and holidays are fee-free

- I will be using the PublicHoliday NuGet package https://github.com/martinjw/Holiday which has been regularly maintained since 2013 and has no reported vulnerabilities

## BONUS POINTS:

Its from the movie Hackers (1995)
