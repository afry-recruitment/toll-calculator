from abc import ABC, abstractmethod


class Vehicle(ABC):
    @abstractmethod
    def get_vehicle_type(self) -> str:
        pass
