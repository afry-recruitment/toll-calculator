package vehiclelist

import "afry-toll-calculator/models"

type Getter interface {
	GetVehicleList() []models.Vehicle
}
