import { freeTollVehicles } from '../config/freeTollVehicles.js';
export default class Vehicles {
  constructor() {}
  getType() {
    return this.constructor.name;
  }
  isTollFree() {
    return freeTollVehicles.includes(this.getType());
  }
}
