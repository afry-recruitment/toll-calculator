# Toll Fee Calculator C#

This readme is intented to give instructions on how to setup and run the program.

## Prerequisites

To develop and run the program you will need the following

1. .NET 8 SDK from [https://dotnet.microsoft.com/en-us/download/dotnet/8.0](here)
2. IDE of your choice. If you are using VSCode please check the `.vscode/extensions.json` file for the required extensions

## Dev & Run

To run the app from the cli
1. Make sure that you are in the directgory `C#/TollFeeCalculator/`
2. Run the app using `dotnet run`
3. A demo will run showing the toll fee calculation


## Test

This project has automated tests written in xUnit

1. Make sure that you are in the directgory `C#/TollFeeCalculator.Test/`
2. Run the tests using `dotnet test`

## Release

To create a production ready release run the following command in `C#/` directory: `dotnet clean && dotnet restore && dotnet publish`.