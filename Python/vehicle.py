from enum import StrEnum

class VehicleType(StrEnum):
    """All existing vehicle types."""
    CAR = "Car"
    MOTORBIKE = "Motorbike"
    TRACTOR = "Tractor"
    EMERGENCY = "Emergency"
    DIPLOMAT = "Diplomat"
    FOREIGN = "Foreign"
    MILITARY = "Military"

    @property
    def is_toll_free(self) -> bool:
        """Check if the vehicle type is toll-free."""
        toll_free_types = {
            VehicleType.MOTORBIKE,
            VehicleType.TRACTOR,
            VehicleType.EMERGENCY,
            VehicleType.DIPLOMAT,
            VehicleType.FOREIGN,
            VehicleType.MILITARY,
        }
        return self in toll_free_types

class Vehicle:
    def __init__(self, vehicle_type: str):
        self._type = VehicleType(vehicle_type)

    def get_type(self) -> VehicleType:
        return self._type
    
    def is_toll_free(self) -> bool:
        return self._type.is_toll_free