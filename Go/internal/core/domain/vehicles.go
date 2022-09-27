package domain

type vehicleType int

const (
	VehicleCar vehicleType = iota
	VehicleDiplomat
	VehicleEmergency
	VehicleForeign
	VehicleMilitary
	VehicleMotorbike
	VehicleTractor
)

type Vehicle struct {
	Type vehicleType
}
