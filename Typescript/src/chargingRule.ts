import { getTollFeeForTime } from "./tariff";

const CHARGE_WINDOW_MINUTES = 60;

export function calculatePassageFees(passages: readonly Date[]): number {
    const chargeablePassages = passages.filter(passage => getTollFeeForTime(passage) > 0);
    const chargeWindows = groupPassagesIntoChargeWindows(chargeablePassages);

    let totalFee = 0;

    for (const window of chargeWindows) {
        totalFee += getHighestFee(window);
    }

    return totalFee;
}

function groupPassagesIntoChargeWindows(passages: readonly Date[]): Date[][] {
    const sortedPassages = [...passages].sort((a, b) => a.getTime() - b.getTime());

    const [firstPassage, ...remainingPassages] = sortedPassages;

    if (!firstPassage) return [];
    

    const windows: Date[][] = [];
    let currentWindow: Date[] = [firstPassage];
    let windowStart = firstPassage;

    for (const passage of remainingPassages) {
        const minutesSinceWindowStart =
        (passage.getTime() - windowStart.getTime()) / 60_000;

        if (minutesSinceWindowStart <= CHARGE_WINDOW_MINUTES) {
        currentWindow.push(passage);
        continue;
        }

        windows.push(currentWindow);

        currentWindow = [passage];
        windowStart = passage;
    }

    windows.push(currentWindow);

    return windows;
}

function getHighestFee(passages: readonly Date[]): number {
    let highestFee = 0;

    for (const passage of passages) {
        const fee = getTollFeeForTime(passage);

        if (fee > highestFee) {
        highestFee = fee;
        }
    }

    return highestFee;
}