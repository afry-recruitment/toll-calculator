![here we are](https://media.giphy.com/media/v1.Y2lkPWVjZjA1ZTQ3MnQ0NWVwbjkwcmY2MHM2aDc0dTU2eHAzbXN2NTRkY3ZscXpkYm9vZiZlcD12MV9naWZzX3NlYXJjaCZjdD1n/YVGeZszGz4eC4/giphy.gif)

# Requirements
This solution uses the library "Public Holidays" to calculate the fees: https://www.nuget.org/packages/PublicHoliday


# Toll fee calculator 2.0
A calculator for vehicle toll fees.

# Usage
Instantiate the TollCalculatorGothenburg class and use the GetTollFee or GetTollFeeForDay method to calculate the fee.
Alternatively used the TollCalculator interface to make your own class or make a child class of TollCalculatorGothenburg.

PS: Please don't put this code on a mixtape to hack the toll collectors via payphone, Hackers is not as applicable today as the 90s.

# Toll Callculation Basis
It is calculated based on these assumptions.
* Every swedish holiday is toll free
* Every Saturday and Sunday are toll free
* The month of july is toll free
* The Toll schedule for all other days is the following:
	- 00:00 - 05:59   - FREE
    - 06:00 - 06:29   - 08
    - 06:30 - 06:59   - 13
    - 07:00 - 07:59   - 18
    - 08:00 - 08:29   - 13
    - 08:30 - 14:59   - 08
    - 15:00 - 15:29   - 13
    - 15:30 - 16:59   - 18
    - 17:00 - 17:59   - 13
    - 18:00 - 18:29   - 08
    - 18:30 - 23:59   - FREE

# Structure
* TollCalculator.cs
    - Contains the Interface the Tollcalculators implementation is based on
    - Contains the Enum VehicleType for representing each possible vehicle used in the calculator
* FeeSpan.cs
    - Contains a Class for containing the Timestamps for eachstart time of the fee and the corresponding fee
* TollCalculatorGothenburg.cs
    - Contains the Class implementation for the Gothenburgs Toll calculations.

  