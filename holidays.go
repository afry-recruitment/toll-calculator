package main

import "time"

// HolidayChecker interface for checking if a date is a holiday
type HolidayChecker interface {
	IsHoliday(t time.Time) bool
}

// SwedishHolidays implements holiday checking for Swedish public holidays.
// Uses the 2013 holiday dates as a template applied to any year.
type SwedishHolidays struct {
	// holidays maps month -> day for recurring holidays
	holidays map[time.Month]map[int]bool
	// julyFree indicates July is always toll-free (vacation month)
	julyFree bool
}

// NewSwedishHolidays creates a new Swedish holiday checker.
// Uses the original 2013 holidays applied to any year.
func NewSwedishHolidays() *SwedishHolidays {
	sh := &SwedishHolidays{
		holidays: make(map[time.Month]map[int]bool),
		julyFree: true,
	}

	// 2013 holidays used as template for all years
	sh.addHoliday(time.January, 1)   // New Year's Day
	sh.addHoliday(time.March, 28)    // Maundy Thursday
	sh.addHoliday(time.March, 29)    // Good Friday
	sh.addHoliday(time.April, 1)     // Easter Monday
	sh.addHoliday(time.April, 30)    // Walpurgis Night
	sh.addHoliday(time.May, 1)       // Labour Day
	sh.addHoliday(time.May, 8)       // Day before Ascension
	sh.addHoliday(time.May, 9)       // Ascension Day
	sh.addHoliday(time.June, 5)      // Day before National Day
	sh.addHoliday(time.June, 6)      // National Day
	sh.addHoliday(time.June, 21)     // Midsummer Eve
	sh.addHoliday(time.November, 1)  // All Saints' Day
	sh.addHoliday(time.December, 24) // Christmas Eve
	sh.addHoliday(time.December, 25) // Christmas Day
	sh.addHoliday(time.December, 26) // Boxing Day
	sh.addHoliday(time.December, 31) // New Year's Eve

	return sh
}

// addHoliday adds a recurring holiday by month and day
func (sh *SwedishHolidays) addHoliday(month time.Month, day int) {
	if sh.holidays[month] == nil {
		sh.holidays[month] = make(map[int]bool)
	}
	sh.holidays[month][day] = true
}

// IsHoliday checks if the given date is a Swedish public holiday
func (sh *SwedishHolidays) IsHoliday(t time.Time) bool {
	month := t.Month()
	day := t.Day()

	// July is always toll-free in Sweden (vacation month)
	if sh.julyFree && month == time.July {
		return true
	}

	// Check if month/day matches a holiday
	if monthHolidays, ok := sh.holidays[month]; ok {
		return monthHolidays[day]
	}

	return false
}
