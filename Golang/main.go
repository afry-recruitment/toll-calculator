package main

import (
	"fmt"
	"math/rand"
	"slices"
	"strings"
	"time"
	"tollfeecalculator/tollcalculator"
	"tollfeecalculator/vehicle"
)

// Available vehicles for simulation, i'll add one toll free and a car which has to pay..
var vehicles = []vehicle.Vehicle{
	vehicle.Car{},
	vehicle.Motorbike{},
}

// Main will simulate a random vehicle passing at a random date and time in 2013
func main() {
	random := rand.New(rand.NewSource(time.Now().UnixNano()))

	// Choose a random vehicle
	vehicle := vehicles[random.Intn(len(vehicles))]
	vehicleName := fmt.Sprintf("%T", vehicle)
	vehicleName = vehicleName[strings.LastIndex(vehicleName, ".")+1:]

	// Generate random timestamps
	numPasses := random.Intn(8) + 1
	year := 2013
	month := time.Month(random.Intn(12) + 1)
	day := random.Intn(28) + 1

	var times []time.Time
	for range numPasses {
		hour := random.Intn(24)
		minute := random.Intn(60)
		t := time.Date(year, month, day, hour, minute, 0, 0, time.Local)
		times = append(times, t)
	}

	//Sort the timestamps
	if len(times) > 1 {
		slices.SortFunc(times, func(a, b time.Time) int {
			return a.Compare(b)
		})
	}

	// Print the results of the simulated trip through our toll system
	fmt.Printf("\n🚗 Simulating trip for %s\n", vehicleName)
	fmt.Printf("📅 Date: %04d-%02d-%02d (%s) \n", year, month, day, times[0].Weekday())
	if vehicle.IsTollFree() {
		fmt.Println("🟢 This is a toll-free vehicle, enjoy!")
	} else if tollcalculator.IsTollFreeDate(times[0]) {
		fmt.Println("🟢 This is a toll-free day!")
	}
	fmt.Println("──────────────────────────────")

	rawTotal := 0
	for _, t := range times {
		fee, err := tollcalculator.GetTollFee(vehicle, []time.Time{t})
		if err != nil {
			fmt.Printf("error getting toll fee: %v\n", err)
			return
		}
		rawTotal += fee
		//Printing the cost for this passage, which is simple the cost of the passage at this point in time
		fmt.Printf("%s → %2d SEK\n", t.Format("15:04"), fee)
	}
	// Compute actual final fee (respecting hourly window + daily cap)
	final, err := tollcalculator.GetTollFee(vehicle, times)
	if err != nil {
		fmt.Printf("error getting final toll fee: %v\n", err)
		return
	}

	fmt.Println("──────────────────────────────")
	fmt.Printf("🧾 Raw total (if all charged): %d SEK\n", rawTotal)
	fmt.Printf("💰 Total charged (after hourly rules): %d SEK\n", final)

}
