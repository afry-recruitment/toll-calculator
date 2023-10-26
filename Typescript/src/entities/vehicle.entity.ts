import { VehicleType } from '../enums/vehicle.enum';

// This is the entity that represents a vehicle.
export class Vehicle {
  type: VehicleType;

  constructor(type: VehicleType) {
    this.type = type;
  }
}
