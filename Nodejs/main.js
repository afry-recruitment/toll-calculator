import { calculateTollFee } from './services/calculateTollFeeServices.js';
import Car from './models/vehicles/Car.js';

const fee = calculateTollFee(
  new Car(),
  new Date('2013-01-04T16:12:00'),
  new Date('2013-01-04T17:19:00'),
  new Date('2013-01-04T17:09:00')
);
console.log(fee);
