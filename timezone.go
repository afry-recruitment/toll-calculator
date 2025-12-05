package main

import (
	"log"
	"time"
)

// SwedishTimezone returns the Europe/Stockholm timezone.
// Falls back to UTC+1 (CET) if the timezone database is unavailable.
var SwedishTimezone = loadSwedishTimezone()

func loadSwedishTimezone() *time.Location {
	return loadSwedishTimezoneWithName("Europe/Stockholm")
}

// loadSwedishTimezoneWithName loads a timezone by name, with fallback to CET.
// This function is extracted to allow testing of the error path.
func loadSwedishTimezoneWithName(tzName string) *time.Location {
	loc, err := time.LoadLocation(tzName)
	if err != nil {
		log.Printf("warning: could not load %s timezone: %v, using UTC+1 fallback", tzName, err)
		// Fallback to fixed UTC+1 (CET) if timezone database unavailable
		loc = time.FixedZone("CET", 1*60*60)
	}
	return loc
}

// InSwedishTime converts a time to Swedish timezone.
// This ensures consistent toll calculation regardless of server location.
func InSwedishTime(t time.Time) time.Time {
	return t.In(SwedishTimezone)
}

// NewSwedishTime creates a time in Swedish timezone.
func NewSwedishTime(year int, month time.Month, day, hour, min, sec int) time.Time {
	return time.Date(year, month, day, hour, min, sec, 0, SwedishTimezone)
}
