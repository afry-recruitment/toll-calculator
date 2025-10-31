# Assignment

I have got the assignment to have a look at a code and make any modifications or suggestions for modifications.

# Solution Description

The solution has been developed in IntelliJ with Java 25 by Laszlo Karsai.

## Implemented modifications

Let me summarize what modifications I have managed to do to the original code.

### 1. Package structure

A simple folder structure has been created, and I have moved the pre-existing Java files to different packages.

### 2. Build system

I have chosen Gradle as build system because it is flexible and easy to start with and understand.

You can use the gradle commands from the project folder or utilise the gradlew(.bat) executable.

Why do we need a build system?

- To run test cases to check if the code works correctly
- To start an application
- To build tha application for deployment
- etc

### 3. Refactored Vehicle model

I saw that the original code had the followings:
- an enum called `TollFreeVehicles`
- an interface called `Vehicle`
- a class called `Car`
- a class called `Motorbike`

I decided to refactor this modelling of the vehicles.

I chose to implement an enum called `VehicleType` containing the type and a flag if it is toll-free or not.

I thought about two other similar ways:
1. Keeping the `Vehicle` interface and continue to implement all the other different vehicle type based on eg `Car`
   -> I would have added a `public boolean isTollFree();` function to the interface.
2. Keeping the `Vehicle` interface and implementing in only two classes `TollFreeVehicle` and `TollVehicle`
   -> I would have defined the interface as the option above.
   -> The model is only used to decide if a vehicle type is toll-free or not.
   -> The type (if it is a car or motorbike) does not really matter unless we want to use these values as input.
   -> However, I concluded it might be important in a later stage to use the specific type.

My reasons for choosing the enum model over interface:
- The specific vehicle type list does not change, it is constant list that we do not edit dynamically.
- No logic or complex functions are needed inside the model, only the flag if the vehicle type is toll-free or not.
- Bonus: it is so much simpler and nicer to define one file than one interface and all the classes.

### 4. Holidays

As written below in the modification suggestions, it would be time-consuming to collect all holidays.
I kept some examples of holiday dates from the year 2013 as it was initiated in the original code.

I have done two changes to the original list of holidays:
- I have fixed the list of holidays from the year 2013 as the original code examples were not all correct.
- I have added the holiday dates as file so that it could be easier to work with them eg adding other years as well.

I used [this](https://www.timeanddate.com/holidays/sweden/2013?hol=1) to list the Swedish public holidays in year 2013.

### 5. Logic for one date toll fee calculation

The original logic for hours 9, 10, 11, 12, 13 and 14 with minutes between 0 and 29 would give back 0 SEK toll fee.
And one minute later from the 30th minute of the same hour, it would give back 8 SEK.
I do not think the design was meant to leave 30 mins of free periods in the middle of the day.
So, I think from 08:30 to 14:59, the fee should be the same (8 SEK).

I have added test case `givenCarVehicleTypeAndEachHourDates_whenCalculation_thenSuccess` to verify my change.

### 6. Logic for total toll fee calculation

I have found some issues with the total fee calculation:
- The dates should be sorted to make sure the list is always handled the same way.
- For the one-hour window, it is not enough to decide based on the whole minutes difference, even millis should count.
  -> For example, 15:00:00 and 16:00:01 would be one-hour period according to the original code but it exceeds it.
- For the one-hour window, we should not always take the first date to calculate the 60 minutes period
  -> We should use a changing reference date instead.

I have added test case `givenCarVehicleTypeAndHourPeriodDates_whenCalculation_thenSuccess` to verify my change.

### 7. Parameterized tests for with different vehicle types, days and time periods

I have created a test case to run multiple tests trying to cover most of the different scenarios.
Its name is `givenDifferentVehicleTypesAndDaysAndTimePeriods_whenCalculation_thenSuccess`.

The test case has some columns:
- vehicleType -> The vehicleType which is used for the tests. 
  -> Applied values: car (toll paying) and motorbike (toll-free)
- day -> The day for which the tests are executed. 
  -> Applied values: 2013-06-03 (normal day), 2013-06-06 (holiday), 2013-06-08 (Saturday), 2013-06-09 (Sunday)
- t1 -> A flag indicating whether a date with time before 06:00 should be applied.
- t2 -> A flag indicating whether a date with time between 06:00 and 06:29 should be applied.
- t3 -> A flag indicating whether a date with time between 06:30 and 06:59 should be applied.
- t4 -> A flag indicating whether a date with time between 07:00 and 07:59 should be applied.
- t5 -> A flag indicating whether a date with time between 08:00 and 08:29 should be applied.
- t6 -> A flag indicating whether a date with time between 08:30 and 14:59 should be applied.
- t7 -> A flag indicating whether a date with time between 15:00 and 15:29 should be applied.
- t8 -> A flag indicating whether a date with time between 15:30 and 16:59 should be applied.
- t9 -> A flag indicating whether a date with time between 17:00 and 17:59 should be applied.
- t10 -> A flag indicating whether a date with time between 18:00 and 18:29 should be applied.
- t11 -> A flag indicating whether a date with time after 18:30 should be applied.
- expectedTollFee -> The expected total fee for that day with the applied times.

## Modification suggestions

Let me summarize the modifications I would like to suggest.

### Holidays

I would suggest not to have the holiday as a strong requirement.

Reason: it would take a lot of time (basically forever) to collect all the holidays from all years. 
The holiday dates can differ between countries as well.

I think we would need to maintain a static date list of holidays in each year to be able to support this requirement.

I will keep the year 2013 holidays in the code so that I could write tests for this scenario as well.