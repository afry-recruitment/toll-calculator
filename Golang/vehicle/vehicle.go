package vehicle

type Vehicle interface {
	GetVehicleType() string
	IsTollFree() bool
}

type Motorbike struct{}

func (Motorbike) GetVehicleType() string { return "Motorbike" }
func (Motorbike) IsTollFree() bool       { return true }

type Car struct{}

func (Car) GetVehicleType() string { return "Car" }
func (Car) IsTollFree() bool       { return false }

type Tractor struct{}

func (Tractor) GetVehicleType() string { return "Tractor" }
func (Tractor) IsTollFree() bool       { return true }

type Emergency struct{}

func (Emergency) GetVehicleType() string { return "Emergency" }
func (Emergency) IsTollFree() bool       { return true }

type Diplomat struct{}

func (Diplomat) GetVehicleType() string { return "Diplomat" }
func (Diplomat) IsTollFree() bool       { return true }

type Foreign struct{}

func (Foreign) GetVehicleType() string { return "Foreign" }
func (Foreign) IsTollFree() bool       { return true }

type Military struct{}

func (Military) GetVehicleType() string { return "Military" }
func (Military) IsTollFree() bool       { return true }
