// Public barrel — re-exports everything intended for consumers.
//
// Internal modules are not re-exported here. Keeping the surface small
// makes it cheaper to evolve the implementation without breaking callers.

export type { Vehicle, VehicleType } from "./vehicle.js";
export type { FeeBand, FeeSchedule } from "./fee-schedule.js";
export type { HolidayCalendar } from "./calendar/index.js";
export { combineCalendars } from "./calendar/combine.js";
export type {
  TollCalculatorOptions,
  TollCalculatorPreset,
  TollCalculator,
} from "./calculator.js";
export { createTollCalculator } from "./calculator.js";
export {
  REFERENCE_PRESET,
  REFERENCE_FEE_SCHEDULE,
} from "./presets/reference.js";
