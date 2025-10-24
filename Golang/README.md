# Toll Fee Calculator (Go) 🚗

A small Go program that calculates toll fees for vehicles based on time of day, vehicle type, and date.

---

## Overview

The calculator follows these main rules:

- Fees range from **8 SEK to 18 SEK** depending on the time of day.  
- You only pay **once per 60-minute window** — the **highest** fee in that window applies.  
- The **daily maximum** fee is **60 SEK**.  
- **Weekends** and **public holidays** are toll-free. Public holidays are based on Sweden 2013. 
- Some vehicles are **always toll-free**:
  - Motorbike  
  - Tractor  
  - Emergency  
  - Diplomat  
  - Military  
  - Foreign

---

##  Run the Simulation

The `main.go` file runs a small random simulation.

```bash
go run ./main.go
```

## Example Output
```
🚗 Simulating trip for Car
📅 Date: 2013-02-04 (Monday)
──────────────────────────────
07:08 → 18 SEK (individual fee)
07:44 → 18 SEK (individual fee)
07:56 → 18 SEK (individual fee)
──────────────────────────────
💰 Total charged (after hourly rules): 18 SEK
```

## Tests 

Tests will check: 

- Toll-free vehicles

- Weekends and holidays

- Hourly window behavior

- Daily fee cap

- Fee breakpoints on exact minutes

- Invalid Timestamp

- Invalid Vehicle Type

- Multiple days (We restrict to one day at the time, after this the vehicle will be billed. Maximum fee for one day is 60 SEK)

Running the tests:
```bash
go test ./test
```

## Requirements

Go 1.25 or later

No external dependencies