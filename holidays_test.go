package main

import (
	"testing"
	"time"
)

func TestNewSwedishHolidays(t *testing.T) {
	// Test that NewSwedishHolidays creates a valid instance
	sh := NewSwedishHolidays()
	if sh == nil {
		t.Fatal("NewSwedishHolidays() should not return nil")
	}

	// Test that holidays map is initialized
	if sh.holidays == nil {
		t.Fatal("holidays map should not be nil")
	}

	// Test that julyFree is set correctly
	if !sh.julyFree {
		t.Error("julyFree should be true")
	}

	// Test that some expected holidays are present
	testCases := []struct {
		month time.Month
		day   int
	}{
		{time.January, 1},   // New Year's Day
		{time.December, 25}, // Christmas Day
		{time.May, 1},       // Labour Day
	}

	for _, tc := range testCases {
		if !sh.IsHoliday(NewSwedishTime(2025, tc.month, tc.day, 12, 0, 0)) {
			t.Errorf("Expected %s %d to be a holiday", tc.month, tc.day)
		}
	}
}

func TestIsHoliday(t *testing.T) {
	sh := NewSwedishHolidays()

	tests := []struct {
		name     string
		date     time.Time
		expected bool
	}{
		{
			name:     "New Year's Day",
			date:     NewSwedishTime(2025, time.January, 1, 12, 0, 0),
			expected: true,
		},
		{
			name:     "Maundy Thursday",
			date:     NewSwedishTime(2025, time.March, 28, 12, 0, 0),
			expected: true,
		},
		{
			name:     "Good Friday",
			date:     NewSwedishTime(2025, time.March, 29, 12, 0, 0),
			expected: true,
		},
		{
			name:     "Easter Monday",
			date:     NewSwedishTime(2025, time.April, 1, 12, 0, 0),
			expected: true,
		},
		{
			name:     "Walpurgis Night",
			date:     NewSwedishTime(2025, time.April, 30, 12, 0, 0),
			expected: true,
		},
		{
			name:     "Labour Day",
			date:     NewSwedishTime(2025, time.May, 1, 12, 0, 0),
			expected: true,
		},
		{
			name:     "Day before Ascension",
			date:     NewSwedishTime(2025, time.May, 8, 12, 0, 0),
			expected: true,
		},
		{
			name:     "Ascension Day",
			date:     NewSwedishTime(2025, time.May, 9, 12, 0, 0),
			expected: true,
		},
		{
			name:     "Day before National Day",
			date:     NewSwedishTime(2025, time.June, 5, 12, 0, 0),
			expected: true,
		},
		{
			name:     "National Day",
			date:     NewSwedishTime(2025, time.June, 6, 12, 0, 0),
			expected: true,
		},
		{
			name:     "Midsummer Eve",
			date:     NewSwedishTime(2025, time.June, 21, 12, 0, 0),
			expected: true,
		},
		{
			name:     "All Saints' Day",
			date:     NewSwedishTime(2025, time.November, 1, 12, 0, 0),
			expected: true,
		},
		{
			name:     "Christmas Eve",
			date:     NewSwedishTime(2025, time.December, 24, 12, 0, 0),
			expected: true,
		},
		{
			name:     "Christmas Day",
			date:     NewSwedishTime(2025, time.December, 25, 12, 0, 0),
			expected: true,
		},
		{
			name:     "Boxing Day",
			date:     NewSwedishTime(2025, time.December, 26, 12, 0, 0),
			expected: true,
		},
		{
			name:     "New Year's Eve",
			date:     NewSwedishTime(2025, time.December, 31, 12, 0, 0),
			expected: true,
		},
		{
			name:     "Regular weekday",
			date:     NewSwedishTime(2025, time.January, 15, 12, 0, 0),
			expected: false,
		},
		{
			name:     "Regular weekend day",
			date:     NewSwedishTime(2025, time.January, 18, 12, 0, 0),
			expected: false,
		},
		{
			name:     "Day before New Year's Day",
			date:     NewSwedishTime(2024, time.December, 31, 12, 0, 0),
			expected: true, // This is New Year's Eve
		},
		{
			name:     "Day after New Year's Day",
			date:     NewSwedishTime(2025, time.January, 2, 12, 0, 0),
			expected: false,
		},
	}

	for _, tt := range tests {
		t.Run(tt.name, func(t *testing.T) {
			result := sh.IsHoliday(tt.date)
			if result != tt.expected {
				t.Errorf("IsHoliday(%v) = %v, want %v", tt.date, result, tt.expected)
			}
		})
	}
}

func TestIsHolidayJuly(t *testing.T) {
	// Test that July is always toll-free (vacation month)
	sh := NewSwedishHolidays()

	tests := []struct {
		name string
		date time.Time
	}{
		{
			name: "July 1st",
			date: NewSwedishTime(2025, time.July, 1, 12, 0, 0),
		},
		{
			name: "July 15th (mid-month)",
			date: NewSwedishTime(2025, time.July, 15, 12, 0, 0),
		},
		{
			name: "July 31st (end of month)",
			date: NewSwedishTime(2025, time.July, 31, 12, 0, 0),
		},
		{
			name: "July 1st at midnight",
			date: NewSwedishTime(2025, time.July, 1, 0, 0, 0),
		},
		{
			name: "July 31st at end of day",
			date: NewSwedishTime(2025, time.July, 31, 23, 59, 59),
		},
		{
			name: "July 1st different year",
			date: NewSwedishTime(2024, time.July, 1, 12, 0, 0),
		},
		{
			name: "July 1st future year",
			date: NewSwedishTime(2030, time.July, 1, 12, 0, 0),
		},
	}

	for _, tt := range tests {
		t.Run(tt.name, func(t *testing.T) {
			result := sh.IsHoliday(tt.date)
			if !result {
				t.Errorf("IsHoliday(%v) = false, want true (July should always be a holiday)", tt.date)
			}
		})
	}
}

func TestIsHolidayEdgeCases(t *testing.T) {
	sh := NewSwedishHolidays()

	tests := []struct {
		name     string
		date     time.Time
		expected bool
	}{
		{
			name:     "Leap year February 29",
			date:     NewSwedishTime(2024, time.February, 29, 12, 0, 0),
			expected: false,
		},
		{
			name:     "Non-leap year February 28",
			date:     NewSwedishTime(2025, time.February, 28, 12, 0, 0),
			expected: false,
		},
		{
			name:     "Year boundary - December 31",
			date:     NewSwedishTime(2025, time.December, 31, 12, 0, 0),
			expected: true, // New Year's Eve
		},
		{
			name:     "Year boundary - January 1",
			date:     NewSwedishTime(2026, time.January, 1, 12, 0, 0),
			expected: true, // New Year's Day
		},
		{
			name:     "Different time of day - morning",
			date:     NewSwedishTime(2025, time.December, 25, 6, 0, 0),
			expected: true, // Christmas Day
		},
		{
			name:     "Different time of day - evening",
			date:     NewSwedishTime(2025, time.December, 25, 22, 0, 0),
			expected: true, // Christmas Day
		},
		{
			name:     "Different time of day - midnight",
			date:     NewSwedishTime(2025, time.December, 25, 0, 0, 0),
			expected: true, // Christmas Day
		},
		{
			name:     "Different timezone conversion",
			date:     InSwedishTime(time.Date(2025, time.December, 25, 12, 0, 0, 0, time.UTC)),
			expected: true, // Christmas Day
		},
	}

	for _, tt := range tests {
		t.Run(tt.name, func(t *testing.T) {
			result := sh.IsHoliday(tt.date)
			if result != tt.expected {
				t.Errorf("IsHoliday(%v) = %v, want %v", tt.date, result, tt.expected)
			}
		})
	}
}

func TestHolidayCheckerInterface(t *testing.T) {
	// Test that SwedishHolidays implements the HolidayChecker interface
	var checker HolidayChecker = NewSwedishHolidays()

	// Test that the interface method works
	christmas := NewSwedishTime(2025, time.December, 25, 12, 0, 0)
	if !checker.IsHoliday(christmas) {
		t.Error("HolidayChecker.IsHoliday should return true for Christmas")
	}

	regularDay := NewSwedishTime(2025, time.January, 15, 12, 0, 0)
	if checker.IsHoliday(regularDay) {
		t.Error("HolidayChecker.IsHoliday should return false for regular day")
	}
}

func TestHolidayConsistency(t *testing.T) {
	// Test that holidays are consistent across different years
	sh := NewSwedishHolidays()

	holidays := []struct {
		month time.Month
		day   int
	}{
		{time.January, 1},
		{time.March, 28},
		{time.March, 29},
		{time.April, 1},
		{time.April, 30},
		{time.May, 1},
		{time.May, 8},
		{time.May, 9},
		{time.June, 5},
		{time.June, 6},
		{time.June, 21},
		{time.November, 1},
		{time.December, 24},
		{time.December, 25},
		{time.December, 26},
		{time.December, 31},
	}

	years := []int{2020, 2021, 2022, 2023, 2024, 2025, 2026, 2027, 2028, 2029, 2030}

	for _, holiday := range holidays {
		for _, year := range years {
			date := NewSwedishTime(year, holiday.month, holiday.day, 12, 0, 0)
			if !sh.IsHoliday(date) {
				t.Errorf("Expected %s %d, %d to be a holiday", holiday.month, holiday.day, year)
			}
		}
	}
}

func TestHolidayNonHolidayDates(t *testing.T) {
	// Test that various non-holiday dates return false
	sh := NewSwedishHolidays()

	nonHolidays := []struct {
		name string
		date time.Time
	}{
		{
			name: "January 2",
			date: NewSwedishTime(2025, time.January, 2, 12, 0, 0),
		},
		{
			name: "January 15",
			date: NewSwedishTime(2025, time.January, 15, 12, 0, 0),
		},
		{
			name: "February 14",
			date: NewSwedishTime(2025, time.February, 14, 12, 0, 0),
		},
		{
			name: "March 15",
			date: NewSwedishTime(2025, time.March, 15, 12, 0, 0),
		},
		{
			name: "April 15",
			date: NewSwedishTime(2025, time.April, 15, 12, 0, 0),
		},
		{
			name: "May 15",
			date: NewSwedishTime(2025, time.May, 15, 12, 0, 0),
		},
		{
			name: "June 15",
			date: NewSwedishTime(2025, time.June, 15, 12, 0, 0),
		},
		{
			name: "August 15",
			date: NewSwedishTime(2025, time.August, 15, 12, 0, 0),
		},
		{
			name: "September 15",
			date: NewSwedishTime(2025, time.September, 15, 12, 0, 0),
		},
		{
			name: "October 15",
			date: NewSwedishTime(2025, time.October, 15, 12, 0, 0),
		},
		{
			name: "November 15",
			date: NewSwedishTime(2025, time.November, 15, 12, 0, 0),
		},
		{
			name: "December 15",
			date: NewSwedishTime(2025, time.December, 15, 12, 0, 0),
		},
	}

	for _, tt := range nonHolidays {
		t.Run(tt.name, func(t *testing.T) {
			result := sh.IsHoliday(tt.date)
			if result {
				t.Errorf("IsHoliday(%v) = true, want false (should not be a holiday)", tt.date)
			}
		})
	}
}

func TestAddHoliday(t *testing.T) {
	// Test the internal addHoliday method indirectly through NewSwedishHolidays
	sh := NewSwedishHolidays()

	// Verify that all expected holidays are present
	expectedHolidays := []struct {
		month time.Month
		day   int
	}{
		{time.January, 1},
		{time.March, 28},
		{time.March, 29},
		{time.April, 1},
		{time.April, 30},
		{time.May, 1},
		{time.May, 8},
		{time.May, 9},
		{time.June, 5},
		{time.June, 6},
		{time.June, 21},
		{time.November, 1},
		{time.December, 24},
		{time.December, 25},
		{time.December, 26},
		{time.December, 31},
	}

	for _, holiday := range expectedHolidays {
		date := NewSwedishTime(2025, holiday.month, holiday.day, 12, 0, 0)
		if !sh.IsHoliday(date) {
			t.Errorf("Expected %s %d to be a holiday", holiday.month, holiday.day)
		}
	}
}

func TestHolidayYearIndependence(t *testing.T) {
	// Test that holidays work the same way regardless of year
	// (since they use fixed month/day combinations)
	sh := NewSwedishHolidays()

	testCases := []struct {
		month time.Month
		day   int
		years []int
	}{
		{
			month: time.January,
			day:   1,
			years: []int{2020, 2021, 2022, 2023, 2024, 2025, 2026, 2027, 2028, 2029, 2030},
		},
		{
			month: time.December,
			day:   25,
			years: []int{2020, 2021, 2022, 2023, 2024, 2025, 2026, 2027, 2028, 2029, 2030},
		},
		{
			month: time.May,
			day:   1,
			years: []int{2020, 2021, 2022, 2023, 2024, 2025, 2026, 2027, 2028, 2029, 2030},
		},
	}

	for _, tc := range testCases {
		for _, year := range tc.years {
			date := NewSwedishTime(year, tc.month, tc.day, 12, 0, 0)
			if !sh.IsHoliday(date) {
				t.Errorf("Expected %s %d, %d to be a holiday", tc.month, tc.day, year)
			}
		}
	}
}
