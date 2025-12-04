package main

import (
	"testing"
	"time"
)

func TestSwedishTimezone(t *testing.T) {
	// Test that SwedishTimezone is loaded correctly
	if SwedishTimezone == nil {
		t.Fatal("SwedishTimezone should not be nil")
	}

	// Test that it's either Europe/Stockholm or the fallback CET
	expectedName := SwedishTimezone.String()
	if expectedName != "Europe/Stockholm" && expectedName != "CET" {
		t.Errorf("Expected SwedishTimezone to be 'Europe/Stockholm' or 'CET', got '%s'", expectedName)
	}

	// Test that the timezone offset is correct (Stockholm is UTC+1 in winter, UTC+2 in summer)
	// We'll test with a known date in winter (January) and summer (July)
	winterTime := time.Date(2025, 1, 15, 12, 0, 0, 0, SwedishTimezone)
	_, offset := winterTime.Zone()
	if offset != 3600 && offset != 7200 { // UTC+1 or UTC+2
		t.Errorf("Expected winter offset to be 3600 or 7200 seconds, got %d", offset)
	}

	summerTime := time.Date(2025, 7, 15, 12, 0, 0, 0, SwedishTimezone)
	_, summerOffset := summerTime.Zone()
	if summerOffset != 3600 && summerOffset != 7200 { // UTC+1 or UTC+2
		t.Errorf("Expected summer offset to be 3600 or 7200 seconds, got %d", summerOffset)
	}
}

func TestInSwedishTime(t *testing.T) {
	tests := []struct {
		name     string
		input    time.Time
		expected time.Time
	}{
		{
			name:     "UTC time conversion",
			input:    time.Date(2025, 1, 15, 12, 0, 0, 0, time.UTC),
			expected: time.Date(2025, 1, 15, 13, 0, 0, 0, SwedishTimezone), // UTC+1 in winter
		},
		{
			name:     "Different timezone conversion",
			input:    time.Date(2025, 1, 15, 12, 0, 0, 0, time.FixedZone("EST", -5*60*60)),
			expected: time.Date(2025, 1, 15, 18, 0, 0, 0, SwedishTimezone), // EST+6 = UTC+1
		},
		{
			name:     "Already in Swedish time",
			input:    time.Date(2025, 1, 15, 12, 0, 0, 0, SwedishTimezone),
			expected: time.Date(2025, 1, 15, 12, 0, 0, 0, SwedishTimezone),
		},
		{
			name:     "Summer time (DST)",
			input:    time.Date(2025, 7, 15, 12, 0, 0, 0, time.UTC),
			expected: time.Date(2025, 7, 15, 14, 0, 0, 0, SwedishTimezone), // UTC+2 in summer
		},
		{
			name:     "Midnight",
			input:    time.Date(2025, 1, 15, 0, 0, 0, 0, time.UTC),
			expected: time.Date(2025, 1, 15, 1, 0, 0, 0, SwedishTimezone),
		},
		{
			name:     "End of day",
			input:    time.Date(2025, 1, 15, 23, 59, 59, 0, time.UTC),
			expected: time.Date(2025, 1, 16, 0, 59, 59, 0, SwedishTimezone),
		},
	}

	for _, tt := range tests {
		t.Run(tt.name, func(t *testing.T) {
			result := InSwedishTime(tt.input)

			// Compare the actual time values (not the timezone, since we're converting)
			if !result.Equal(tt.expected) {
				t.Errorf("InSwedishTime() = %v, want %v", result, tt.expected)
			}

			// Verify the result is in Swedish timezone
			if result.Location() != SwedishTimezone {
				t.Errorf("InSwedishTime() location = %v, want %v", result.Location(), SwedishTimezone)
			}
		})
	}
}

func TestNewSwedishTime(t *testing.T) {
	tests := []struct {
		name          string
		year          int
		month         time.Month
		day           int
		hour          int
		min           int
		sec           int
		expectedYear  int
		expectedMonth time.Month
		expectedDay   int
		expectedHour  int
		expectedMin   int
		expectedSec   int
	}{
		{
			name:          "Normal date and time",
			year:          2025,
			month:         time.January,
			day:           15,
			hour:          12,
			min:           30,
			sec:           45,
			expectedYear:  2025,
			expectedMonth: time.January,
			expectedDay:   15,
			expectedHour:  12,
			expectedMin:   30,
			expectedSec:   45,
		},
		{
			name:          "Midnight",
			year:          2025,
			month:         time.January,
			day:           1,
			hour:          0,
			min:           0,
			sec:           0,
			expectedYear:  2025,
			expectedMonth: time.January,
			expectedDay:   1,
			expectedHour:  0,
			expectedMin:   0,
			expectedSec:   0,
		},
		{
			name:          "End of day",
			year:          2025,
			month:         time.December,
			day:           31,
			hour:          23,
			min:           59,
			sec:           59,
			expectedYear:  2025,
			expectedMonth: time.December,
			expectedDay:   31,
			expectedHour:  23,
			expectedMin:   59,
			expectedSec:   59,
		},
		{
			name:          "Summer date",
			year:          2025,
			month:         time.July,
			day:           15,
			hour:          14,
			min:           30,
			sec:           0,
			expectedYear:  2025,
			expectedMonth: time.July,
			expectedDay:   15,
			expectedHour:  14,
			expectedMin:   30,
			expectedSec:   0,
		},
		{
			name:          "DST transition month (March)",
			year:          2025,
			month:         time.March,
			day:           31,
			hour:          10,
			min:           15,
			sec:           30,
			expectedYear:  2025,
			expectedMonth: time.March,
			expectedDay:   31,
			expectedHour:  10,
			expectedMin:   15,
			expectedSec:   30,
		},
		{
			name:          "DST transition month (October)",
			year:          2025,
			month:         time.October,
			day:           27,
			hour:          10,
			min:           15,
			sec:           30,
			expectedYear:  2025,
			expectedMonth: time.October,
			expectedDay:   27,
			expectedHour:  10,
			expectedMin:   15,
			expectedSec:   30,
		},
	}

	for _, tt := range tests {
		t.Run(tt.name, func(t *testing.T) {
			result := NewSwedishTime(tt.year, tt.month, tt.day, tt.hour, tt.min, tt.sec)

			// Verify the time components
			if result.Year() != tt.expectedYear {
				t.Errorf("NewSwedishTime() year = %d, want %d", result.Year(), tt.expectedYear)
			}
			if result.Month() != tt.expectedMonth {
				t.Errorf("NewSwedishTime() month = %v, want %v", result.Month(), tt.expectedMonth)
			}
			if result.Day() != tt.expectedDay {
				t.Errorf("NewSwedishTime() day = %d, want %d", result.Day(), tt.expectedDay)
			}
			if result.Hour() != tt.expectedHour {
				t.Errorf("NewSwedishTime() hour = %d, want %d", result.Hour(), tt.expectedHour)
			}
			if result.Minute() != tt.expectedMin {
				t.Errorf("NewSwedishTime() minute = %d, want %d", result.Minute(), tt.expectedMin)
			}
			if result.Second() != tt.expectedSec {
				t.Errorf("NewSwedishTime() second = %d, want %d", result.Second(), tt.expectedSec)
			}

			// Verify the timezone
			if result.Location() != SwedishTimezone {
				t.Errorf("NewSwedishTime() location = %v, want %v", result.Location(), SwedishTimezone)
			}
		})
	}
}

func TestLoadSwedishTimezone(t *testing.T) {
	// Test that loadSwedishTimezone returns a valid location
	// Note: This is indirectly tested through SwedishTimezone, but we can test the function directly
	loc := loadSwedishTimezone()

	if loc == nil {
		t.Fatal("loadSwedishTimezone() should not return nil")
	}

	// Verify it's either Europe/Stockholm or the fallback CET
	locName := loc.String()
	if locName != "Europe/Stockholm" && locName != "CET" {
		t.Errorf("Expected location name to be 'Europe/Stockholm' or 'CET', got '%s'", locName)
	}

	// Test that it can be used to create times
	testTime := time.Date(2025, 1, 15, 12, 0, 0, 0, loc)
	if testTime.Location() != loc {
		t.Errorf("Created time should have the correct location")
	}
}

func TestTimezoneConsistency(t *testing.T) {
	// Test that InSwedishTime and NewSwedishTime produce consistent results
	// Create a time using NewSwedishTime
	createdTime := NewSwedishTime(2025, 1, 15, 12, 30, 45)

	// Convert it using InSwedishTime (should be a no-op since it's already in Swedish time)
	convertedTime := InSwedishTime(createdTime)

	// They should be equal
	if !createdTime.Equal(convertedTime) {
		t.Errorf("Created time %v should equal converted time %v", createdTime, convertedTime)
	}

	// Both should have the same location
	if createdTime.Location() != convertedTime.Location() {
		t.Errorf("Locations should match: %v vs %v", createdTime.Location(), convertedTime.Location())
	}
}

func TestTimezoneEdgeCases(t *testing.T) {
	// Test edge cases for timezone handling

	// Test with zero time
	zeroTime := time.Time{}
	swedishZero := InSwedishTime(zeroTime)
	if swedishZero.Location() != SwedishTimezone {
		t.Errorf("Zero time conversion should use Swedish timezone")
	}

	// Test with very old date
	oldTime := time.Date(1900, 1, 1, 12, 0, 0, 0, time.UTC)
	swedishOld := InSwedishTime(oldTime)
	if swedishOld.Location() != SwedishTimezone {
		t.Errorf("Old time conversion should use Swedish timezone")
	}

	// Test with future date
	futureTime := time.Date(2100, 12, 31, 23, 59, 59, 0, time.UTC)
	swedishFuture := InSwedishTime(futureTime)
	if swedishFuture.Location() != SwedishTimezone {
		t.Errorf("Future time conversion should use Swedish timezone")
	}

	// Test NewSwedishTime with edge case values
	edgeTime := NewSwedishTime(2000, 1, 1, 0, 0, 0)
	if edgeTime.Location() != SwedishTimezone {
		t.Errorf("Edge case time should use Swedish timezone")
	}
}

func TestLoadSwedishTimezoneFallback(t *testing.T) {
	// Test the fallback logic by directly testing what happens when LoadLocation fails
	// Since we can't easily force LoadLocation to fail, we test the fallback behavior
	// by verifying that the FixedZone fallback (CET) works correctly

	// Create a fallback timezone matching the one in loadSwedishTimezone
	fallbackTZ := time.FixedZone("CET", 1*60*60)

	// Verify the fallback timezone properties
	if fallbackTZ.String() != "CET" {
		t.Errorf("Fallback timezone name should be 'CET', got '%s'", fallbackTZ.String())
	}

	// Test that the fallback timezone has the correct offset (UTC+1)
	testTime := time.Date(2025, 1, 15, 12, 0, 0, 0, fallbackTZ)
	_, offset := testTime.Zone()
	if offset != 3600 { // 1 hour = 3600 seconds
		t.Errorf("Fallback timezone offset should be 3600 seconds (UTC+1), got %d", offset)
	}

	// Verify that if SwedishTimezone is the fallback, it behaves correctly
	// This tests the error path logic indirectly
	if SwedishTimezone.String() == "CET" {
		// If we're using the fallback, verify it works
		testSwedishTime := time.Date(2025, 1, 15, 12, 0, 0, 0, SwedishTimezone)
		_, swedishOffset := testSwedishTime.Zone()
		if swedishOffset != 3600 {
			t.Errorf("SwedishTimezone fallback offset should be 3600 seconds, got %d", swedishOffset)
		}
	}

	// Test that the fallback can be used to create and convert times
	fallbackTime := time.Date(2025, 1, 15, 12, 0, 0, 0, fallbackTZ)
	convertedTime := fallbackTime.In(SwedishTimezone)

	// The conversion should work regardless of whether we're using the real timezone or fallback
	if convertedTime.Location() != SwedishTimezone {
		t.Errorf("Time converted to SwedishTimezone should have correct location")
	}
}

// TestLoadSwedishTimezoneErrorPath tests the error handling logic
// This test verifies the fallback behavior works correctly by testing with an invalid timezone name.
func TestLoadSwedishTimezoneErrorPath(t *testing.T) {
	// Test that loadSwedishTimezone handles errors gracefully
	// We test this by verifying the function always returns a valid location

	loc := loadSwedishTimezone()
	if loc == nil {
		t.Fatal("loadSwedishTimezone() must never return nil, even on error")
	}

	// Verify it returns either the real timezone or the fallback
	locName := loc.String()
	validNames := []string{"Europe/Stockholm", "CET"}
	isValid := false
	for _, name := range validNames {
		if locName == name {
			isValid = true
			break
		}
	}
	if !isValid {
		t.Errorf("loadSwedishTimezone() returned invalid location: %s", locName)
	}

	// Test that the returned location can be used for time operations
	testCases := []struct {
		year  int
		month time.Month
		day   int
		hour  int
		min   int
		sec   int
	}{
		{2025, time.January, 15, 12, 0, 0},
		{2025, time.July, 15, 12, 0, 0},
		{2025, time.December, 31, 23, 59, 59},
	}

	for _, tc := range testCases {
		t.Run("time_operations", func(t *testing.T) {
			testTime := time.Date(tc.year, tc.month, tc.day, tc.hour, tc.min, tc.sec, 0, loc)
			if testTime.Location() != loc {
				t.Errorf("Created time should have the correct location")
			}
			// Verify time operations work
			if testTime.Year() != tc.year {
				t.Errorf("Year mismatch: got %d, want %d", testTime.Year(), tc.year)
			}
		})
	}

	// Test the error path by using an invalid timezone name
	// This will trigger the fallback logic and test lines 14-18
	invalidTZ := loadSwedishTimezoneWithName("Invalid/Timezone/Name")
	if invalidTZ == nil {
		t.Fatal("loadSwedishTimezoneWithName() must never return nil, even on error")
	}

	// Verify it falls back to CET
	if invalidTZ.String() != "CET" {
		t.Errorf("Expected fallback to 'CET', got '%s'", invalidTZ.String())
	}

	// Verify the fallback has the correct offset (UTC+1)
	testTime := time.Date(2025, 1, 15, 12, 0, 0, 0, invalidTZ)
	_, offset := testTime.Zone()
	if offset != 3600 { // 1 hour = 3600 seconds
		t.Errorf("Fallback timezone offset should be 3600 seconds (UTC+1), got %d", offset)
	}

	// Verify the fallback can be used for time operations
	if testTime.Location() != invalidTZ {
		t.Errorf("Created time should have the correct location")
	}
}
