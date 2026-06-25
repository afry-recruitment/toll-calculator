// Lightweight assertion script for the toll accounting rules. No test runner or
// network/LLM is required — run with: pnpm exec tsx IntelligentTollCalcTests/tollModel.test.ts
// (uses ModelProvider.None so no model client is involved).

import assert from "node:assert";
import { TollModel } from "../IntelligentTollCalcSDK/Core/TollModel";
import { ModelProvider } from "../IntelligentTollCalcSDK/Core/tollConfig";

// Weekday at a given hour:minute. 2026-06-25 is a Thursday.
const weekday = (hour: number, minute = 0) => new Date(2026, 5, 25, hour, minute);

async function run(): Promise<void> {
    // Free on weekends (2026-06-27 is a Saturday).
    {
        const model = new TollModel();
        const toll = await model.calculateToll(new Date(2026, 5, 27, 8, 0), "W1", "Volvo XC90", ModelProvider.None);
        assert.strictEqual(toll, 0, "weekend should be free");
    }

    // Free on a holiday (New Year's Day), even on a weekday.
    {
        const model = new TollModel();
        const toll = await model.calculateToll(new Date(2026, 0, 1, 8, 0), "H1", "Volvo XC90", ModelProvider.None);
        assert.strictEqual(toll, 0, "holiday should be free");
    }

    // Free outside the chargeable hours (e.g. 03:00).
    {
        const model = new TollModel();
        const toll = await model.calculateToll(weekday(3), "N1", "Volvo XC90", ModelProvider.None);
        assert.strictEqual(toll, 0, "night should be free");
    }

    // Highest fee in the hour applies: a later, higher passage in the same clock
    // hour charges only the difference; a lower one charges nothing.
    {
        const model = new TollModel();
        const first = await model.calculateToll(weekday(7, 0), "V1", "Volvo XC90", ModelProvider.None);   // 18
        const lower = await model.calculateToll(weekday(8, 5), "V1", "Volvo XC90", ModelProvider.None);   // hour 8 -> 13 (new hour)
        const sameHourLower = await model.calculateToll(weekday(8, 20), "V1", "Volvo XC90", ModelProvider.None); // hour 8 -> 13, no extra
        assert.strictEqual(first, 18, "07:00 should charge 18");
        assert.strictEqual(lower, 13, "08:05 (new hour) should charge 13");
        assert.strictEqual(sameHourLower, 0, "second passage in hour 8 (not higher) charges 0");
    }

    // Within the same hour, a higher later passage tops up to the highest fee.
    {
        const model = new TollModel();
        const a = await model.calculateToll(weekday(6, 5), "V2", "Volvo XC90", ModelProvider.None);  // 06:00-06:30 -> 8
        const b = await model.calculateToll(weekday(6, 40), "V2", "Volvo XC90", ModelProvider.None); // 06:30-07:00 -> 13, same hour 6
        assert.strictEqual(a, 8, "06:05 should charge 8");
        assert.strictEqual(b, 5, "06:40 should top up to 13 (charge the 5 difference)");
    }

    // Daily maximum of 60 SEK is enforced across the day.
    {
        const model = new TollModel();
        let total = 0;
        // Six distinct rush hours at 18 SEK each = 108 SEK uncapped.
        for (const hour of [6, 7, 8, 15, 16, 17]) {
            total += await model.calculateToll(weekday(hour, 0), "V3", "Volvo XC90", ModelProvider.None);
        }
        assert.strictEqual(total, 60, "daily total should be capped at 60");
    }

    // Date-aware key: the same hour on different days is charged independently.
    {
        const model = new TollModel();
        const day1 = await model.calculateToll(new Date(2026, 5, 25, 7, 0), "V4", "Volvo XC90", ModelProvider.None);
        const day2 = await model.calculateToll(new Date(2026, 5, 26, 7, 0), "V4", "Volvo XC90", ModelProvider.None);
        assert.strictEqual(day1, 18, "day 1 07:00 should charge 18");
        assert.strictEqual(day2, 18, "day 2 07:00 should charge 18 (no cross-day collision)");
    }

    console.log("All toll accounting assertions passed.");
}

run().catch(err => {
    console.error(err);
    process.exit(1);
});
