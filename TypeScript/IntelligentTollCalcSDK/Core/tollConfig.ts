// Configuration and domain types for the Intelligent Toll Calculation SDK.

// A fee that applies to a half-open time interval [startMinute, endMinute) of the
// day, expressed in minutes since midnight. Times not covered by any interval are
// toll-free (e.g. nights).
export type FeeInterval = {
    startMinute: number;
    endMinute: number;
    fee: number;
}

// Result of assessing a single passage: the toll charged and the vehicle type
// the passage was classified as (Unknown when no classification was performed,
// e.g. on weekends/holidays/off-hours or without a model provider).
export type TollAssessment = {
    toll: number;
    vehicleType: VehicleType;
}

export type TollConfig = {
    // Maximum total fee charged to a single vehicle in one day.
    maximumDailyToll: number;
    // Explicit time-of-day fee schedule. Highest fees fall on the rush-hour windows.
    feeSchedule: FeeInterval[];
    // Vehicle types that are exempt from tolls.
    tollFreeVehicleTypes: VehicleType[];
}

// General vehicle categories a model client can classify a vehicle model into.
export enum VehicleType {
    Car = "Car",
    Motorcycle = "Motorcycle",
    Bus = "Bus",
    Truck = "Truck",
    Emergency = "Emergency",
    Diplomat = "Diplomat",
    Military = "Military",
    Tractor = "Tractor",
    Unknown = "Unknown"
}

// All known vehicle types, used to constrain model output to the enum.
export const AllVehicleTypes: VehicleType[] = Object.values(VehicleType);

// Coerce an arbitrary string (e.g. model output) into a known VehicleType,
// falling back to Unknown (which is never toll-free) when it doesn't match.
export function toVehicleType(value: string | undefined | null): VehicleType {
    if (!value) {
        return VehicleType.Unknown;
    }
    const normalized = value.toLowerCase().trim();
    return AllVehicleTypes.find(t => t.toLowerCase() === normalized) ?? VehicleType.Unknown;
}

// helper to express schedule boundaries as minutes since midnight
const at = (hours: number, minutes: number): number => hours * 60 + minutes;

export const DefaultTollConfig: TollConfig = {
    maximumDailyToll: 60,
    // Fees range between 8 and 18 SEK; rush-hour windows (07:00-08:00 and
    // 15:30-17:00) carry the maximum. Times outside the schedule are free.
    feeSchedule: [
        { startMinute: at(6, 0),  endMinute: at(6, 30),  fee: 8 },
        { startMinute: at(6, 30), endMinute: at(7, 0),   fee: 13 },
        { startMinute: at(7, 0),  endMinute: at(8, 0),   fee: 18 },
        { startMinute: at(8, 0),  endMinute: at(8, 30),  fee: 13 },
        { startMinute: at(8, 30), endMinute: at(15, 0),  fee: 8 },
        { startMinute: at(15, 0), endMinute: at(15, 30), fee: 13 },
        { startMinute: at(15, 30), endMinute: at(17, 0), fee: 18 },
        { startMinute: at(17, 0), endMinute: at(18, 0),  fee: 13 },
        { startMinute: at(18, 0), endMinute: at(18, 30), fee: 8 }
        // 18:30-06:00 is toll-free (no interval).
    ],
    tollFreeVehicleTypes: [
        VehicleType.Motorcycle,
        VehicleType.Bus,
        VehicleType.Emergency,
        VehicleType.Diplomat,
        VehicleType.Military,
        VehicleType.Tractor
    ]
}

// Resolve the fee for a given instant from the schedule. Returns 0 when no
// interval covers the time (toll-free).
export function feeForTime(dateTime: Date, schedule: FeeInterval[] = DefaultTollConfig.feeSchedule): number {
    const minutes = dateTime.getHours() * 60 + dateTime.getMinutes();
    const interval = schedule.find(i => minutes >= i.startMinute && minutes < i.endMinute);
    return interval ? interval.fee : 0;
}

// Toll-free public holidays. The year is a placeholder — only month and day are
// compared, so any vehicle is free on these dates every year.
export const OfficialHolidays: Date[] = [
    new Date(2000, 0, 1),   // New Year's Day
    new Date(2000, 11, 24), // Christmas Eve
    new Date(2000, 11, 25), // Christmas Day
    new Date(2000, 11, 26)  // Boxing Day
];

// Enum for model providers
export enum ModelProvider {
    OpenAI = "OpenAI",
    Anthropic = "Anthropic",
    Ollama = "Ollama",
    None = "None"
}
