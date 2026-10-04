import { Inject, Injectable } from '@nestjs/common';
import { isTollFreeDate } from '../utils/calendar/swedish-calendar.js';
import { toLocalDateTime } from '../utils/calendar/stockholm-time.js';
import { tollFeeConfig } from './config/toll-fee.config.js';
import { TOLL_FREE_VEHICLE_TYPES } from './types/toll.types.js';
import type {
  Passage,
  TollFeeConfig,
  VehicleType,
} from './types/toll.types.js';

const CHARGE_WINDOW_MS = 60 * 60 * 1000;

type FeeLevel = 'lowFee' | 'mediumFee' | 'highFee';

const FEE_SCHEDULE: ReadonlyArray<{
  from: number;
  until: number;
  level: FeeLevel;
}> = [
  { from: hm(6, 0), until: hm(6, 30), level: 'lowFee' },
  { from: hm(6, 30), until: hm(7, 0), level: 'mediumFee' },
  { from: hm(7, 0), until: hm(8, 0), level: 'highFee' },
  { from: hm(8, 0), until: hm(8, 30), level: 'mediumFee' },
  { from: hm(8, 30), until: hm(15, 0), level: 'lowFee' },
  { from: hm(15, 0), until: hm(15, 30), level: 'mediumFee' },
  { from: hm(15, 30), until: hm(17, 0), level: 'highFee' },
  { from: hm(17, 0), until: hm(18, 0), level: 'mediumFee' },
  { from: hm(18, 0), until: hm(18, 30), level: 'lowFee' },
];

function hm(hour: number, minute: number): number {
  return hour * 60 + minute;
}

@Injectable()
export class TollFeeCalculator {
  constructor(
    @Inject(tollFeeConfig.KEY) private readonly config: TollFeeConfig,
  ) {}

  getPassageFee(vehicleType: VehicleType, passedAt: Date): number {
    if (TOLL_FREE_VEHICLE_TYPES.has(vehicleType)) return 0;

    const local = toLocalDateTime(passedAt);
    if (isTollFreeDate(local.date)) return 0;

    const minuteOfDay = hm(local.hour, local.minute);
    const slot = FEE_SCHEDULE.find(
      (s) => minuteOfDay >= s.from && minuteOfDay < s.until,
    );
    return slot ? this.config[slot.level] : 0;
  }

  getDailyFee(passages: readonly Passage[]): number {
    const chargeable = passages
      .map((p) => ({
        time: p.passedAt.getTime(),
        fee: this.getPassageFee(p.vehicleType, p.passedAt),
      }))
      .filter((p) => p.fee > 0)
      .sort((a, b) => a.time - b.time);

    let total = 0;
    let windowStart = Number.NEGATIVE_INFINITY;
    let windowFee = 0;
    for (const { time, fee } of chargeable) {
      if (time - windowStart < CHARGE_WINDOW_MS) {
        windowFee = Math.max(windowFee, fee);
      } else {
        total += windowFee;
        windowStart = time;
        windowFee = fee;
      }
    }
    total += windowFee;

    return Math.min(total, this.config.maxDailyFee);
  }
}
