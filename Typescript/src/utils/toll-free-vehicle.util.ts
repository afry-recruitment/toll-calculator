import { Vehicle } from '../entities/vehicle.entity';
import { VehicleType } from '../enums/vehicle.enum';

// The vehicles that are toll free
const tollFreeVehicles = [
  VehicleType.Motorbike,
  VehicleType.Tractor,
  VehicleType.Emergency,
  VehicleType.Diplomat,
  VehicleType.Foreign,
  VehicleType.Military,
];

export const isTollFreeVehicle = (vehicle: Vehicle): boolean => {
  return tollFreeVehicles.includes(vehicle.type);
};
