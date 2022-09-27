package port

import (
	"time"

	"github.com/cmoscofian/toll-calculator/internal/core/domain"
)

type TollCalculator interface {
	// CalculateTollFee returns a float64 with the daily fee value due and an
	// error.
	//
	// This method is responsible for calculating the fee amounts due, based on
	// two params the domain.Vehicle and the time.Time passes that represents
	// everytime a driver has driven pass the toll. If at any point, the process
	// fails, it should return a descriptive error.
	CalculateTollFee(domain.Vehicle, ...time.Time) (float64, error)
}
