package models

type VehicleType string

type Vehicle interface {
	GetType() VehicleType
	IsTollFree() bool
}

type vehicle struct {
	vehicleType VehicleType
	tollFree    bool
}

func NewVehicle(vehicleType VehicleType, tollFree bool) Vehicle {
	return vehicle{vehicleType, tollFree}
}

func (v vehicle) GetType() VehicleType {
	return v.vehicleType
}

func (v vehicle) IsTollFree() bool {
	return v.tollFree
}
