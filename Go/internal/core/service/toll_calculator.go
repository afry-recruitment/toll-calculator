package service

import (
	"sort"
	"time"

	"github.com/cmoscofian/toll-calculator/internal/core/domain"
)

type tollCalculator struct {
	fees             []domain.Fee
	holidays         []time.Time
	tollFreeVehicles []domain.Vehicle
}

func NewTollCalculator(tollFreeVehicles []domain.Vehicle, holidays []time.Time, fees []domain.Fee) *tollCalculator {
	return &tollCalculator{
		fees:             fees,
		tollFreeVehicles: tollFreeVehicles,
		holidays:         holidays,
	}
}

func (t tollCalculator) CalculateTollFee(vehicle domain.Vehicle, passes ...time.Time) (float64, error) {
	if t.isTollFreeVehicle(vehicle) {
		return 0, nil
	}

	if t.isTollFreeDay(passes[0]) {
		return 0, nil
	}

	sort.SliceStable(passes, func(i, j int) bool {
		return passes[i].Before(passes[j])
	})

	totalFee := t.getFee(passes[0])

	for i := 1; i < len(passes); i++ {
		prev := passes[i-1]
		curr := passes[i]

		prevFee := t.getFee(prev)
		currFee := t.getFee(curr)

		sub := curr.Sub(prev)
		if sub <= domain.MinTimeBetweenCharge {
			if totalFee > 0 {
				totalFee -= prevFee
			}

			if currFee >= prevFee {
				prevFee = currFee
			}

			totalFee += prevFee
		} else {
			totalFee += currFee
		}

		if totalFee >= domain.MaxDailyFeeAllowed {
			return domain.MaxDailyFeeAllowed, nil
		}
	}

	return totalFee, nil
}

func (t tollCalculator) isTollFreeVehicle(vehicle domain.Vehicle) bool {
	for _, v := range t.tollFreeVehicles {
		if v == vehicle {
			return true
		}
	}
	return false
}

func (t tollCalculator) isTollFreeDay(day time.Time) bool {
	if day.Weekday() == time.Saturday || day.Weekday() == time.Sunday {
		return true
	}

	for _, d := range t.holidays {
		if day.Truncate(24 * time.Hour).Equal(d.Truncate(24 * time.Hour)) {
			return true
		}
	}

	return false
}

func (t tollCalculator) getFee(pass time.Time) float64 {
	for _, f := range t.fees {
		if f.Check(pass) {
			return f.Fee
		}
	}
	return 0.0
}
