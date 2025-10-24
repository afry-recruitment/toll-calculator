package test

import (
	"encoding/json"
	"fmt"
	"os"
	"testing"
	"time"
	"tollfeecalculator/tollcalculator"
	"tollfeecalculator/vehicle"
)

type TestCase struct {
	Vehicle     string   `json:"vehicle"`
	Timestamps  []string `json:"timestamps"`
	Expected    int      `json:"expected"`
	ExpectError bool     `json:"expect_error"`
}

var vehicleType = map[string]vehicle.Vehicle{
	"Motorbike": vehicle.Motorbike{},
	"Car":       vehicle.Car{},
	"Tractor":   vehicle.Tractor{},
	"Diplomat":  vehicle.Diplomat{},
	"Military":  vehicle.Military{},
	"Emergency": vehicle.Emergency{},
	"Foreign":   vehicle.Foreign{},
}

func loadTestData(t *testing.T, filepath string) []TestCase {
	t.Helper()
	data, err := os.ReadFile(filepath)
	if err != nil {
		t.Fatalf("failed to read %s: %v", filepath, err)
	}
	var cases []TestCase
	if err := json.Unmarshal(data, &cases); err != nil {
		t.Fatalf("failed to unmarshal %s: %v", filepath, err)
	}
	return cases
}

func parseTimes(t *testing.T, timestamps []string) ([]time.Time, error) {
	t.Helper()
	var times []time.Time
	for _, ts := range timestamps {
		parsed, err := time.Parse(time.RFC3339, ts)
		if err != nil {
			return nil, fmt.Errorf("failed to parse %q: %w", ts, err)
		}
		times = append(times, parsed)
	}
	return times, nil
}

func runTests(t *testing.T, cases []TestCase) {
	for _, c := range cases {
		t.Run(c.Vehicle, func(t *testing.T) {
			vehicle, ok := vehicleType[c.Vehicle]
			if !ok {
				if c.ExpectError {
					return
				}
				t.Errorf("unknown vehicle type: %s", c.Vehicle)
			}
			times, parseErr := parseTimes(t, c.Timestamps)

			if parseErr != nil {
				if c.ExpectError {
					return
				}
				t.Errorf("%s: unexpected parse error: %v", c.Vehicle, parseErr)
			}
			result, err := tollcalculator.GetTollFee(vehicle, times)
			if c.ExpectError {
				expectedMsg := "timestamps span multiple days, only one day allowed per calculation"
				if err.Error() != expectedMsg {
					t.Errorf("%s: expected error %q, got %q", c.Vehicle, expectedMsg, err.Error())
				}
				return
			}

			if err != nil {
				t.Errorf("%s: unexpected error: %v", c.Vehicle, err)
				return
			}
			if result != c.Expected {
				t.Errorf("%s: expected %d, result %d", c.Vehicle, c.Expected, result)
			}
		})
	}
}

// TestFeeFreeVehicles verify that the toll free vehicles never get charged
func TestFeeFreeVehicles(t *testing.T) {
	cases := loadTestData(t, "testdata/toll_free.json")
	runTests(t, cases)
}

// TestWeekendsAndHolidays tests certain dates (holidays) and weekends to ensure they are toll free
func TestWeekendsAndHolidays(t *testing.T) {
	cases := loadTestData(t, "testdata/holiday_and_weekends.json")
	runTests(t, cases)
}

// TestHourlyWindowCap ensure that only the highest fee within a 60-minute window applies.
func TestHourlyWindowCap(t *testing.T) {
	cases := loadTestData(t, "testdata/hourly_window.json")
	runTests(t, cases)
}

// TestDailyCap verifies that a drivers fees per day can never exceed 60 SEK
func TestDailyCap(t *testing.T) {
	cases := loadTestData(t, "testdata/daily_cap.json")
	runTests(t, cases)
}

// TestFeeBreakpoints tests the exact seconds before, on and after a fee changes to verify that the correct fee is applied
func TestFeeBreakpoints(t *testing.T) {
	cases := loadTestData(t, "testdata/fee_breakpoints.json")
	runTests(t, cases)
}

// TestMultipleDays test should throw error if multiple dates are sent in one go, if that happens something is messed up (We run this daily, so timestamps should only be from one day at a time)
func TestMultipleDays(t *testing.T) {
	cases := loadTestData(t, "testdata/multiple_days.json")
	runTests(t, cases)
}

// TestInvalidVehicle tests that invalid vehicles triggers an error
func TestInvalidVehicle(t *testing.T) {
	cases := loadTestData(t, "testdata/invalid_vehicle.json")
	runTests(t, cases)
}

// TestInvalidTimestamp verifies that invalid timestamps cause parsing errors.
func TestInvalidTimestamp(t *testing.T) {
	cases := loadTestData(t, "testdata/invalid_timestamp.json")
	runTests(t, cases)
}
