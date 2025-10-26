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

## Modification suggestions

Let me summarize the modifications I would like to suggest.

### Holidays

I would suggest not to have the holiday as a strong requirement.

Reason: it would take a lot of time (basically forever) to collect all the holidays from all years. 
The holiday dates can differ between countries as well.

I think we would need to maintain a static date list of holidays in each year to be able to support this requirement.

I will keep the year 2013 holidays in the code so that I could write tests for this scenario as well.