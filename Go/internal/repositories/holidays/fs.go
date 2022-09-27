package holidays

import (
	"encoding/json"
	"log"
	"os"
	"time"

	"github.com/cmoscofian/toll-calculator/internal/core/domain"
)

type fs struct {
	path string
}

func NewFS(path string) *fs {
	return &fs{
		path: path,
	}
}

func (f fs) FetchHolidays(years ...int) []time.Time {
	bs, err := os.ReadFile(f.path)
	if err != nil {
		log.Panicf("unable to load holidays file from path %s due to %s", f.path, err.Error())
	}

	dates := make([]string, 0)
	if err := json.Unmarshal(bs, &dates); err != nil {
		log.Panicf("unable to parse holiday data due to %s", err.Error())
	}

	holidays := make([]time.Time, 0, len(dates))
	for _, date := range dates {
		holiday, err := time.Parse(domain.HolidaysDateLayout, date)
		if err != nil {
			log.Panicf("unable to parse holiday from date %s due to %s", date, err.Error())
		}

		for _, year := range years {
			if holiday.Year() == year {
				holidays = append(holidays, holiday)
			}
		}
	}

	return holidays
}
