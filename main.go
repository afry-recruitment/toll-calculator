package main

import (
	"fmt"
	"time"
)

func main() {
	// Create toll calculator with Swedish holidays
	holidays := NewSwedishHolidays()
	calculator := NewTollCalculator(holidays)

	// Demo: Calculate toll for different vehicles and times
	fmt.Println("=== Toll Fee Calculator Demo ===")
	fmt.Println()

	// Test with a regular car on a weekday
	car := Car{}
	fmt.Printf("Vehicle: %s (toll-free: %v)\n", car.GetType(), car.IsTollFree())
	fmt.Println()

	// Create some sample passes on a Monday (2025-01-08) in Swedish timezone
	passes := []time.Time{
		NewSwedishTime(2025, 1, 8, 6, 15, 0),  // 06:15 -> 8 SEK
		NewSwedishTime(2025, 1, 8, 7, 30, 0),  // 07:30 -> 18 SEK (rush hour)
		NewSwedishTime(2025, 1, 8, 8, 45, 0),  // 08:45 -> 8 SEK
		NewSwedishTime(2025, 1, 8, 15, 45, 0), // 15:45 -> 18 SEK (rush hour)
		NewSwedishTime(2025, 1, 8, 17, 30, 0), // 17:30 -> 13 SEK
	}

	fmt.Println("Passes on Monday 2025-01-08:")
	for _, pass := range passes {
		fee := calculator.GetTollFee(pass)
		fmt.Printf("  %s -> %d SEK\n", pass.Format("15:04"), fee)
	}
	fmt.Println()

	totalFee := calculator.CalculateToll(car, passes)
	fmt.Printf("Total daily fee: %d SEK\n", totalFee)
	fmt.Println()

	// Demonstrate the 60-minute rule
	fmt.Println("--- 60-minute rule demonstration ---")
	closePassses := []time.Time{
		NewSwedishTime(2025, 1, 8, 7, 0, 0),  // 07:00 -> 18 SEK
		NewSwedishTime(2025, 1, 8, 7, 30, 0), // 07:30 -> 18 SEK (within 60 min, same fee)
		NewSwedishTime(2025, 1, 8, 7, 55, 0), // 07:55 -> 18 SEK (still within 60 min)
	}

	fmt.Println("Three passes within 60 minutes (all rush hour):")
	for _, pass := range closePassses {
		fee := calculator.GetTollFee(pass)
		fmt.Printf("  %s -> %d SEK\n", pass.Format("15:04"), fee)
	}
	total := calculator.CalculateToll(car, closePassses)
	fmt.Printf("Total (only highest fee in window): %d SEK\n", total)
	fmt.Println()

	// Demonstrate toll-free vehicle
	fmt.Println("--- Toll-free vehicle demonstration ---")
	motorbike := Motorbike{}
	fmt.Printf("Vehicle: %s (toll-free: %v)\n", motorbike.GetType(), motorbike.IsTollFree())
	motorbikeFee := calculator.CalculateToll(motorbike, passes)
	fmt.Printf("Same passes, toll-free vehicle: %d SEK\n", motorbikeFee)
	fmt.Println()

	// Demonstrate weekend (toll-free)
	fmt.Println("--- Weekend demonstration ---")
	saturday := NewSwedishTime(2025, 1, 4, 8, 0, 0) // January 4, 2025 is a Saturday
	fmt.Printf("Saturday %s: %d SEK\n", saturday.Format("2006-01-02 15:04"), calculator.GetTollFee(saturday))
	fmt.Println()

	// Demonstrate holiday (toll-free)
	fmt.Println("--- Holiday demonstration ---")
	christmasEve := NewSwedishTime(2025, 12, 24, 8, 0, 0)
	fmt.Printf("Christmas Eve %s: %d SEK\n", christmasEve.Format("2006-01-02 15:04"), calculator.GetTollFee(christmasEve))
	fmt.Println()

	// Demonstrate max daily fee cap
	fmt.Println("--- Maximum daily fee demonstration ---")
	manyPasses := []time.Time{
		NewSwedishTime(2025, 1, 8, 6, 0, 0),   // 06:00 -> 8 SEK
		NewSwedishTime(2025, 1, 8, 7, 5, 0),   // 07:05 -> 18 SEK
		NewSwedishTime(2025, 1, 8, 8, 10, 0),  // 08:10 -> 13 SEK
		NewSwedishTime(2025, 1, 8, 9, 15, 0),  // 09:15 -> 8 SEK
		NewSwedishTime(2025, 1, 8, 10, 20, 0), // 10:20 -> 8 SEK
		NewSwedishTime(2025, 1, 8, 11, 25, 0), // 11:25 -> 8 SEK
		NewSwedishTime(2025, 1, 8, 12, 30, 0), // 12:30 -> 8 SEK
		NewSwedishTime(2025, 1, 8, 15, 35, 0), // 15:35 -> 18 SEK
		NewSwedishTime(2025, 1, 8, 16, 40, 0), // 16:40 -> 18 SEK
		NewSwedishTime(2025, 1, 8, 17, 45, 0), // 17:45 -> 13 SEK
	}

	fmt.Println("Many passes throughout the day:")
	sumFees := 0
	for _, pass := range manyPasses {
		fee := calculator.GetTollFee(pass)
		sumFees += fee
		fmt.Printf("  %s -> %d SEK\n", pass.Format("15:04"), fee)
	}
	fmt.Printf("Sum of individual fees: %d SEK\n", sumFees)

	cappedFee := calculator.CalculateToll(car, manyPasses)
	fmt.Printf("Actual total (capped at 60 SEK max): %d SEK\n", cappedFee)
	fmt.Println()

	// Demonstrate multi-day toll calculation
	fmt.Println("--- Multi-day toll demonstration ---")
	multiDayPasses := []time.Time{
		// Day 1 (Monday): Rush hour passes = 18 + 18 = 36 SEK
		NewSwedishTime(2025, 1, 8, 7, 30, 0),
		NewSwedishTime(2025, 1, 8, 16, 0, 0),
		// Day 2 (Tuesday): Rush hour passes = 18 + 18 = 36 SEK
		NewSwedishTime(2025, 1, 9, 7, 0, 0),
		NewSwedishTime(2025, 1, 9, 15, 45, 0),
		// Day 3 (Wednesday): One rush hour pass = 18 SEK
		NewSwedishTime(2025, 1, 10, 7, 15, 0),
	}

	fmt.Println("Passes across 3 days:")
	for _, pass := range multiDayPasses {
		fee := calculator.GetTollFee(pass)
		fmt.Printf("  %s -> %d SEK\n", pass.Format("2006-01-02 15:04"), fee)
	}

	multiDayTotal := calculator.CalculateToll(car, multiDayPasses)
	fmt.Printf("Total across all days (each day capped at 60 SEK): %d SEK\n", multiDayTotal)
	fmt.Println()

	// Demonstrate multi-day spanning across month boundary
	fmt.Println("--- Month boundary demonstration ---")
	monthBoundaryPasses := []time.Time{
		// January 30 (Thursday): Rush hour = 18 SEK
		NewSwedishTime(2025, 1, 30, 7, 30, 0),
		// January 31 (Friday): Rush hour = 18 SEK
		NewSwedishTime(2025, 1, 31, 7, 30, 0),
		// February 3 (Monday): Rush hour = 18 SEK
		NewSwedishTime(2025, 2, 3, 7, 30, 0),
		// February 4 (Tuesday): Rush hour = 18 SEK
		NewSwedishTime(2025, 2, 4, 7, 30, 0),
	}

	fmt.Println("Passes spanning January to February:")
	for _, pass := range monthBoundaryPasses {
		fee := calculator.GetTollFee(pass)
		fmt.Printf("  %s (%s) -> %d SEK\n", pass.Format("2006-01-02 15:04"), pass.Weekday(), fee)
	}

	monthBoundaryTotal := calculator.CalculateToll(car, monthBoundaryPasses)
	fmt.Printf("Total across month boundary: %d SEK\n", monthBoundaryTotal)
	fmt.Println()

	// Demonstrate multi-day spanning across year boundary
	fmt.Println("--- Year boundary demonstration ---")
	yearBoundaryPasses := []time.Time{
		// December 29, 2025 (Monday): Rush hour = 18 SEK
		NewSwedishTime(2025, 12, 29, 7, 30, 0),
		// December 30, 2025 (Tuesday): Rush hour = 18 SEK
		NewSwedishTime(2025, 12, 30, 7, 30, 0),
		// December 31, 2025 (Wednesday): New Year's Eve = 0 SEK (holiday)
		NewSwedishTime(2025, 12, 31, 7, 30, 0),
		// January 1, 2026 (Thursday): New Year's Day = 0 SEK (holiday)
		NewSwedishTime(2026, 1, 1, 7, 30, 0),
		// January 2, 2026 (Friday): Rush hour = 18 SEK
		NewSwedishTime(2026, 1, 2, 7, 30, 0),
	}

	fmt.Println("Passes spanning 2025 to 2026 (note: Dec 31 & Jan 1 are holidays):")
	for _, pass := range yearBoundaryPasses {
		fee := calculator.GetTollFee(pass)
		holidayNote := ""
		if fee == 0 && pass.Weekday() != time.Saturday && pass.Weekday() != time.Sunday {
			holidayNote = " (holiday)"
		}
		fmt.Printf("  %s (%s) -> %d SEK%s\n", pass.Format("2006-01-02 15:04"), pass.Weekday(), fee, holidayNote)
	}

	yearBoundaryTotal := calculator.CalculateToll(car, yearBoundaryPasses)
	fmt.Printf("Total across year boundary: %d SEK (only non-holiday weekdays charged)\n", yearBoundaryTotal)
	fmt.Println()

	// Demonstrate multi-day with daily cap applied per day
	fmt.Println("--- Multi-day with daily caps demonstration ---")
	heavyUsagePasses := []time.Time{
		// Day 1 (Monday Jan 13): Many passes, exceeds cap
		NewSwedishTime(2025, 1, 13, 6, 15, 0),  // 8 SEK
		NewSwedishTime(2025, 1, 13, 7, 30, 0),  // 18 SEK
		NewSwedishTime(2025, 1, 13, 9, 0, 0),   // 8 SEK
		NewSwedishTime(2025, 1, 13, 12, 0, 0),  // 8 SEK
		NewSwedishTime(2025, 1, 13, 15, 30, 0), // 18 SEK
		NewSwedishTime(2025, 1, 13, 17, 0, 0),  // 13 SEK = 73 SEK uncapped
		// Day 2 (Tuesday Jan 14): Many passes, exceeds cap
		NewSwedishTime(2025, 1, 14, 6, 15, 0),  // 8 SEK
		NewSwedishTime(2025, 1, 14, 7, 30, 0),  // 18 SEK
		NewSwedishTime(2025, 1, 14, 9, 0, 0),   // 8 SEK
		NewSwedishTime(2025, 1, 14, 12, 0, 0),  // 8 SEK
		NewSwedishTime(2025, 1, 14, 15, 30, 0), // 18 SEK
		NewSwedishTime(2025, 1, 14, 17, 0, 0),  // 13 SEK = 73 SEK uncapped
	}

	fmt.Println("Heavy usage across 2 days (each would be 73 SEK uncapped):")
	day1Sum, day2Sum := 0, 0
	for _, pass := range heavyUsagePasses {
		fee := calculator.GetTollFee(pass)
		if pass.Day() == 13 {
			day1Sum += fee
		} else {
			day2Sum += fee
		}
		fmt.Printf("  %s -> %d SEK\n", pass.Format("2006-01-02 15:04"), fee)
	}
	fmt.Printf("Day 1 uncapped sum: %d SEK (capped to 60 SEK)\n", day1Sum)
	fmt.Printf("Day 2 uncapped sum: %d SEK (capped to 60 SEK)\n", day2Sum)

	heavyUsageTotal := calculator.CalculateToll(car, heavyUsagePasses)
	fmt.Printf("Total with daily caps: %d SEK (60 + 60)\n", heavyUsageTotal)
}
