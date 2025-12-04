package main

import (
	"log"
	"sort"
	"time"
)

const maxDailyFee = 60

// FeeSlot represents a time range and its associated toll fee.
// Times are in 24-hour format (e.g., 6:30 to 6:59 = StartHour:6, StartMin:30, EndHour:6, EndMin:59)
type FeeSlot struct {
	StartHour int
	StartMin  int
	EndHour   int
	EndMin    int
	Fee       int
}

// DefaultFeeSchedule returns the standard Swedish toll fee schedule.
func DefaultFeeSchedule() []FeeSlot {
	return []FeeSlot{
		{6, 0, 6, 29, 8},     // Early morning: 8 SEK
		{6, 30, 6, 59, 13},   // Pre-rush: 13 SEK
		{7, 0, 7, 59, 18},    // Morning rush: 18 SEK
		{8, 0, 8, 29, 13},    // Post-rush: 13 SEK
		{8, 30, 14, 59, 8},   // Daytime: 8 SEK
		{15, 0, 15, 29, 13},  // Pre-rush: 13 SEK
		{15, 30, 16, 59, 18}, // Evening rush: 18 SEK
		{17, 0, 17, 59, 13},  // Post-rush: 13 SEK
		{18, 0, 18, 29, 8},   // Early evening: 8 SEK
		// Night (18:30-05:59) has no slot, defaults to 0 SEK
	}
}

// TollCalculator handles toll fee calculations
type TollCalculator struct {
	holidays    HolidayChecker
	feeSchedule []FeeSlot
}

// NewTollCalculator creates a new calculator with the default fee schedule
func NewTollCalculator(holidays HolidayChecker) *TollCalculator {
	return &TollCalculator{
		holidays:    holidays,
		feeSchedule: DefaultFeeSchedule(),
	}
}

// NewTollCalculatorWithSchedule creates a calculator with a fee schedule
func NewTollCalculatorWithSchedule(holidays HolidayChecker, schedule []FeeSlot) *TollCalculator {
	return &TollCalculator{
		holidays:    holidays,
		feeSchedule: schedule,
	}
}

// CalculateToll calculates the total toll fee for the given passes.
// If passes span multiple days, each day is calculated separately
// (with its own 60 SEK daily cap) and the results are summed.
//
// Rules applied:
// - Fees range from 8 to 18 SEK depending on time of day
// - Maximum daily fee is 60 SEK per day
// - Only the highest fee applies within each 60-minute window
// - Weekends and holidays are toll-free
// - Certain vehicle types are toll-free
func (tc *TollCalculator) CalculateToll(vehicle Vehicle, passes []time.Time) int {
	if len(passes) == 0 {
		return 0
	}

	// Nil vehicle is treated as toll-free (logging it is a warning)
	if vehicle == nil {
		log.Println("warning: CalculateToll called with nil vehicle")
		return 0
	}

	// Toll-free vehicles pay nothing
	if vehicle.IsTollFree() {
		return 0
	}

	// Group passes by calendar day
	passesByDay := tc.groupByDay(passes)

	// Calculate toll for each day and sum
	totalFee := 0
	for _, dayPasses := range passesByDay {
		totalFee += tc.calculateSingleDayToll(dayPasses)
	}

	return totalFee
}

// calculateSingleDayToll calculates toll for passes on a single calendar day.
// Assumes all passes are on the same day (caller must ensure this).
func (tc *TollCalculator) calculateSingleDayToll(passes []time.Time) int {
	if len(passes) == 0 {
		return 0
	}

	// Sort passes chronologically
	sortedPasses := make([]time.Time, len(passes))
	copy(sortedPasses, passes)
	sort.Slice(sortedPasses, func(i, j int) bool {
		return sortedPasses[i].Before(sortedPasses[j])
	})

	totalFee := 0
	windowStart := sortedPasses[0]
	windowMaxFee := tc.GetTollFee(windowStart)

	for i := 1; i < len(sortedPasses); i++ {
		currentPass := sortedPasses[i]
		currentFee := tc.GetTollFee(currentPass)

		// Check if current pass is within 60 minutes of window start
		minutesDiff := currentPass.Sub(windowStart).Minutes()

		if minutesDiff <= 60 {
			// Same window - track the highest fee
			if currentFee > windowMaxFee {
				windowMaxFee = currentFee
			}
		} else {
			// New window - add the previous window's max fee and start new window
			totalFee += windowMaxFee
			windowStart = currentPass
			windowMaxFee = currentFee
		}
	}

	// Add the last window's fee
	totalFee += windowMaxFee

	// Cap at maximum daily fee
	if totalFee > maxDailyFee {
		totalFee = maxDailyFee
	}

	return totalFee
}

// groupByDay groups passes by calendar day in Swedish timezone
func (tc *TollCalculator) groupByDay(passes []time.Time) map[string][]time.Time {
	groups := make(map[string][]time.Time)

	for _, pass := range passes {
		// Convert to Swedish time for correct day grouping
		swedishTime := InSwedishTime(pass)
		// Use YYYY-MM-DD format as day key
		key := swedishTime.Format("2006-01-02")
		groups[key] = append(groups[key], pass)
	}

	return groups
}

// GetTollFee returns the toll fee for a single pass at the given time.
// The time is converted to Swedish timezone for correct fee calculation.
func (tc *TollCalculator) GetTollFee(t time.Time) int {
	// Convert to Swedish time for correct hour/minute extraction
	swedishTime := InSwedishTime(t)

	if tc.isTollFreeDate(swedishTime) {
		return 0
	}

	hour := swedishTime.Hour()
	minute := swedishTime.Minute()

	// Convert to minutes since midnight for easy comparison
	timeInMinutes := hour*60 + minute

	// Check each fee slot
	for _, slot := range tc.feeSchedule {
		slotStart := slot.StartHour*60 + slot.StartMin
		slotEnd := slot.EndHour*60 + slot.EndMin

		if timeInMinutes >= slotStart && timeInMinutes <= slotEnd {
			return slot.Fee
		}
	}

	// No matching slot = free (night time)
	return 0
}

// isTollFreeDate checks if the given date is toll-free (weekend or holiday).
// Expects time to already be in Swedish timezone.
func (tc *TollCalculator) isTollFreeDate(t time.Time) bool {
	weekday := t.Weekday()

	// Weekends are free
	if weekday == time.Saturday || weekday == time.Sunday {
		return true
	}

	// Check holidays
	if tc.holidays != nil && tc.holidays.IsHoliday(t) {
		return true
	}

	return false
}
