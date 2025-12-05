# Toll Fee Calculator - Go Implementation

A toll fee calculator for vehicles, implementing Swedish congestion tax rules.

## Requirements

- Go 1.25.5

## Quick Start

```bash
go run .
```

### Using Makefile

The project includes a Makefile with convenient commands:

```bash
make test      # Run all tests
make build     # Build the binary
make run-demo  # Build and run the demo application
make clean     # Remove build artifacts
```

## Project Structure

```text
.
├── main.go            # Demo application with example usage
├── calculator.go      # Core toll calculation logic
├── calculator_test.go # Tests for calculator
├── vehicle.go         # Vehicle interface and all vehicle types
├── holidays.go        # Extensible holiday checking system
├── holidays_test.go   # Tests for holidays
├── timezone.go        # Swedish timezone handling
├── timezone_test.go   # Tests for timezone
├── go.mod             # Go module definition
└── Makefile           # Build and test commands
```

## How It Works

### Fee Schedule

| Time Period | Fee (SEK) | Description |
|-------------|-----------|-------------|
| 06:00-06:29 | 8 | Early morning |
| 06:30-06:59 | 13 | Pre-rush |
| 07:00-07:59 | 18 | Morning rush hour |
| 08:00-08:29 | 13 | Post-rush |
| 08:30-14:59 | 8 | Daytime |
| 15:00-15:29 | 13 | Pre-rush |
| 15:30-16:59 | 18 | Evening rush hour |
| 17:00-17:59 | 13 | Post-rush |
| 18:00-18:29 | 8 | Early evening |
| 18:30-05:59 | 0 | Night (free) |

### Rules

1. **Maximum daily fee**: 60 SEK per vehicle per day (each day capped independently)
2. **60-minute rule**: Only the highest fee applies within any 60-minute window
3. **Multi-day support**: Passes spanning multiple days are grouped and calculated per day
4. **Toll-free dates**: Weekends, public holidays, and the entire month of July
5. **Toll-free vehicles**: Motorbikes, tractors, emergency vehicles, diplomat vehicles, foreign-registered vehicles, and military vehicles

### API

```go
// Create calculator with default Swedish fee schedule
calculator := NewTollCalculator(holidays)

// Calculate toll for any number of passes (handles single or multiple days)
fee := calculator.CalculateToll(vehicle, passes)

// Get fee for a single pass at a specific time
fee := calculator.GetTollFee(time)
```

### Timezone Handling

All times are interpreted in Swedish timezone (Europe/Stockholm) to ensure correct fee calculation regardless of where the server is running:

```go
// Create times in Swedish timezone explicitly
pass := NewSwedishTime(2025, 1, 8, 7, 30, 0) // 07:30 Swedish time

// Convert existing time to Swedish timezone
swedishTime := InSwedishTime(someUTCTime)

// Access the timezone directly
loc := SwedishTimezone
```

If the system timezone database is unavailable, a fallback to UTC+1 (CET) is used with a warning logged.

## Bugs Fixed from C#/Java Implementation

### 1. Critical: Time Difference Calculation (C# only)

**Original bug:**

```csharp
long diffInMillies = date.Millisecond - intervalStart.Millisecond;
```

This only compared the millisecond *component* (0-999ms) of each timestamp, not the actual time difference between them. For example, comparing 07:00:00.500 and 07:30:00.200 would give -300ms instead of 30 minutes!

**Fix:** We use Go's `time.Duration` with proper subtraction:

```go
minutesDiff := currentPass.Sub(windowStart).Minutes()
```

### 2. Fee Schedule Gaps (Both C# and Java)

**Original bug:**

```java
else if (hour >= 8 && hour <= 14 && minute >= 30 && minute <= 59) return 8;
```

This condition only applied the 8 SEK fee when `minute >= 30`, leaving times like 09:00-09:29, 10:00-10:29, etc. to fall through to return 0.

**Fix:** We properly handle the entire 08:30-14:59 range using a structured fee schedule:

```go
// In DefaultFeeSchedule():
{8, 30, 14, 59, 8},   // Daytime: 8 SEK

// The GetTollFee method checks each slot:
for _, slot := range tc.feeSchedule {
    slotStart := slot.StartHour*60 + slot.StartMin
    slotEnd := slot.EndHour*60 + slot.EndMin
    
    if timeInMinutes >= slotStart && timeInMinutes <= slotEnd {
        return slot.Fee
    }
}
```

This ensures the entire 08:30-14:59 range is properly covered with the 8 SEK fee.

### 3. Operator Precedence Issue (Both C# and Java)

**Original bug:**

```java
else if (hour == 15 && minute >= 0 || hour == 16 && minute <= 59) return 18;
```

Due to operator precedence (`&&` before `||`), this was ambiguous and conflicted with earlier conditions.

**Fix:** Clear, non-overlapping time ranges using the structured fee schedule:

```go
// In DefaultFeeSchedule():
{15, 30, 16, 59, 18}, // Evening rush: 18 SEK

// The GetTollFee method checks each slot:
for _, slot := range tc.feeSchedule {
    slotStart := slot.StartHour*60 + slot.StartMin
    slotEnd := slot.EndHour*60 + slot.EndMin
    
    if timeInMinutes >= slotStart && timeInMinutes <= slotEnd {
        return slot.Fee
    }
}
```

This ensures the 15:30-16:59 range is properly covered with the 18 SEK fee, avoiding any operator precedence ambiguity.

### 4. Interval Tracking Algorithm (Both C# and Java)

**Original bug:** The `intervalStart` variable was never updated when a new 60-minute window should begin. The algorithm also incorrectly subtracted fees, leading to potential negative or incorrect totals.

**Fix:** Complete rewrite with proper sliding window logic:

- Track the start of each 60-minute window
- Track the maximum fee within each window
- When a pass exceeds 60 minutes from window start, finalize the window and start a new one
- Passes are sorted chronologically before processing

### 5. Hardcoded Year (Both C# and Java)

**Original bug:** Holidays were only defined for 2013:

```java
if (year == 2013) {
    // ...holidays...
}
```

**Fix:** Extensible `HolidayChecker` interface with support for multiple years:

```go
type HolidayChecker interface {
    IsHoliday(t time.Time) bool
}
```

### 6. Missing Vehicle Types (Both C# and Java)

**Original:** Only `Car` and `Motorbike` were implemented.

**Fix:** All toll-free vehicle types are now implemented:

- Motorbike
- Tractor
- Emergency
- Diplomat
- Foreign
- Military

## Design Improvements

1. **Interface-based design**: The `Vehicle` interface includes an `IsTollFree()` method, eliminating string-based type checking
2. **Data-driven fee schedule**: Fee slots are defined as data structures, not hardcoded switch statements
3. **Multi-day support**: Passes spanning multiple days are automatically grouped and each day is calculated with its own 60 SEK cap
4. **Explicit timezone handling**: All times are converted to Swedish timezone (Europe/Stockholm) for consistent calculations regardless of server location
5. **Graceful error handling**: Nil vehicles are handled without panics (logs warning, returns 0)
6. **Separation of concerns**: Holidays, vehicles, timezone, and calculation logic are in separate files
7. **Extensibility**: Easy to add new vehicle types, holiday definitions, or fee schedules
8. **Immutability**: Passes are copied before sorting to avoid mutating the input
9. **Clear documentation**: Code comments explain the business rules
10. **Standard library only**: No external dependencies
