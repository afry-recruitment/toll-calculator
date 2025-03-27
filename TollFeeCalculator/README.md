# Toll Fee Calculator - .NET 8 C# Console App

**A .NET 8 console application for calculating vehicle toll fees based on the current time.**

## Overview

This project implements a toll fee calculator as a .NET 8 C# console application. It adheres to the specified requirements for calculating toll fees based on time of day, vehicle type, and other factors. The application calculates the toll fee *based solely on the current system time (`DateTime.Now`)*. While the core logic is designed to eventually process a collection of vehicle passage times, the current implementation only uses the current time for calculations.

## Upstart
Install Nager.Date version 1.3.0  and other nuget pckages to start the project

## Features

*   **Automated Toll Calculation (Current Time Only):** Calculates toll fees based on the following rules, *using the current system time (`DateTime.Now`)*:
    *   **Time-Based Fees:** Fees vary between 8 and 18 SEK depending on the time of day. Rush hour periods incur the highest fee.
    *   **Daily Maximum:** Ensures the total toll fee for a single day does not exceed 60 SEK.
    *   **Hourly Charge Limit:** Charges a vehicle only once per hour, applying the highest fee within that hour.  Since it's only using `DateTime.Now`, this is effectively a single calculation.
    *   **Vehicle Type Exemptions:** Excludes specific vehicle types (e.g., motorcycles, emergency vehicles) from toll fees.
    *   **Weekend and Holiday Exemptions:** Exempts all vehicles from toll fees on weekends and designated holidays.
*   **Core Logic Designed for Multiple Times:** The core logic is designed to eventually process a collection of vehicle passage times, and unit tests confirm the underlying functionality.
*   **.NET 8 Implementation:** Built using the .NET 8 framework for performance and maintainability.

## Usage

The application calculates the toll fee based on the *current system time* when it is run.  To use the application:

1.  **Run the Application:** Execute the console application.
2.  **Receive Output:** The application will output the calculated toll fee for the *current time* in SEK.

## Input Format

*Not Applicable:* The application does not currently accept any input. It uses `DateTime.Now` to determine the passage time.

## Output Format

The application outputs the calculated toll fee for the *current time* in SEK. For example: