# Toll Fee Calculator

A configurable toll fee calculator for Swedish city toll systems, built with .NET 10 and Minimal APIs.

Great movie btw! -> https://www.imdb.com/title/tt0113243/

## Features

- ✅ Time-based fee calculation (rush hour pricing)
- ✅ Rolling 60-minute charging windows
- ✅ Daily fee cap (60 SEK)
- ✅ Toll-free vehicle types
- ✅ Weekend and holiday support
- ✅ Multi-day calculations

## Quick Start

```bash
# Clone and run
git clone <repo>
cd TollFeeCalculator
dotnet run

# Access API documentation
https://localhost:5001/api/docs
```

## API Usage

**Calculate Toll Fee**
```bash
POST /api/toll/calculate
Content-Type: application/json

{
  "passages": [
    "2026-01-02T07:00:00",
    "2026-01-02T15:30:00"
  ],
  "vehicleType": "Car"
}

Response:
{
  "totalFee": 36,
  "currency": "SEK"
}
```

**Toll-Free Vehicles:** Motorcycle, Bus, Emergency, Diplomat, Foreign, Military

## Configuration

Edit `appsettings.json` to customize:
- Fee schedule (time ranges and amounts)
- Daily cap
- Charge interval
- Toll-free vehicle types
- Currency

## How It Works

1. **Rolling 60-Minute Windows:** Only charged once per hour. Within each window, the highest fee applies.
2. **Daily Cap:** Maximum 60 SEK per day, regardless of passages.
3. **Multi-Day Support:** Automatically groups by day and applies cap per day.
4. **Toll-Free Checks:** Weekends, holidays (2026), and specific vehicle types return 0 SEK.

**Example:**
- 07:00 → 18 SEK (rush hour)
- 07:30 → Not charged (same 60-min window)
- 08:35 → 8 SEK (new window, 95 min later)
- **Total: 26 SEK**


## Running Tests

```bash
dotnet test
```

## Design Decisions

See [ASSUMPTIONS.md](ASSUMPTIONS.md) for detailed reasoning on:
- Why rolling windows vs clock hours
- Separation of concerns (calculator, vehicle service, holiday provider)
- Configuration approach
- Edge case handling

## Legacy Code

The `legacy/` folder contains the original implementation with fixes documented in [BUGFIX_SUMMARY.md](BUGFIX_SUMMARY.md).

## Technology

- .NET 10
- Minimal APIs
- Scalar API documentation
- XUnit for testing

