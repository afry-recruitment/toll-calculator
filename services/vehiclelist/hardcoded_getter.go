package vehiclelist

import "afry-toll-calculator/models"

type hardcodedGetter struct{}

func NewHardcodedGetter() Getter {
	return &hardcodedGetter{}
}

func (h *hardcodedGetter) GetVehicleList() []models.Vehicle {
	return []models.Vehicle{
		models.NewVehicle("car", false),

		models.NewVehicle("motorbike", true),
		models.NewVehicle("tractor", true),
		models.NewVehicle("emergency", true),
		models.NewVehicle("diplomat", true),
		models.NewVehicle("foreign", true),
		models.NewVehicle("military", true),
	}
}
