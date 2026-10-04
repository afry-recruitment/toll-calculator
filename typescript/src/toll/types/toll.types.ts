import type { TollEntry } from '../toll-entry.schema.js';

export enum VehicleType {
  Car = 'Car',
  Motorbike = 'Motorbike',
  Tractor = 'Tractor',
  Emergency = 'Emergency',
  Diplomat = 'Diplomat',
  Foreign = 'Foreign',
  Military = 'Military',
}

export const TOLL_FREE_VEHICLE_TYPES: ReadonlySet<VehicleType> = new Set([
  VehicleType.Motorbike,
  VehicleType.Tractor,
  VehicleType.Emergency,
  VehicleType.Diplomat,
  VehicleType.Foreign,
  VehicleType.Military,
]);

export interface TollFeeConfig {
  lowFee: number;
  mediumFee: number;
  highFee: number;
  maxDailyFee: number;
}

export interface LocalDateTime {
  date: string;
  hour: number;
  minute: number;
}

export interface Passage {
  vehicleType: VehicleType;
  passedAt: Date;
}

export interface DailyTotal {
  vehicleNumber: string;
  registrationCountry: string;
  date: string;
  totalFee: number;
  passages: Array<{ passage: TollEntry; fee: number }>;
}

export interface TollTransactionReceipt {
  transactionId: string;
  reference: string;
  status: 'CREATED' | 'UPDATED';
}
