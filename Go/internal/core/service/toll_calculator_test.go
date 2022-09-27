package service

import (
	"testing"
	"time"

	"github.com/cmoscofian/toll-calculator/internal/core/domain"
	"github.com/cmoscofian/toll-calculator/internal/repositories/fees"
	"github.com/cmoscofian/toll-calculator/internal/repositories/holidays"
)

func TestCalculateTollFee(t *testing.T) {
	tollFreeVehicles := []domain.Vehicle{
		{Type: domain.VehicleDiplomat},
		{Type: domain.VehicleEmergency},
		{Type: domain.VehicleForeign},
		{Type: domain.VehicleMilitary},
		{Type: domain.VehicleMotorbike},
		{Type: domain.VehicleTractor},
	}

	holidays := holidays.NewInMemory()
	fees := fees.NewInMemory()
	service := NewTollCalculator(tollFreeVehicles, holidays.FetchHolidays(2022), fees.FetchFees())

	t.Run("should return zero when calculating for a toll free vehicle", func(t *testing.T) {
		t.Parallel()
		vehicle := domain.Vehicle{Type: domain.VehicleDiplomat}
		fee, err := service.CalculateTollFee(vehicle)
		if err != nil {
			t.FailNow()
		}

		if fee != 0 {
			t.FailNow()
		}
	})

	t.Run("should return zero when calculating for a saturday", func(t *testing.T) {
		t.Parallel()
		vehicle := domain.Vehicle{Type: domain.VehicleCar}
		fee, err := service.CalculateTollFee(vehicle, time.Date(2022, time.October, 1, 0, 0, 0, 0, time.UTC))
		if err != nil {
			t.FailNow()
		}

		if fee != 0 {
			t.FailNow()
		}
	})

	t.Run("should return zero when calculating for a sunday", func(t *testing.T) {
		t.Parallel()
		vehicle := domain.Vehicle{Type: domain.VehicleCar}
		fee, err := service.CalculateTollFee(vehicle, time.Date(2022, time.October, 2, 0, 0, 0, 0, time.UTC))
		if err != nil {
			t.FailNow()
		}

		if fee != 0 {
			t.FailNow()
		}
	})

	t.Run("should return zero when calculating for a holiday", func(t *testing.T) {
		t.Parallel()
		vehicle := domain.Vehicle{Type: domain.VehicleCar}
		fee, err := service.CalculateTollFee(vehicle, time.Date(2022, time.January, 1, 0, 0, 0, 0, time.UTC))
		if err != nil {
			t.FailNow()
		}

		if fee != 0 {
			t.FailNow()
		}
	})

	t.Run("should return just the highest fee when they happen under 60 minutes from each other", func(t *testing.T) {
		t.Parallel()
		vehicle := domain.Vehicle{Type: domain.VehicleCar}
		passes := []time.Time{
			time.Date(2022, time.January, 3, 6, 30, 0, 0, time.UTC),
			time.Date(2022, time.January, 3, 7, 20, 0, 0, time.UTC),
		}
		fee, err := service.CalculateTollFee(vehicle, passes...)
		if err != nil {
			t.FailNow()
		}

		if fee != 18 {
			t.FailNow()
		}
	})

	t.Run("should sum fees when they happen with more than 60 minutes from each other", func(t *testing.T) {
		t.Parallel()
		vehicle := domain.Vehicle{Type: domain.VehicleCar}
		passes := []time.Time{
			time.Date(2022, time.January, 3, 6, 30, 0, 0, time.UTC),
			time.Date(2022, time.January, 3, 7, 31, 0, 0, time.UTC),
		}
		fee, err := service.CalculateTollFee(vehicle, passes...)
		if err != nil {
			t.FailNow()
		}

		if fee != 31 {
			t.FailNow()
		}
	})

	t.Run("should handle multiple fees correctly", func(t *testing.T) {
		t.Parallel()
		vehicle := domain.Vehicle{Type: domain.VehicleCar}
		passes := []time.Time{
			time.Date(2022, time.January, 3, 6, 30, 0, 0, time.UTC),
			time.Date(2022, time.January, 3, 7, 31, 0, 0, time.UTC),
			time.Date(2022, time.January, 3, 9, 00, 0, 0, time.UTC),
		}
		fee, err := service.CalculateTollFee(vehicle, passes...)
		if err != nil {
			t.FailNow()
		}

		if fee != 39 {
			t.FailNow()
		}
	})

	t.Run("should return the max value when max reached", func(t *testing.T) {
		t.Parallel()
		vehicle := domain.Vehicle{Type: domain.VehicleCar}
		passes := []time.Time{
			time.Date(2022, time.January, 3, 6, 30, 0, 0, time.UTC),
			time.Date(2022, time.January, 3, 7, 31, 0, 0, time.UTC),
			time.Date(2022, time.January, 3, 8, 32, 0, 0, time.UTC),
			time.Date(2022, time.January, 3, 9, 33, 0, 0, time.UTC),
			time.Date(2022, time.January, 3, 10, 34, 0, 0, time.UTC),
			time.Date(2022, time.January, 3, 11, 35, 0, 0, time.UTC),
			time.Date(2022, time.January, 3, 12, 36, 0, 0, time.UTC),
			time.Date(2022, time.January, 3, 13, 37, 0, 0, time.UTC),
		}
		fee, err := service.CalculateTollFee(vehicle, passes...)
		if err != nil {
			t.FailNow()
		}

		if fee != domain.MaxDailyFeeAllowed {
			t.FailNow()
		}
	})
}
