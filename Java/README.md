# Table of Contents

[1. Summary](#1)<br>
[2. Implemented modifications](#2)<br>
&emsp;[2.1. Package structure](#2.1)<br>
&emsp;[2.2. Build system](#2.2)<br>
&emsp;[2.3. Refactored Vehicle model](#2.3)<br>
&emsp;[2.4. Holidays](#2.4)<br>
&emsp;[2.5. Logic for one date toll fee calculation](#2.5)<br>
&emsp;[2.6. Logic for total toll fee calculation](#2.6)<br>
&emsp;[2.7. Tests and Build](#2.7)<br>
&emsp;[2.8. Parameterized tests for with different vehicle types, days and time periods](#2.8)<br>
&emsp;[2.9. Spring Boot - Backend application](#2.9)<br>
&emsp;[2.10. Logging](#2.10)<br>
&emsp;[2.11. React - Frontend application](#2.11)<br>
[3. Modification suggestions](#3)<br>
&emsp;[3.1. Holidays](#3.1)<br>

# 1. Summary <a id="1"></a>

This the Toll Fee Calculator application developed by Laszlo Karsai as an coding assignment.
I have been provided a base code, and then I could modify it as how I see fit.

I have managed to fix some algorithmic mistakes and created a well-tested web application from it.
I have also listed what possible future changes I would do it if I had more time.

For more information about my changes and ideas, please read below.

# 2. Implemented modifications <a id="2"/>

Let me summarize what modifications I have managed to do to the original code and how you can try them out.
Please, note that these changes are in chronological order.
During my development, I used JDK version 25.

## 2.1. Package structure <a id="2.1"/>

A simple folder structure has been created, and I have moved the pre-existing Java files to different packages.

## 2.2. Build system <a id="2.2"/>

I have chosen Gradle 9.1.0 as build system because it is flexible and easy to start with and understand.

You can use the gradle commands from the project folder or utilise the gradlew(.bat) executable.

Why do we need a build system?

- To run test cases to check if the code works correctly
- To start an application
- To build tha application for deployment
- etc

For the command below, please enter the `Java` or `Java/Frontend` folder of the repo.

You can use the Gradle wrapper in the repo to try it out:
```shell
./gradlew -version
```

## 2.3. Refactored Vehicle model <a id="2.3"/>

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

## 2.4. Holidays <a id="2.4"/>

As written below in the modification suggestions, it would be time-consuming to collect all holidays.
I kept some examples of holiday dates from the year 2013 as it was initiated in the original code.

I have done two changes to the original list of holidays:
- I have fixed the list of holidays from the year 2013 as the original code examples were not all correct.
- I have added the holiday dates as file so that it could be easier to work with them eg adding other years as well.

I used [this](https://www.timeanddate.com/holidays/sweden/2013?hol=1) to list the Swedish public holidays in year 2013.

## 2.5. Logic for one date toll fee calculation <a id="2.5"/>

The original logic for hours 9, 10, 11, 12, 13 and 14 with minutes between 0 and 29 would give back 0 SEK toll fee.
And one minute later from the 30th minute of the same hour, it would give back 8 SEK.
I do not think the design was meant to leave 30 mins of free periods in the middle of the day.
So, I think from 08:30 to 14:59, the fee should be the same (8 SEK).

I have added test case `givenCarVehicleTypeAndEachHourDates_whenCalculation_thenSuccess` to verify my change.

## 2.6. Logic for total toll fee calculation <a id="2.6"/>

I have found some issues with the total fee calculation:
- The dates should be sorted to make sure the list is always handled the same way.
- For the one-hour window, it is not enough to decide based on the whole minutes difference, even millis should count.
  -> For example, 15:00:00 and 16:00:01 would be one-hour period according to the original code but it exceeds it.
- For the one-hour window, we should not always take the first date to calculate the 60 minutes period
  -> We should use a changing reference date instead.

I have added test case `givenCarVehicleTypeAndHourPeriodDates_whenCalculation_thenSuccess` to verify my change.

## 2.7. Tests and Build <a id="2.7"/>

I have added tests meaning to go through as many scenarios as possible to make sure the algorithm works correctly.

For the commands below, please enter the `Java` folder of the repo.

You can run the test cases with the following command:
```shell
./gradlew clean test
```

You can run the test cases and build the project with the following command:
```shell
./gradlew clean build
```

You can build the project without running the test cases with the following command:
```shell
./gradlew clean build -x test
```

## 2.8. Parameterized tests for with different vehicle types, days and time periods <a id="2.8"/>

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

## 2.9. Spring Boot - Backend application <a id="2.9"/>

I have managed to turn project into a Spring Boot application.
This can be called by users with different input to do the calculation.

For this change, I needed to perform the following changes:
- Serialization config has been added so that the JSON response fields are shown with snake case instead of camel case.
- Error handling has been updated and ApiException has been created.
- Global exception handling has been introduced to support the different error handling corner cases.

There are some endpoints:
- `/info` -> for basic info about the application
- `/fee` -> for calculating the total fee
- `/actutator` -> for getting the available actuators list (only health is enabled)
- `/actuator/health` -> for getting to know if the application health is indicated as up or down

For the commands below, please enter the `Java` folder of the repo.

It can be started by the following command:
```shell
./gradlew bootRun
```

Once it is started, it is available on port 8080.

To call the `/info` endpoint:
```shell
curl -XGET "http://localhost:8080/info"
```

To call the `/fee` endpoint with example request parameters:
```shell
curl -XGET "http://localhost:8080/fee?vehicle_type=car&dates=2013-06-03T11:00:00"
```

To call the `/actuator/health` endpoint:
```shell
curl -XGET "http://localhost:8080/actuator/health"
```

You can also have a look at the Swagger page at `http://localhost:8080/swagger-ui/index.html`.
What is more, you can read about the different endpoint options and execute them in this page.

## 2.10. Logging <a id="2.10"/>

I have implemented a logging solution where all requests are logged. So are the errors.
There are also logs for progress during a request.

For the command below, please enter the `Java` folder of the repo.

By default, the application has DEBUG logging enabled for the `com.assignment.toll_calculator` package.
If you want to run it with INFO logging, use this command:
```shell
./gradlew bootRun --args='--LOG_LEVEL=INFO'
```

## 2.11. React - Frontend application <a id="2.11"/>

I have managed to set up a React app as well that interacts with the Spring Boot application.

This frontend app has some basic CSS configuration to help the user understanding.

It also contains test utilizing the React Testing Library.

I have managed to set up a frontend application as well that communicates with the API endpoints.

For the commands below, please enter the `Java/Frontend` folder of the repo.

To run the frontend application:
```shell
./gradlew clean start
```
It will be available on port 3000, and it expects the Spring Boot application running on port 8080.

To run the frontend application tests:
```shell
./gradlew clean test
```
For the tests, the API calls are mocked so the Spring Boot application is not needed to run!

# 3. Modification suggestions <a id="3"/>

Let me summarize the modifications I would like to suggest.

## 3.1. Holidays <a id="3.1"/>

I would suggest not to have the holiday as a strong requirement.

Reason: it would take a lot of time (basically forever) to collect all the holidays from all years. 
The holiday dates can differ between countries as well.

I think we would need to maintain a static date list of holidays in each year to be able to support this requirement.

I will keep the year 2013 holidays in the code so that I could write tests for this scenario as well.