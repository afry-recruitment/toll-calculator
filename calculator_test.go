package main

import (
	"testing"
	"time"
)

// mockHolidayChecker is a simple mock for testing
type mockHolidayChecker struct {
	holidays map[string]bool
}

func newMockHolidayChecker() *mockHolidayChecker {
	return &mockHolidayChecker{
		holidays: make(map[string]bool),
	}
}

func (m *mockHolidayChecker) IsHoliday(t time.Time) bool {
	key := t.Format("2006-01-02")
	return m.holidays[key]
}

func (m *mockHolidayChecker) setHoliday(date time.Time) {
	key := date.Format("2006-01-02")
	m.holidays[key] = true
}

func TestDefaultFeeSchedule(t *testing.T) {
	schedule := DefaultFeeSchedule()

	if len(schedule) == 0 {
		t.Fatal("DefaultFeeSchedule() should not return empty schedule")
	}

	// Test that all slots are valid (start <= end)
	for i, slot := range schedule {
		startMinutes := slot.StartHour*60 + slot.StartMin
		endMinutes := slot.EndHour*60 + slot.EndMin
		if startMinutes > endMinutes {
			t.Errorf("Slot %d: start time (%d:%02d) should be <= end time (%d:%02d)",
				i, slot.StartHour, slot.StartMin, slot.EndHour, slot.EndMin)
		}
		if slot.Fee < 0 {
			t.Errorf("Slot %d: fee should be non-negative, got %d", i, slot.Fee)
		}
	}

	// Test that expected slots are present
	expectedSlots := []struct {
		startHour, startMin, endHour, endMin, fee int
	}{
		{6, 0, 6, 29, 8},
		{6, 30, 6, 59, 13},
		{7, 0, 7, 59, 18},
		{8, 0, 8, 29, 13},
		{8, 30, 14, 59, 8},
		{15, 0, 15, 29, 13},
		{15, 30, 16, 59, 18},
		{17, 0, 17, 59, 13},
		{18, 0, 18, 29, 8},
	}

	if len(schedule) != len(expectedSlots) {
		t.Errorf("Expected %d slots, got %d", len(expectedSlots), len(schedule))
	}

	for i, expected := range expectedSlots {
		if i >= len(schedule) {
			break
		}
		slot := schedule[i]
		if slot.StartHour != expected.startHour || slot.StartMin != expected.startMin ||
			slot.EndHour != expected.endHour || slot.EndMin != expected.endMin ||
			slot.Fee != expected.fee {
			t.Errorf("Slot %d: expected %d:%02d-%d:%02d fee %d, got %d:%02d-%d:%02d fee %d",
				i, expected.startHour, expected.startMin, expected.endHour, expected.endMin, expected.fee,
				slot.StartHour, slot.StartMin, slot.EndHour, slot.EndMin, slot.Fee)
		}
	}
}

func TestNewTollCalculator(t *testing.T) {
	holidays := newMockHolidayChecker()
	calc := NewTollCalculator(holidays)

	if calc == nil {
		t.Fatal("NewTollCalculator() should not return nil")
	}

	if calc.holidays == nil {
		t.Error("holidays should not be nil")
	}

	if calc.feeSchedule == nil {
		t.Error("feeSchedule should not be nil")
	}

	if len(calc.feeSchedule) == 0 {
		t.Error("feeSchedule should not be empty")
	}

	// Test with nil holidays
	calcNil := NewTollCalculator(nil)
	if calcNil == nil {
		t.Fatal("NewTollCalculator(nil) should not return nil")
	}
	if calcNil.holidays != nil {
		t.Error("holidays should be nil when passed nil")
	}
}

func TestNewTollCalculatorWithSchedule(t *testing.T) {
	holidays := newMockHolidayChecker()
	customSchedule := []FeeSlot{
		{6, 0, 9, 0, 10},
		{9, 1, 17, 0, 15},
		{17, 1, 18, 0, 10},
	}

	calc := NewTollCalculatorWithSchedule(holidays, customSchedule)

	if calc == nil {
		t.Fatal("NewTollCalculatorWithSchedule() should not return nil")
	}

	if len(calc.feeSchedule) != len(customSchedule) {
		t.Errorf("Expected %d slots, got %d", len(customSchedule), len(calc.feeSchedule))
	}

	// Test with empty schedule
	calcEmpty := NewTollCalculatorWithSchedule(holidays, []FeeSlot{})
	if calcEmpty == nil {
		t.Fatal("NewTollCalculatorWithSchedule() should not return nil with empty schedule")
	}
	if len(calcEmpty.feeSchedule) != 0 {
		t.Error("feeSchedule should be empty")
	}

	// Test with nil holidays
	calcNil := NewTollCalculatorWithSchedule(nil, customSchedule)
	if calcNil == nil {
		t.Fatal("NewTollCalculatorWithSchedule(nil, ...) should not return nil")
	}
}

func TestGetTollFee(t *testing.T) {
	holidays := NewSwedishHolidays()
	calc := NewTollCalculator(holidays)

	tests := []struct {
		name     string
		date     time.Time
		expected int
	}{
		// Test all fee slots
		{
			name:     "Early morning 6:00",
			date:     NewSwedishTime(2025, time.January, 15, 6, 0, 0),
			expected: 8,
		},
		{
			name:     "Early morning 6:15",
			date:     NewSwedishTime(2025, time.January, 15, 6, 15, 0),
			expected: 8,
		},
		{
			name:     "Early morning boundary 6:29",
			date:     NewSwedishTime(2025, time.January, 15, 6, 29, 0),
			expected: 8,
		},
		{
			name:     "Pre-rush 6:30",
			date:     NewSwedishTime(2025, time.January, 15, 6, 30, 0),
			expected: 13,
		},
		{
			name:     "Pre-rush 6:45",
			date:     NewSwedishTime(2025, time.January, 15, 6, 45, 0),
			expected: 13,
		},
		{
			name:     "Pre-rush boundary 6:59",
			date:     NewSwedishTime(2025, time.January, 15, 6, 59, 0),
			expected: 13,
		},
		{
			name:     "Morning rush 7:00",
			date:     NewSwedishTime(2025, time.January, 15, 7, 0, 0),
			expected: 18,
		},
		{
			name:     "Morning rush 7:30",
			date:     NewSwedishTime(2025, time.January, 15, 7, 30, 0),
			expected: 18,
		},
		{
			name:     "Morning rush boundary 7:59",
			date:     NewSwedishTime(2025, time.January, 15, 7, 59, 0),
			expected: 18,
		},
		{
			name:     "Post-rush 8:00",
			date:     NewSwedishTime(2025, time.January, 15, 8, 0, 0),
			expected: 13,
		},
		{
			name:     "Post-rush 8:15",
			date:     NewSwedishTime(2025, time.January, 15, 8, 15, 0),
			expected: 13,
		},
		{
			name:     "Post-rush boundary 8:29",
			date:     NewSwedishTime(2025, time.January, 15, 8, 29, 0),
			expected: 13,
		},
		{
			name:     "Daytime 8:30",
			date:     NewSwedishTime(2025, time.January, 15, 8, 30, 0),
			expected: 8,
		},
		{
			name:     "Daytime 12:00",
			date:     NewSwedishTime(2025, time.January, 15, 12, 0, 0),
			expected: 8,
		},
		{
			name:     "Daytime boundary 14:59",
			date:     NewSwedishTime(2025, time.January, 15, 14, 59, 0),
			expected: 8,
		},
		{
			name:     "Pre-rush evening 15:00",
			date:     NewSwedishTime(2025, time.January, 15, 15, 0, 0),
			expected: 13,
		},
		{
			name:     "Pre-rush evening 15:15",
			date:     NewSwedishTime(2025, time.January, 15, 15, 15, 0),
			expected: 13,
		},
		{
			name:     "Pre-rush evening boundary 15:29",
			date:     NewSwedishTime(2025, time.January, 15, 15, 29, 0),
			expected: 13,
		},
		{
			name:     "Evening rush 15:30",
			date:     NewSwedishTime(2025, time.January, 15, 15, 30, 0),
			expected: 18,
		},
		{
			name:     "Evening rush 16:00",
			date:     NewSwedishTime(2025, time.January, 15, 16, 0, 0),
			expected: 18,
		},
		{
			name:     "Evening rush boundary 16:59",
			date:     NewSwedishTime(2025, time.January, 15, 16, 59, 0),
			expected: 18,
		},
		{
			name:     "Post-rush evening 17:00",
			date:     NewSwedishTime(2025, time.January, 15, 17, 0, 0),
			expected: 13,
		},
		{
			name:     "Post-rush evening 17:30",
			date:     NewSwedishTime(2025, time.January, 15, 17, 30, 0),
			expected: 13,
		},
		{
			name:     "Post-rush evening boundary 17:59",
			date:     NewSwedishTime(2025, time.January, 15, 17, 59, 0),
			expected: 13,
		},
		{
			name:     "Early evening 18:00",
			date:     NewSwedishTime(2025, time.January, 15, 18, 0, 0),
			expected: 8,
		},
		{
			name:     "Early evening 18:15",
			date:     NewSwedishTime(2025, time.January, 15, 18, 15, 0),
			expected: 8,
		},
		{
			name:     "Early evening boundary 18:29",
			date:     NewSwedishTime(2025, time.January, 15, 18, 29, 0),
			expected: 8,
		},
		// Test night time (free)
		{
			name:     "Night 18:30",
			date:     NewSwedishTime(2025, time.January, 15, 18, 30, 0),
			expected: 0,
		},
		{
			name:     "Night 20:00",
			date:     NewSwedishTime(2025, time.January, 15, 20, 0, 0),
			expected: 0,
		},
		{
			name:     "Night 23:59",
			date:     NewSwedishTime(2025, time.January, 15, 23, 59, 0),
			expected: 0,
		},
		{
			name:     "Night 0:00",
			date:     NewSwedishTime(2025, time.January, 15, 0, 0, 0),
			expected: 0,
		},
		{
			name:     "Night 5:59",
			date:     NewSwedishTime(2025, time.January, 15, 5, 59, 0),
			expected: 0,
		},
		// Test weekends (free)
		{
			name:     "Saturday",
			date:     NewSwedishTime(2025, time.January, 18, 10, 0, 0), // Saturday
			expected: 0,
		},
		{
			name:     "Sunday",
			date:     NewSwedishTime(2025, time.January, 19, 10, 0, 0), // Sunday
			expected: 0,
		},
		// Test holidays (free) - using actual SwedishHolidays
		{
			name:     "Holiday",
			date:     NewSwedishTime(2025, time.January, 1, 10, 0, 0), // New Year's Day
			expected: 0,
		},
	}

	for _, tt := range tests {
		t.Run(tt.name, func(t *testing.T) {
			result := calc.GetTollFee(tt.date)
			if result != tt.expected {
				t.Errorf("GetTollFee(%v) = %d, want %d", tt.date, result, tt.expected)
			}
		})
	}
}

func TestGetTollFeeWithCustomSchedule(t *testing.T) {
	holidays := newMockHolidayChecker()
	customSchedule := []FeeSlot{
		{9, 0, 17, 0, 20},
	}
	calc := NewTollCalculatorWithSchedule(holidays, customSchedule)

	tests := []struct {
		name     string
		date     time.Time
		expected int
	}{
		{
			name:     "Within custom slot",
			date:     NewSwedishTime(2025, time.January, 15, 12, 0, 0),
			expected: 20,
		},
		{
			name:     "Before custom slot",
			date:     NewSwedishTime(2025, time.January, 15, 8, 0, 0),
			expected: 0,
		},
		{
			name:     "After custom slot",
			date:     NewSwedishTime(2025, time.January, 15, 18, 0, 0),
			expected: 0,
		},
	}

	for _, tt := range tests {
		t.Run(tt.name, func(t *testing.T) {
			result := calc.GetTollFee(tt.date)
			if result != tt.expected {
				t.Errorf("GetTollFee(%v) = %d, want %d", tt.date, result, tt.expected)
			}
		})
	}
}

func TestIsTollFreeDate(t *testing.T) {
	holidays := NewSwedishHolidays()
	calc := NewTollCalculator(holidays)

	tests := []struct {
		name     string
		date     time.Time
		expected bool
	}{
		{
			name:     "Monday",
			date:     NewSwedishTime(2025, time.January, 13, 10, 0, 0), // Monday
			expected: false,
		},
		{
			name:     "Tuesday",
			date:     NewSwedishTime(2025, time.January, 14, 10, 0, 0), // Tuesday
			expected: false,
		},
		{
			name:     "Wednesday",
			date:     NewSwedishTime(2025, time.January, 15, 10, 0, 0), // Wednesday
			expected: false,
		},
		{
			name:     "Thursday",
			date:     NewSwedishTime(2025, time.January, 16, 10, 0, 0), // Thursday
			expected: false,
		},
		{
			name:     "Friday",
			date:     NewSwedishTime(2025, time.January, 17, 10, 0, 0), // Friday
			expected: false,
		},
		{
			name:     "Saturday",
			date:     NewSwedishTime(2025, time.January, 18, 10, 0, 0), // Saturday
			expected: true,
		},
		{
			name:     "Sunday",
			date:     NewSwedishTime(2025, time.January, 19, 10, 0, 0), // Sunday
			expected: true,
		},
		{
			name:     "Holiday on weekday",
			date:     NewSwedishTime(2025, time.January, 1, 10, 0, 0), // New Year's Day (Wednesday)
			expected: true,
		},
		{
			name:     "Regular weekday",
			date:     NewSwedishTime(2025, time.January, 15, 10, 0, 0), // Regular Wednesday
			expected: false,
		},
	}

	for _, tt := range tests {
		t.Run(tt.name, func(t *testing.T) {
			result := calc.isTollFreeDate(tt.date)
			if result != tt.expected {
				t.Errorf("isTollFreeDate(%v) = %v, want %v", tt.date, result, tt.expected)
			}
		})
	}

	// Test with nil holidays
	calcNil := NewTollCalculator(nil)
	regularDay := NewSwedishTime(2025, time.January, 15, 10, 0, 0)
	if calcNil.isTollFreeDate(regularDay) {
		t.Error("isTollFreeDate should return false for regular weekday when holidays is nil")
	}
}

func TestGroupByDay(t *testing.T) {
	holidays := newMockHolidayChecker()
	calc := NewTollCalculator(holidays)

	tests := []struct {
		name     string
		passes   []time.Time
		expected int // expected number of days
	}{
		{
			name:     "Single day",
			passes:   []time.Time{NewSwedishTime(2025, time.January, 15, 10, 0, 0)},
			expected: 1,
		},
		{
			name: "Multiple passes same day",
			passes: []time.Time{
				NewSwedishTime(2025, time.January, 15, 10, 0, 0),
				NewSwedishTime(2025, time.January, 15, 14, 0, 0),
				NewSwedishTime(2025, time.January, 15, 16, 0, 0),
			},
			expected: 1,
		},
		{
			name: "Two consecutive days",
			passes: []time.Time{
				NewSwedishTime(2025, time.January, 15, 10, 0, 0),
				NewSwedishTime(2025, time.January, 16, 10, 0, 0),
			},
			expected: 2,
		},
		{
			name: "Multiple days",
			passes: []time.Time{
				NewSwedishTime(2025, time.January, 15, 10, 0, 0),
				NewSwedishTime(2025, time.January, 16, 10, 0, 0),
				NewSwedishTime(2025, time.January, 17, 10, 0, 0),
			},
			expected: 3,
		},
		{
			name: "Day boundary crossing",
			passes: []time.Time{
				NewSwedishTime(2025, time.January, 15, 23, 30, 0),
				NewSwedishTime(2025, time.January, 16, 0, 30, 0),
			},
			expected: 2,
		},
		{
			name: "Year boundary",
			passes: []time.Time{
				NewSwedishTime(2024, time.December, 31, 23, 30, 0),
				NewSwedishTime(2025, time.January, 1, 0, 30, 0),
			},
			expected: 2,
		},
		{
			name: "Month boundary",
			passes: []time.Time{
				NewSwedishTime(2025, time.January, 31, 10, 0, 0),
				NewSwedishTime(2025, time.February, 1, 10, 0, 0),
			},
			expected: 2,
		},
	}

	for _, tt := range tests {
		t.Run(tt.name, func(t *testing.T) {
			groups := calc.groupByDay(tt.passes)
			if len(groups) != tt.expected {
				t.Errorf("groupByDay() returned %d days, want %d", len(groups), tt.expected)
			}

			// Verify all passes are accounted for
			totalPasses := 0
			for _, dayPasses := range groups {
				totalPasses += len(dayPasses)
			}
			if totalPasses != len(tt.passes) {
				t.Errorf("groupByDay() lost passes: expected %d, got %d", len(tt.passes), totalPasses)
			}
		})
	}
}

func TestCalculateSingleDayToll(t *testing.T) {
	holidays := newMockHolidayChecker()
	calc := NewTollCalculator(holidays)

	tests := []struct {
		name     string
		passes   []time.Time
		expected int
	}{
		{
			name:     "Empty passes",
			passes:   []time.Time{},
			expected: 0,
		},
		{
			name:     "Single pass",
			passes:   []time.Time{NewSwedishTime(2025, time.January, 15, 10, 0, 0)},
			expected: 8, // Daytime fee
		},
		{
			name: "Two passes same window",
			passes: []time.Time{
				NewSwedishTime(2025, time.January, 15, 10, 0, 0),  // 8 SEK
				NewSwedishTime(2025, time.January, 15, 10, 30, 0), // 8 SEK, same window
			},
			expected: 8, // Only highest fee in window
		},
		{
			name: "Two passes different fees same window",
			passes: []time.Time{
				NewSwedishTime(2025, time.January, 15, 7, 0, 0),  // 18 SEK (rush hour)
				NewSwedishTime(2025, time.January, 15, 7, 30, 0), // 18 SEK, same window
			},
			expected: 18, // Highest fee in window
		},
		{
			name: "Two passes different windows",
			passes: []time.Time{
				NewSwedishTime(2025, time.January, 15, 7, 0, 0),  // 18 SEK
				NewSwedishTime(2025, time.January, 15, 8, 30, 0), // 8 SEK, different window (>60 min)
			},
			expected: 26, // 18 + 8
		},
		{
			name: "Multiple passes in same window",
			passes: []time.Time{
				NewSwedishTime(2025, time.January, 15, 7, 0, 0),  // 18 SEK
				NewSwedishTime(2025, time.January, 15, 7, 15, 0), // 18 SEK
				NewSwedishTime(2025, time.January, 15, 7, 30, 0), // 18 SEK
			},
			expected: 18, // Only highest fee
		},
		{
			name: "Multiple passes different windows",
			passes: []time.Time{
				NewSwedishTime(2025, time.January, 15, 7, 0, 0),   // 18 SEK
				NewSwedishTime(2025, time.January, 15, 8, 30, 0),  // 8 SEK, new window
				NewSwedishTime(2025, time.January, 15, 15, 30, 0), // 18 SEK, new window
			},
			expected: 44, // 18 + 8 + 18
		},
		{
			name: "Passes exactly 60 minutes apart",
			passes: []time.Time{
				NewSwedishTime(2025, time.January, 15, 7, 0, 0), // 18 SEK
				NewSwedishTime(2025, time.January, 15, 8, 0, 0), // 13 SEK, same window (<=60 min)
			},
			expected: 18, // Only highest fee
		},
		{
			name: "Passes just over 60 minutes apart",
			passes: []time.Time{
				NewSwedishTime(2025, time.January, 15, 7, 0, 0), // 18 SEK
				NewSwedishTime(2025, time.January, 15, 8, 1, 0), // 13 SEK, new window (>60 min)
			},
			expected: 31, // 18 + 13
		},
		{
			name: "Daily cap exceeded",
			passes: []time.Time{
				NewSwedishTime(2025, time.January, 15, 7, 0, 0),   // 18 SEK
				NewSwedishTime(2025, time.January, 15, 8, 30, 0),  // 8 SEK, new window
				NewSwedishTime(2025, time.January, 15, 10, 0, 0),  // 8 SEK, new window
				NewSwedishTime(2025, time.January, 15, 12, 0, 0),  // 8 SEK, new window
				NewSwedishTime(2025, time.January, 15, 15, 30, 0), // 18 SEK, new window
				NewSwedishTime(2025, time.January, 15, 17, 0, 0),  // 13 SEK, new window
			},
			expected: 60, // Capped at 60 SEK (18+8+8+8+18+13 = 73)
		},
		{
			name: "Unsorted passes",
			passes: []time.Time{
				NewSwedishTime(2025, time.January, 15, 10, 0, 0),  // 8 SEK
				NewSwedishTime(2025, time.January, 15, 7, 0, 0),   // 18 SEK
				NewSwedishTime(2025, time.January, 15, 15, 30, 0), // 18 SEK
			},
			expected: 44, // Should be sorted: 18 + 8 + 18
		},
	}

	for _, tt := range tests {
		t.Run(tt.name, func(t *testing.T) {
			result := calc.calculateSingleDayToll(tt.passes)
			if result != tt.expected {
				t.Errorf("calculateSingleDayToll() = %d, want %d", result, tt.expected)
			}
		})
	}
}

func TestCalculateToll(t *testing.T) {
	holidays := NewSwedishHolidays()
	calc := NewTollCalculator(holidays)
	car := Car{}

	tests := []struct {
		name     string
		vehicle  Vehicle
		passes   []time.Time
		expected int
	}{
		{
			name:     "Empty passes",
			vehicle:  car,
			passes:   []time.Time{},
			expected: 0,
		},
		{
			name:     "Nil vehicle",
			vehicle:  nil,
			passes:   []time.Time{NewSwedishTime(2025, time.January, 15, 10, 0, 0)},
			expected: 0,
		},
		{
			name:     "Toll-free vehicle",
			vehicle:  Motorbike{},
			passes:   []time.Time{NewSwedishTime(2025, time.January, 15, 10, 0, 0)},
			expected: 0,
		},
		{
			name:     "Single pass",
			vehicle:  car,
			passes:   []time.Time{NewSwedishTime(2025, time.January, 15, 10, 0, 0)},
			expected: 8,
		},
		{
			name:    "Multiple passes same day",
			vehicle: car,
			passes: []time.Time{
				NewSwedishTime(2025, time.January, 15, 7, 0, 0),  // 18 SEK
				NewSwedishTime(2025, time.January, 15, 8, 30, 0), // 8 SEK, new window
			},
			expected: 26,
		},
		{
			name:    "Multiple days",
			vehicle: car,
			passes: []time.Time{
				NewSwedishTime(2025, time.January, 15, 7, 0, 0),  // Day 1: 18 SEK
				NewSwedishTime(2025, time.January, 15, 8, 30, 0), // Day 1: 8 SEK, new window
				NewSwedishTime(2025, time.January, 16, 7, 0, 0),  // Day 2: 18 SEK
				NewSwedishTime(2025, time.January, 16, 8, 30, 0), // Day 2: 8 SEK, new window
			},
			expected: 52, // (18+8) + (18+8) = 52
		},
		{
			name:    "Multiple days with daily cap",
			vehicle: car,
			passes: []time.Time{
				// Day 1: exceeds cap
				NewSwedishTime(2025, time.January, 15, 7, 0, 0),   // 18 SEK
				NewSwedishTime(2025, time.January, 15, 8, 30, 0),  // 8 SEK
				NewSwedishTime(2025, time.January, 15, 10, 0, 0),  // 8 SEK
				NewSwedishTime(2025, time.January, 15, 12, 0, 0),  // 8 SEK
				NewSwedishTime(2025, time.January, 15, 15, 30, 0), // 18 SEK
				NewSwedishTime(2025, time.January, 15, 17, 0, 0),  // 13 SEK
				// Day 2: normal
				NewSwedishTime(2025, time.January, 16, 7, 0, 0),  // 18 SEK
				NewSwedishTime(2025, time.January, 16, 8, 30, 0), // 8 SEK
			},
			expected: 86, // 60 (capped) + 26 = 86
		},
		{
			name:    "Weekend passes",
			vehicle: car,
			passes: []time.Time{
				NewSwedishTime(2025, time.January, 18, 10, 0, 0), // Saturday
				NewSwedishTime(2025, time.January, 19, 10, 0, 0), // Sunday
			},
			expected: 0,
		},
		{
			name:    "Holiday passes",
			vehicle: car,
			passes: []time.Time{
				NewSwedishTime(2025, time.January, 1, 10, 0, 0), // New Year's Day
			},
			expected: 0,
		},
		{
			name:    "Mixed weekday and weekend",
			vehicle: car,
			passes: []time.Time{
				NewSwedishTime(2025, time.January, 15, 10, 0, 0), // Wednesday: 8 SEK
				NewSwedishTime(2025, time.January, 18, 10, 0, 0), // Saturday: 0 SEK
			},
			expected: 8,
		},
	}

	for _, tt := range tests {
		t.Run(tt.name, func(t *testing.T) {
			result := calc.CalculateToll(tt.vehicle, tt.passes)
			if result != tt.expected {
				t.Errorf("CalculateToll() = %d, want %d", result, tt.expected)
			}
		})
	}
}

func TestCalculateTollEdgeCases(t *testing.T) {
	holidays := NewSwedishHolidays()
	calc := NewTollCalculator(holidays)
	car := Car{}

	tests := []struct {
		name     string
		vehicle  Vehicle
		passes   []time.Time
		expected int
	}{
		{
			name:    "All toll-free vehicle types",
			vehicle: Motorbike{},
			passes: []time.Time{
				NewSwedishTime(2025, time.January, 15, 7, 0, 0),
				NewSwedishTime(2025, time.January, 15, 15, 30, 0),
			},
			expected: 0,
		},
		{
			name:     "Tractor",
			vehicle:  Tractor{},
			passes:   []time.Time{NewSwedishTime(2025, time.January, 15, 7, 0, 0)},
			expected: 0,
		},
		{
			name:     "Emergency",
			vehicle:  Emergency{},
			passes:   []time.Time{NewSwedishTime(2025, time.January, 15, 7, 0, 0)},
			expected: 0,
		},
		{
			name:     "Diplomat",
			vehicle:  Diplomat{},
			passes:   []time.Time{NewSwedishTime(2025, time.January, 15, 7, 0, 0)},
			expected: 0,
		},
		{
			name:     "Foreign",
			vehicle:  Foreign{},
			passes:   []time.Time{NewSwedishTime(2025, time.January, 15, 7, 0, 0)},
			expected: 0,
		},
		{
			name:     "Military",
			vehicle:  Military{},
			passes:   []time.Time{NewSwedishTime(2025, time.January, 15, 7, 0, 0)},
			expected: 0,
		},
		{
			name:    "Day boundary at midnight",
			vehicle: car,
			passes: []time.Time{
				NewSwedishTime(2025, time.January, 15, 23, 30, 0), // Day 1: 0 SEK (night)
				NewSwedishTime(2025, time.January, 16, 0, 30, 0),  // Day 2: 0 SEK (night)
			},
			expected: 0,
		},
		{
			name:    "Year boundary",
			vehicle: car,
			passes: []time.Time{
				NewSwedishTime(2024, time.December, 31, 10, 0, 0), // 0 SEK (New Year's Eve - holiday)
				NewSwedishTime(2025, time.January, 1, 10, 0, 0),   // 0 SEK (New Year's Day - holiday)
			},
			expected: 0,
		},
		{
			name:    "Month boundary",
			vehicle: car,
			passes: []time.Time{
				NewSwedishTime(2025, time.January, 30, 10, 0, 0), // Thursday: 8 SEK
				NewSwedishTime(2025, time.January, 31, 10, 0, 0), // Friday: 8 SEK
				NewSwedishTime(2025, time.February, 3, 10, 0, 0), // Monday: 8 SEK
			},
			expected: 24, // 8 + 8 + 8 = 24 SEK (3 separate days)
		},
		{
			name:    "Very large number of passes",
			vehicle: car,
			passes: func() []time.Time {
				passes := make([]time.Time, 100)
				for i := 0; i < 100; i++ {
					passes[i] = NewSwedishTime(2025, time.January, 15, 7+i/10, i%60, 0)
				}
				return passes
			}(),
			expected: 60, // Should be capped
		},
	}

	for _, tt := range tests {
		t.Run(tt.name, func(t *testing.T) {
			result := calc.CalculateToll(tt.vehicle, tt.passes)
			if result != tt.expected {
				t.Errorf("CalculateToll() = %d, want %d", result, tt.expected)
			}
		})
	}
}

func TestCalculateTollWithCustomHolidays(t *testing.T) {
	holidays := newMockHolidayChecker()
	holidays.setHoliday(NewSwedishTime(2025, time.January, 20, 12, 0, 0))
	calc := NewTollCalculator(holidays)
	car := Car{}

	tests := []struct {
		name     string
		passes   []time.Time
		expected int
	}{
		{
			name:     "Custom holiday",
			passes:   []time.Time{NewSwedishTime(2025, time.January, 20, 10, 0, 0)},
			expected: 0,
		},
		{
			name:     "Regular day",
			passes:   []time.Time{NewSwedishTime(2025, time.January, 21, 10, 0, 0)},
			expected: 8,
		},
	}

	for _, tt := range tests {
		t.Run(tt.name, func(t *testing.T) {
			result := calc.CalculateToll(car, tt.passes)
			if result != tt.expected {
				t.Errorf("CalculateToll() = %d, want %d", result, tt.expected)
			}
		})
	}
}

func TestCalculateTollNilHolidays(t *testing.T) {
	calc := NewTollCalculator(nil)
	car := Car{}

	// Test that nil holidays doesn't cause issues
	passes := []time.Time{NewSwedishTime(2025, time.January, 15, 10, 0, 0)}
	result := calc.CalculateToll(car, passes)
	if result != 8 {
		t.Errorf("CalculateToll() with nil holidays = %d, want 8", result)
	}

	// Test that weekends still work
	weekendPasses := []time.Time{NewSwedishTime(2025, time.January, 18, 10, 0, 0)} // Saturday
	weekendResult := calc.CalculateToll(car, weekendPasses)
	if weekendResult != 0 {
		t.Errorf("CalculateToll() on weekend with nil holidays = %d, want 0", weekendResult)
	}
}

func TestCalculateTollConsistency(t *testing.T) {
	holidays := newMockHolidayChecker()
	calc := NewTollCalculator(holidays)
	car := Car{}

	// Test that same passes in different order give same result
	passes1 := []time.Time{
		NewSwedishTime(2025, time.January, 15, 7, 0, 0),
		NewSwedishTime(2025, time.January, 15, 8, 30, 0),
		NewSwedishTime(2025, time.January, 15, 15, 30, 0),
	}

	passes2 := []time.Time{
		NewSwedishTime(2025, time.January, 15, 15, 30, 0),
		NewSwedishTime(2025, time.January, 15, 7, 0, 0),
		NewSwedishTime(2025, time.January, 15, 8, 30, 0),
	}

	result1 := calc.CalculateToll(car, passes1)
	result2 := calc.CalculateToll(car, passes2)

	if result1 != result2 {
		t.Errorf("CalculateToll() should be consistent: got %d and %d for same passes in different order", result1, result2)
	}
}

func TestCalculateToll60MinuteWindow(t *testing.T) {
	holidays := newMockHolidayChecker()
	calc := NewTollCalculator(holidays)
	car := Car{}

	tests := []struct {
		name     string
		passes   []time.Time
		expected int
	}{
		{
			name: "Exactly 60 minutes - same window",
			passes: []time.Time{
				NewSwedishTime(2025, time.January, 15, 7, 0, 0), // 18 SEK
				NewSwedishTime(2025, time.January, 15, 8, 0, 0), // 13 SEK, exactly 60 min
			},
			expected: 18, // Highest fee in window
		},
		{
			name: "59 minutes - same window",
			passes: []time.Time{
				NewSwedishTime(2025, time.January, 15, 7, 0, 0),  // 18 SEK
				NewSwedishTime(2025, time.January, 15, 7, 59, 0), // 18 SEK, 59 min
			},
			expected: 18,
		},
		{
			name: "61 minutes - different windows",
			passes: []time.Time{
				NewSwedishTime(2025, time.January, 15, 7, 0, 0), // 18 SEK
				NewSwedishTime(2025, time.January, 15, 8, 1, 0), // 13 SEK, 61 min
			},
			expected: 31, // 18 + 13
		},
		{
			name: "Multiple passes in 60-minute window",
			passes: []time.Time{
				NewSwedishTime(2025, time.January, 15, 7, 0, 0),  // 18 SEK
				NewSwedishTime(2025, time.January, 15, 7, 15, 0), // 18 SEK
				NewSwedishTime(2025, time.January, 15, 7, 30, 0), // 18 SEK
				NewSwedishTime(2025, time.January, 15, 7, 45, 0), // 18 SEK
				NewSwedishTime(2025, time.January, 15, 7, 59, 0), // 18 SEK
			},
			expected: 18, // Only highest fee
		},
		{
			name: "Window with different fees",
			passes: []time.Time{
				NewSwedishTime(2025, time.January, 15, 6, 0, 0),  // 8 SEK
				NewSwedishTime(2025, time.January, 15, 6, 45, 0), // 13 SEK
				NewSwedishTime(2025, time.January, 15, 7, 0, 0),  // 18 SEK
			},
			expected: 18, // Highest fee in window
		},
	}

	for _, tt := range tests {
		t.Run(tt.name, func(t *testing.T) {
			result := calc.CalculateToll(car, tt.passes)
			if result != tt.expected {
				t.Errorf("CalculateToll() = %d, want %d", result, tt.expected)
			}
		})
	}
}

func TestCalculateTollDailyCap(t *testing.T) {
	holidays := newMockHolidayChecker()
	calc := NewTollCalculator(holidays)
	car := Car{}

	// Create passes that would exceed 60 SEK
	passes := []time.Time{
		NewSwedishTime(2025, time.January, 15, 7, 0, 0),   // 18 SEK
		NewSwedishTime(2025, time.January, 15, 8, 30, 0),  // 8 SEK, new window
		NewSwedishTime(2025, time.January, 15, 10, 0, 0),  // 8 SEK, new window
		NewSwedishTime(2025, time.January, 15, 12, 0, 0),  // 8 SEK, new window
		NewSwedishTime(2025, time.January, 15, 15, 30, 0), // 18 SEK, new window
		NewSwedishTime(2025, time.January, 15, 17, 0, 0),  // 13 SEK, new window
		NewSwedishTime(2025, time.January, 15, 18, 0, 0),  // 8 SEK, new window
	}
	// Total would be: 18 + 8 + 8 + 8 + 18 + 13 + 8 = 81 SEK, but capped at 60

	result := calc.CalculateToll(car, passes)
	if result != 60 {
		t.Errorf("CalculateToll() with daily cap = %d, want 60", result)
	}

	// Test that exactly 60 SEK is not capped
	exactPasses := []time.Time{
		NewSwedishTime(2025, time.January, 15, 7, 0, 0),   // 18 SEK
		NewSwedishTime(2025, time.January, 15, 8, 30, 0),  // 8 SEK
		NewSwedishTime(2025, time.January, 15, 10, 0, 0),  // 8 SEK
		NewSwedishTime(2025, time.January, 15, 12, 0, 0),  // 8 SEK
		NewSwedishTime(2025, time.January, 15, 15, 30, 0), // 18 SEK
	}
	// Total: 18 + 8 + 8 + 8 + 18 = 60 SEK

	exactResult := calc.CalculateToll(car, exactPasses)
	if exactResult != 60 {
		t.Errorf("CalculateToll() with exactly 60 SEK = %d, want 60", exactResult)
	}
}

func TestCalculateTollMultiDayCap(t *testing.T) {
	holidays := newMockHolidayChecker()
	calc := NewTollCalculator(holidays)
	car := Car{}

	// Test that each day has its own cap
	passes := []time.Time{
		// Day 1: exceeds cap
		NewSwedishTime(2025, time.January, 15, 7, 0, 0),   // 18 SEK
		NewSwedishTime(2025, time.January, 15, 8, 30, 0),  // 8 SEK
		NewSwedishTime(2025, time.January, 15, 10, 0, 0),  // 8 SEK
		NewSwedishTime(2025, time.January, 15, 12, 0, 0),  // 8 SEK
		NewSwedishTime(2025, time.January, 15, 15, 30, 0), // 18 SEK
		NewSwedishTime(2025, time.January, 15, 17, 0, 0),  // 13 SEK
		// Day 2: exceeds cap
		NewSwedishTime(2025, time.January, 16, 7, 0, 0),   // 18 SEK
		NewSwedishTime(2025, time.January, 16, 8, 30, 0),  // 8 SEK
		NewSwedishTime(2025, time.January, 16, 10, 0, 0),  // 8 SEK
		NewSwedishTime(2025, time.January, 16, 12, 0, 0),  // 8 SEK
		NewSwedishTime(2025, time.January, 16, 15, 30, 0), // 18 SEK
		NewSwedishTime(2025, time.January, 16, 17, 0, 0),  // 13 SEK
	}

	result := calc.CalculateToll(car, passes)
	expected := 120 // 60 (day 1) + 60 (day 2)
	if result != expected {
		t.Errorf("CalculateToll() with multi-day cap = %d, want %d", result, expected)
	}
}
