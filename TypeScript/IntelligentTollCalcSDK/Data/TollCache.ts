import { DefaultTollConfig } from "../Core/tollConfig";

// Tracks tolls charged per vehicle per day so it can enforce:
//   - a vehicle is charged at most once per clock hour (the highest fee applies)
//   - a maximum total fee per vehicle per day
class TollCache {
    // vehicle+day key -> (hour of day -> highest fee charged that hour)
    private readonly dailyHourlyFees: Map<string, Map<number, number>> = new Map();

    constructor(private readonly maximumDailyToll: number = DefaultTollConfig.maximumDailyToll) {}

    // Unique per vehicle per calendar day (date-aware, so the same hour on
    // different days does not collide).
    private generateKey(vehicleID: string, date: Date): string {
        return `${vehicleID}_${date.getFullYear()}-${date.getMonth()}-${date.getDate()}`;
    }

    // Daily total so far, capped at the configured daily maximum.
    private cappedDailyTotal(hourlyFees: Map<number, number>): number {
        let total = 0;
        for (const fee of hourlyFees.values()) {
            total += fee;
        }
        return Math.min(total, this.maximumDailyToll);
    }

    // Register a passage and return the amount actually charged for it.
    // Returns 0 when an equal or higher fee was already charged this hour, or
    // when the daily maximum has already been reached.
    public registerCharge(vehicleID: string, date: Date, fee: number): number {
        const key = this.generateKey(vehicleID, date);
        let hourlyFees = this.dailyHourlyFees.get(key);
        if (!hourlyFees) {
            hourlyFees = new Map<number, number>();
            this.dailyHourlyFees.set(key, hourlyFees);
        }

        const hour = date.getHours();
        const previousHourFee = hourlyFees.get(hour) ?? 0;
        // Highest fee in the hour applies: a lower (or equal) passage adds nothing.
        if (fee <= previousHourFee) {
            return 0;
        }

        const previousTotal = this.cappedDailyTotal(hourlyFees);
        hourlyFees.set(hour, fee);
        const newTotal = this.cappedDailyTotal(hourlyFees);
        return newTotal - previousTotal;
    }
}

export default TollCache;
