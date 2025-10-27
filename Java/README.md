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

I chose to implement an enum called `Vehicle` containing the type value and a flag if the vehicle is toll-free or not.

I thought about two other similar ways:
1. Keeping the `Vehicle` interface and continue to implement all the other different vehicle type based on eg `Car`
   -> I would have added a `public boolean isTollFree();` function to the interface.
2. Keeping the `Vehicle` interface and implementing in only two classes `TollFreeVehicle` and `TollVehicle`
   -> I would have defined the interface as the option above.
   -> The model is only used to decide if a vehicle is toll-free or not.
   -> The type (if it is a car or motorbike) does not really matter unless we want to use these values as input.
   -> However, I concluded it might be important in a later stage to use the specific type.

My reasons for choosing the enum model over interface:
- The specific vehicle type list does not change, it is constant list that we do not edit dynamically.
- No logic or complex functions are needed inside the model, only the flag if the vehicle is toll-free or not.
- Bonus: it is so much simpler and nicer to define one file than one interface and all the classes.

## Modification suggestions

Let me summarize the modifications I would like to suggest.

### Holidays

I would suggest not to have the holiday as a strong requirement.

Reason: it would take a lot of time (basically forever) to collect all the holidays from all years. 
The holiday dates can differ between countries as well.

I think we would need to maintain a static date list of holidays in each year to be able to support this requirement.

I will keep the year 2013 holidays in the code so that I could write tests for this scenario as well.