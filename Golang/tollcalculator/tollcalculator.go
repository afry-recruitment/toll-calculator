package tollcalculator

import (
	"errors"
	"slices"
	"time"
	"tollfeecalculator/vehicle"
)

func GetTollFee(vehicle vehicle.Vehicle, timestamps []time.Time) (int, error) {
	//If timestamps are missing, return 0 fee
	if len(timestamps) == 0 {
		return 0, nil
	}

	//If timestamps are out of order, sort them
	if len(timestamps) > 1 {
		slices.SortFunc(timestamps, func(a, b time.Time) int {
			return a.Compare(b)
		})
	}

	//Check that all timestamps are from the same day, otherwise throw error
	firstDay := timestamps[0].YearDay()
	firstYear := timestamps[0].Year()
	for _, t := range timestamps {
		if t.Year() != firstYear || t.YearDay() != firstDay {
			return 0, errors.New("timestamps span multiple days, only one day allowed per calculation")
		}
	}

	//Set intervalStart to first date in the slice, get tempFee
	intervalStart := timestamps[0]
	totalFee := 0
	tempFee := getFee(vehicle, intervalStart)

	//Loop through all the timestamps
	for i := 1; i < len(timestamps); i++ {

		//Save the current timestamp and get the fee for the timestamp
		currentTime := timestamps[i]
		fee := getFee(vehicle, currentTime)
		//Get the diffrence in minutes to the start interval
		diffMinutes := currentTime.Sub(intervalStart).Minutes()

		//Check if diffrence in time is less then 60 minutes between the timestamps
		if diffMinutes <= 60 {
			if fee >= tempFee {
				tempFee = fee
			}
		} else { // Otherwise start a new time interval
			totalFee += tempFee
			tempFee = fee
			intervalStart = currentTime
		}

	}
	// Adding the new 1 hour interval top fee to the total
	totalFee += tempFee

	// Since the daily total is 60, we cut of any fee above it
	if totalFee > 60 {
		totalFee = 60
	}

	return totalFee, nil
}

func getFee(vehicle vehicle.Vehicle, date time.Time) int {
	// Return 0 if vehicle is tollfree or the date is matching a holiday or weekend
	if IsTollFreeDate(date) || vehicle.IsTollFree() {
		return 0
	}

	hour, minute := date.Hour(), date.Minute()
	// Fee table with diffrent rates diffrent periods of the day
	switch {
	case hour == 6 && minute < 30:
		return 8
	case hour == 6 && minute >= 30:
		return 13
	case hour == 7:
		return 18
	case hour == 8 && minute < 30:
		return 13
	case (hour == 8 && minute >= 30) || (hour >= 9 && hour <= 14):
		return 8
	case hour == 15 && minute < 30:
		return 13
	case (hour == 15 && minute >= 30) || hour == 16:
		return 18
	case hour == 17:
		return 13
	case hour == 18 && minute < 30:
		return 8
	default:
		return 0
	}
}

func IsTollFreeDate(date time.Time) bool {
	//Weekend toll free
	if date.Weekday() == time.Saturday || date.Weekday() == time.Sunday {
		return true
	}

	year := date.Year()
	month := date.Month()
	day := date.Day()

	//Toll free holidays 2013 Sweden
	if year == 2013 {
		if month == 1 && day == 1 ||
			month == 3 && (day == 28 || day == 29) ||
			month == 4 && (day == 1 || day == 30) ||
			month == 5 && (day == 1 || day == 8 || day == 9) ||
			month == 6 && (day == 5 || day == 6 || day == 21) ||
			month == 7 ||
			month == 11 && day == 1 ||
			month == 12 && (day == 24 || day == 25 || day == 26 || day == 31) {

			return true
		}
	}

	return false

}
