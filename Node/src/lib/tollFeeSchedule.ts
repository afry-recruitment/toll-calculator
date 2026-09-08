type TollTimeSlot = {
  start: string;
  end: string;
  fee: number;
};
export const tollFees: Record<string, number> = {
  feeFree: 0,
  feeLowest: 8,
  feeMiddle: 13,
  feeHighest: 18,
} as const;
export const MAX_TOTAL_FEE = 60;

export const tollFeeSchedule: TollTimeSlot[] = [
  { start: "00:00", end: "05:59", fee: tollFees.feeFree },
  { start: "06:00", end: "06:29", fee: tollFees.feeLowest },
  { start: "06:30", end: "06:59", fee: tollFees.feeMiddle },
  { start: "07:00", end: "07:59", fee: tollFees.feeHighest },
  { start: "08:00", end: "08:29", fee: tollFees.feeMiddle },
  { start: "08:30", end: "14:59", fee: tollFees.feeLowest },
  { start: "15:00", end: "15:29", fee: tollFees.feeMiddle },
  { start: "15:30", end: "16:59", fee: tollFees.feeHighest },
  { start: "17:00", end: "17:59", fee: tollFees.feeMiddle },
  { start: "18:00", end: "18:29", fee: tollFees.feeLowest },
  { start: "18:30", end: "23:59", fee: tollFees.feeFree },
];
