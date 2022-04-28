import { calculateTollFee } from './services/calculateTollFeeServices.js';
import Car from './models/vehicles/Car.js';

const fee = calculateTollFee(
  new Car(),
  new Date('2013-01-03T15:10:00Z'), // 13
  new Date('2013-01-03T15:20:00Z'), // 13
  new Date('2013-01-09T15:10:00Z'), // 13 +
  new Date('2013-01-03T15:40:00Z'), // 18 +

  new Date('2013-01-01T15:10:00Z'), // 0 Holiday
  new Date('2013-01-05T15:20:00Z'), // 0 Weekend
  new Date('2013-01-05T15:10:00Z'), // 0 Weekend
  new Date('2013-01-05T15:40:00Z'), // 0 Weekend

  new Date('2013-01-04T15:10:00Z'), // 13
  new Date('2013-01-04T15:20:00Z'), // 13
  new Date('2013-01-04T15:10:00Z'), // 13
  new Date('2013-01-04T15:40:00Z') // 18 +
);
console.log(fee); // must be 49
