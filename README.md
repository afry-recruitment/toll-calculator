## Assumptions
This code is never going to see the light of day, so I will just keep it simple for all our sake, ie. not creating an actual useable program such as a WebApi 

Being able to run tests should showcase that the code is functional

Code should function on its own without use of external libraries or APIs

## Requirements and line of thinking

### 1. Fees will differ between 8 SEK and 18 SEK, depending on the time of day 

- Since requirment explicitly states that SEK will be the currency used, I will not add extra code to set a specific culture (with currencies and the countries holidays) in order to keep the program lightweight and easy to read


### 2. Rush-hour traffic will render the highest fee

- Will use the toll fees for hours that already exists in the GetTollFee function in TollCalculator.cs


### 3. The maximum fee for one day is 60 SEK

- In the future the maximum fee may be increased or decreased, so save this in a constant, easy to implement and offers some flexibility to be able to change this value quickly and easily


### 4. A vehicle should only be charged once an hour

   4.1 In the case of multiple fees in the same hour period, the highest one applies.

- The solution here will be the core logic problem to solve in this assignment, spend extra time to make it readable and pretty


### 5. Some vehicle types are fee-free

- Attach a bool function for each vehicle type


### 6. Weekends and holidays are fee-free

- This one can be a bit tricky since different countries have different holidays, in this case we use SEK so it should be set to Swedish holidays. 

Obviously hard-coding it is a bad solution, but since I want to limit the scope of this assignment and not rely on external libraries or APIs it might be worth the trade off.

Could create a function that reads from a json file depending on the culture set for the program, but that would also increase the scope and possibly decrease readability.

According to my line of thinking in 1. I will simply hard-code them, since there is a requirement to use SEK and no requirement to adapt the program to different cultures.

Since holidays unfortunately dont occur the same date every year, I will set the holidays to the dates for 2023 and add a check to see if the year used in the dates for the toll calculation are the correct year.

## BONUS POINTS:

Its from the movie Hackers (1995)
