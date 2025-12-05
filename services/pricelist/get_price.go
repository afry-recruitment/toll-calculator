package pricelist

import (
	"time"
)

// GetPrice returns the price for the entry time, based on the available priceBlocks. PriceBlocks
// definition and minutesFromMidnight format is intentional, for readability purposes and easier/faster lookup.
//
// Notes about the original code logic and inconsistencies:
// - The original code had overlapping and inconsistent time ranges, e.g.:
//   - 08:30–14:59 was written as `hour >= 8 && hour <= 14 && minute >= 30 && minute <= 59`,
//     which technically only includes minutes 30–59 of each hour, not the full block.
//   - 15:30–16:59 was expressed as `hour == 15 && minute >= 0 || hour == 16 && minute <= 59`,
//     which overlaps with the previous block and evaluates incorrectly due to operator precedence.
//   - These issues were corrected in the `priceBlocks` slice by defining continuous, non-overlapping
//     ranges.
func (s *svc) GetPrice(entry time.Time) int {
	minutesFromMidnight := entry.Hour()*60 + entry.Minute()

	return s.priceOfMinute[minutesFromMidnight]
}

// mustMinutes returns the number of minutes after midnight. If parsing fails, the function will panic.
func mustMinutes(s string) int {
	t, err := time.Parse("15:04", s)
	if err != nil {
		panic(err)
	}

	return t.Hour()*60 + t.Minute()
}
