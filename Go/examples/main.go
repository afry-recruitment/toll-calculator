package main

import (
	"fmt"
	"log"
	"time"

	"github.com/cmoscofian/toll-calculator/internal/core/domain"
	"github.com/cmoscofian/toll-calculator/internal/core/service"
	"github.com/cmoscofian/toll-calculator/internal/repositories/fees"
	"github.com/cmoscofian/toll-calculator/internal/repositories/holidays"
)

func main() {
	// holidaysRepo := holidays.NewFS("holidays.json")
	holidaysRepo := holidays.NewInMemory()
	holidays := holidaysRepo.FetchHolidays(2022)

	feesRepo := fees.NewFS("fees.json")
	//feesRepo := fees.NewInMemory("fees.json")
	fees := feesRepo.FetchFees()

	freeTollVehicles := []domain.Vehicle{{Type: domain.VehicleMotorbike}}

	tollCalculator := service.NewTollCalculator(freeTollVehicles, holidays, fees)
	car := domain.Vehicle{Type: domain.VehicleCar}

	passes := []time.Time{
		time.Date(2022, time.January, 3, 6, 30, 0, 0, time.UTC),
		time.Date(2022, time.January, 3, 7, 20, 0, 0, time.UTC),
		time.Date(2022, time.January, 3, 5, 30, 0, 0, time.UTC),
		time.Date(2022, time.January, 3, 12, 10, 0, 0, time.UTC),
		time.Date(2022, time.January, 3, 15, 15, 0, 0, time.UTC),
	}

	fee, err := tollCalculator.CalculateTollFee(car, passes...)
	if err != nil {
		log.Printf("error calculating toll fee: %s", err.Error())
	}

	fmt.Println(fee)
}
