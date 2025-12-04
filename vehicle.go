package main

// Vehicle interface defines the contract for all vehicle types
type Vehicle interface {
	GetType() string
	IsTollFree() bool
}

// Car represents a standard car - subject to toll fees
type Car struct{}

func (c Car) GetType() string {
	return "Car"
}

func (c Car) IsTollFree() bool {
	return false
}

// Motorbike is toll-free
type Motorbike struct{}

func (m Motorbike) GetType() string {
	return "Motorbike"
}

func (m Motorbike) IsTollFree() bool {
	return true
}

// Tractor is toll-free
type Tractor struct{}

func (t Tractor) GetType() string {
	return "Tractor"
}

func (t Tractor) IsTollFree() bool {
	return true
}

// Emergency vehicles are toll-free
type Emergency struct{}

func (e Emergency) GetType() string {
	return "Emergency"
}

func (e Emergency) IsTollFree() bool {
	return true
}

// Diplomat vehicles are toll-free
type Diplomat struct{}

func (d Diplomat) GetType() string {
	return "Diplomat"
}

func (d Diplomat) IsTollFree() bool {
	return true
}

// Foreign registered vehicles are toll-free
type Foreign struct{}

func (f Foreign) GetType() string {
	return "Foreign"
}

func (f Foreign) IsTollFree() bool {
	return true
}

// Military vehicles are toll-free
type Military struct{}

func (m Military) GetType() string {
	return "Military"
}

func (m Military) IsTollFree() bool {
	return true
}
