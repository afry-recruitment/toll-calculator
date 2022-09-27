package domain

import "time"

const (
	MaxDailyFeeAllowed   float64       = 60.0
	MinTimeBetweenCharge time.Duration = time.Minute * 60
)

const (
	MinFeeValue float64 = 8
	MaxFeeValue float64 = 18
)

const (
	HolidaysDateLayout string = "02-01-06"
)
