import { Injectable } from '@nestjs/common';
import { Vehicle } from '../entities/vehicle.entity';
//import { VehicleType } from '../enums/vehicle.enum';
import { checkForSameHour, tollFeeForTime } from '../utils/toll-fee-time.util';
import { isTollFreeDate } from '../utils/toll-free-dates.util';
import { isTollFreeVehicle } from '../utils/toll-free-vehicle.util';

// main service for calculating toll fee
@Injectable()
export class TollCalculatorService {
  // method for calculating toll fee for a single date.
  getTollFee(date: Date, vehicle: Vehicle): number {
    if (isTollFreeVehicle(vehicle) || isTollFreeDate(date)) {
      return 0;
    }
    return tollFeeForTime(date);
  }
  // method for calculating toll fee for multiple dates
  getTotalTollFee(dateTimes: Date[], vehicle: Vehicle): number {
    if (isTollFreeVehicle(vehicle)) return 0;

    dateTimes = checkForSameHour(dateTimes);
    const totalFee = dateTimes.reduce((sum, date) => {
      if (isTollFreeDate(date)) return sum;
      return sum + this.getTollFee(date, vehicle);
    }, 0);

    return Math.min(totalFee, 60);
  }
}
