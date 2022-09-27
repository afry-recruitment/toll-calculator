package port

import (
	"time"

	"github.com/cmoscofian/toll-calculator/internal/core/domain"
)

type Holidays interface {
	// FetchHolidays returns a slice of time.Time and an error.
	//
	// This method should handle fetching the holidays data based on the years
	// params. Otherwise, returns an error.
	FetchHolidays(years ...int) ([]time.Time, error)
}

type Fees interface {
	// FetchFees returns a slice of domain.Fee.
	//
	// This method should handle fetching the fees configurations to be used by
	// the core service.
	FetchFees() []domain.Fee
}
