# Toll Calculator

## Table of Contents
- [Introduction](#introduction)
- [Features](#features)
- [Setup](#setup)
- [Usage](#usage)
- [Unit Tests](#unit-tests)
- [Changes made](#unit-tests)
- [Future Work](#future-work)
- [Movie Name](#movie-name)

## Introduction
The **Toll Calculator** is a C# application that determines whether a date is toll-free and calculates toll fees for vehicles. This project incorporates public holiday rules, special Swedish toll-free rules (e.g., the entire month of July), and weekends.

## Features
- Toll-free date checking based on Swedish rule, currently hardcoded for 2024 but modifiable. 
- Includes weekends, holidays, and custom toll-free periods.
- Written in C# with support for unit testing.
- Uses a configurable list of toll-free dates to adapt to specific rules.
- Extendable for other countries or rules.

## Setup
Follow these steps to set up the project:

### Prerequisites
- .NET SDK (version 9.0 or higher)
- A compatible IDE (e.g. Visual Studio)

### Steps
Assuming you have cloned the repository and have the this project do the following:
Using *bash* or *Powershell*

1. Navigate to the project directory   
    * cd ...\toll-calculator\C#\toll.-calculator


2. Restore dependencies 
    * dotnet restore
3. Build Project
    * dotnet build
      
## Usage
Using bash or Powershell
1. * dotnet run
2. Enter a vehicle type and dates when prompted.

    *Example input:*

    *Vehicle: Car*  
    *Dates: 2024-03-29, 2024-03-30*  
    *Output: The application will calculate the toll fee and indicate whether the dates are toll-free.*

## Unit Tests
Unit tests are written using the *xUnit framework*. To execute tests run below command in *bash/Powershell*:  
* dotnet test  

For further details open the Test Explorer in Visual Studio to see the tests and their status. 

## Changes Made

- Created a project, inserted the provided files in the project and made the project compilable and executable. 
- Created a structure of Models, Interfaces and Tests
- Added Files:
    - Truck.cs
         - Added one more vehicle which is not a toll-free vehicle
    - TollCalculatorTests.cs
        - Contains unit tests. More tests can easily be added. 
    - Program.cs
        - Contains main method which executes on **dotnet run** 
    - README.md 
        - Specifically for this added project
    - .gitignore
        - necessary to ignore project related files which does not need to be comitted

### Changes to TollCalculator.cs
- Changed name of both methods named *GetTollFee* to *GetTotalTollFee* and *GetFeeForPassage*.
- *GetTotalTollFee*
    - Made  to be compatible for edge cases and adaptive to calibrate accross different dates correctly 
    - Changed *diffInMinutes* to actually calibrate the difference in minutes between the the current date and the intervalstart
- *GetFeeForPassage* 
    - Changed the else if statements to use a switch statement instead. 
    - Corrected for the first half hour between 8 and 14 to also charge 8 kr since this is clearly a mistake that was overlooked. 
- *IsTollFreeDate*
    - Used a list to store all holidays. The incoming dates will be checked against the list for a match. Also any year will be accounted for, not just 2013. 
- *IsTollFreeVechicle*
    - Instead of using Enum and Equals as well as the toString method to verify the vehicletypes we now store the vehicle types in a hashset and check if the vehicle matches any element in the set. 

## Future Work
The holidays are hardcoded and based on 2024. There are APIs such as Nager.Date which can be used to dynamically retrieve correct holidays independent on the year. However a license is required. A method has been added which makes this possible provided 
a license is available. 

## Movie Name
Hackers! Have not seen it but I definetely will. 