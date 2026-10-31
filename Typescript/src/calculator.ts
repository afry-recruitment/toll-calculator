import type { VehicleType } from "./types";
import { isTollFreeVehicle } from "./vehicle";
import { isTollFreeDate } from "./calendar";
import { calculatePassageFees } from "./chargingRule";


const MAX_DAILY_FEE = 60;

export function calculateDailyTollFee(vehicleType: VehicleType, passages: readonly Date[]): number {
    const firstPassage = passages[0];

    if (!firstPassage) {
        return 0;
    }
    
    validatePassages(passages, firstPassage);

    if (isTollFreeVehicle(vehicleType)) {
        return 0;
    }

    if (isTollFreeDate(firstPassage)) {
        return 0;
    }

    const totalFee = calculatePassageFees(passages);

    return Math.min(totalFee, MAX_DAILY_FEE);
}

function validatePassages(passages: readonly Date[], firstPassage: Date): void {
    for (const passage of passages) {
        if (Number.isNaN(passage.getTime())) {
        throw new RangeError("Passages must contain valid dates");
        }

        const sameDay =
        passage.getFullYear() === firstPassage.getFullYear() &&
        passage.getMonth() === firstPassage.getMonth() &&
        passage.getDate() === firstPassage.getDate();

        if (!sameDay) {
        throw new RangeError("All passages must be on the same day");
        }
    }
}