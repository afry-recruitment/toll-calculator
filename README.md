# Toll Calculator

A C# console application for calculating vehicle toll fees based on
Swedish congestion tax rules.

## Features

- Calculate toll fees per vehicle based on time of day
- Handles Swedish public holidays and weekends (toll free)
- Sliding 60 minute window — only most expensive fee per hour charged
- Daily maximum limit of 60 SEK per vehicle
- Fee free vehicles (Bus)
- Detailed fee breakdown per toll entry

## How to Run

1. Clone the repository
2. Open `TollCalculator.sln` in Visual Studio
3. Set `TollCalculator` as startup project
4. Press F5

## Project Structure

- `TollCalculator/` - Main application
  - `Models/` - Vehicle and toll entry classes
  - `Services/` - Calculation and holiday logic

- `TollCalculator.Tests/` - Unit tests

## Design Decisions

- `Vehicle` is abstract — a plain vehicle is never instantiated

- Toll free rules live in `TollCalculatorService`, not in vehicle
  classes — toll rules are a government concern, not a vehicle concern

- `SwedishHolidayService` is a pure calendar utility with no
  knowledge of toll rules — Single Responsibility Principle
- Swedish holidays calculated without third party dependencies
- Fee schedule defined as a static table in `TollCalculatorService`
  — easy to maintain and extend
- `VehicleRegistry` acts as a vehicle lookup — mirrors real world
  where toll sensors only register a license plate
- Unknown vehicles are charged full toll fee, and a warning is written to Console.Error. The caller can separately invoke GetUnknownVehicles() to retrieve a list of unregistered vehicles for further investigation.

- Holiday calculation methods (GetEaster, GetMidsummerEve, GetAllSaints) are 
  marked internal to hide implementation details from external consumers, 
  while remaining accessible to the test project via InternalsVisibleTo.

## Fee Schedule

| Time | Fee |
|---|---|
| 06:00–06:29 | 8 SEK |
| 06:30–06:59 | 13 SEK |
| 07:00–07:59 | 18 SEK |
| 08:00–08:29 | 13 SEK |
| 08:30–14:59 | 8 SEK |
| 15:00–15:29 | 13 SEK |
| 15:30–16:59 | 18 SEK |
| 17:00–17:59 | 13 SEK |
| 18:00–18:29 | 8 SEK |
| Other | 0 SEK |

## Known Limitations

- Individual entry fees in VehicleFeeDetails reflect the raw time-based fee 
  and do not account for the sliding window absorption. TotalFee is always correct.

- No persistent storage — data lives in memory only

## Test Coverage

- Grouping entries by registration number
- Sorting entries by timestamp
- Empty list input
- Toll free vehicles
- Weekend exemption
- Time based fee schedule
- Daily maximum limit of 60 SEK
- Sliding 60 minute window hourly max fee
- Swedish public holidays
- Fee stored in VehicleFeeDetails
- Fee schedule boundary transitions
- Entries spanning multiple days
- Unregistered vehicle handling
- Null input guard
- Duplicate registration number in registry
