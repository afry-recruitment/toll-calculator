package holidays

import (
	"time"
)

type inMemory struct {
	holidays []time.Time
}

func NewInMemory() *inMemory {
	return &inMemory{
		holidays: []time.Time{
			time.Date(2022, time.January, 1, 0, 0, 0, 0, time.UTC),
			time.Date(2022, time.January, 6, 0, 0, 0, 0, time.UTC),
			time.Date(2022, time.April, 15, 0, 0, 0, 0, time.UTC),
			time.Date(2022, time.April, 17, 0, 0, 0, 0, time.UTC),
			time.Date(2022, time.April, 18, 0, 0, 0, 0, time.UTC),
			time.Date(2022, time.May, 1, 0, 0, 0, 0, time.UTC),
			time.Date(2022, time.May, 26, 0, 0, 0, 0, time.UTC),
			time.Date(2022, time.June, 5, 0, 0, 0, 0, time.UTC),
			time.Date(2022, time.June, 6, 0, 0, 0, 0, time.UTC),
			time.Date(2022, time.June, 25, 0, 0, 0, 0, time.UTC),
			time.Date(2022, time.November, 5, 0, 0, 0, 0, time.UTC),
			time.Date(2022, time.December, 24, 0, 0, 0, 0, time.UTC),
			time.Date(2022, time.December, 25, 0, 0, 0, 0, time.UTC),
			time.Date(2022, time.December, 26, 0, 0, 0, 0, time.UTC),
			time.Date(2022, time.December, 31, 0, 0, 0, 0, time.UTC),
			time.Date(2023, time.January, 1, 0, 0, 0, 0, time.UTC),
			time.Date(2023, time.January, 6, 0, 0, 0, 0, time.UTC),
			time.Date(2023, time.April, 7, 0, 0, 0, 0, time.UTC),
			time.Date(2023, time.April, 9, 0, 0, 0, 0, time.UTC),
			time.Date(2023, time.April, 10, 0, 0, 0, 0, time.UTC),
			time.Date(2023, time.May, 1, 0, 0, 0, 0, time.UTC),
			time.Date(2023, time.May, 18, 0, 0, 0, 0, time.UTC),
			time.Date(2023, time.May, 28, 0, 0, 0, 0, time.UTC),
			time.Date(2023, time.June, 6, 0, 0, 0, 0, time.UTC),
			time.Date(2023, time.June, 24, 0, 0, 0, 0, time.UTC),
			time.Date(2023, time.November, 4, 0, 0, 0, 0, time.UTC),
			time.Date(2023, time.December, 24, 0, 0, 0, 0, time.UTC),
			time.Date(2023, time.December, 25, 0, 0, 0, 0, time.UTC),
			time.Date(2023, time.December, 26, 0, 0, 0, 0, time.UTC),
			time.Date(2023, time.December, 31, 0, 0, 0, 0, time.UTC),
		},
	}
}

func (i inMemory) FetchHolidays(years ...int) []time.Time {
	holidays := make([]time.Time, 0, len(i.holidays))

	for _, holiday := range i.holidays {
		for _, year := range years {
			if year == holiday.Year() {
				holidays = append(holidays, holiday)
			}
		}
	}

	return holidays
}
