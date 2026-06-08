import org.junit.jupiter.api.Nested;
import org.junit.jupiter.api.Test;

import java.time.LocalDateTime;

import static org.junit.jupiter.api.Assertions.assertEquals;

class TollCalculatorTest {

    private final TollCalculator calculator = new TollCalculator();
    private final Car car = new Car();

    @Nested
    class Weekends {

        @Test
        void saturday_isFree() {
            // 2019-03-16 Saturday 07:00
            assertEquals(0, calculator.getTollFee(LocalDateTime.of(2019, 3, 16, 7, 0), car));
        }

        @Test
        void sunday_isFree() {
            // 2024-11-17 Sunday 07:00
            assertEquals(0, calculator.getTollFee(LocalDateTime.of(2024, 11, 17, 7, 0), car));
        }
    }

    @Nested
    class PublicHolidays {

        @Test
        void newYearsDay_isFree() {
            // 2022-01-01 Saturday 07:00 — New Year's Day
            assertEquals(0, calculator.getTollFee(LocalDateTime.of(2022, 1, 1, 7, 0), car));
        }

        @Test
        void newYearsDay_isFree_inFuture() {
            // 2026-01-01 Thursday 07:00 — New Year's Day
            assertEquals(0, calculator.getTollFee(LocalDateTime.of(2026, 1, 1, 7, 0), car));
        }

        @Test
        void christmasDay_isFree() {
            // 2021-12-25 Saturday 07:00 — Christmas Day
            assertEquals(0, calculator.getTollFee(LocalDateTime.of(2021, 12, 25, 7, 0), car));
        }

        @Test
        void christmasDay_isFree_inFuture() {
            // 2025-12-25 Thursday 07:00 — Christmas Day
            assertEquals(0, calculator.getTollFee(LocalDateTime.of(2025, 12, 25, 7, 0), car));
        }

        @Test
        void labourDay_isFree() {
            // 2023-05-01 Monday 07:00 — Labour Day
            assertEquals(0, calculator.getTollFee(LocalDateTime.of(2023, 5, 1, 7, 0), car));
        }

        @Test
        void nationalDay_isFree() {
            // 2024-06-06 Thursday 07:00 — Swedish National Day
            assertEquals(0, calculator.getTollFee(LocalDateTime.of(2024, 6, 6, 7, 0), car));
        }

        @Test
        void easterMonday_isFree() {
            // 2024-04-01 Monday 07:00 — Easter Monday
            assertEquals(0, calculator.getTollFee(LocalDateTime.of(2024, 4, 1, 7, 0), car));
        }

        @Test
        void easterMonday_isFree_differentYear() {
            // 2025-04-21 Monday 07:00 — Easter Monday
            assertEquals(0, calculator.getTollFee(LocalDateTime.of(2025, 4, 21, 7, 0), car));
        }
    }

    @Nested
    class HolidayEves {

        @Test
        void christmasEve_isFree() {
            // 2024-12-24 Tuesday 07:00 — Christmas Eve
            assertEquals(0, calculator.getTollFee(LocalDateTime.of(2024, 12, 24, 7, 0), car));
        }

        @Test
        void newYearsEve_isFree() {
            // 2024-12-31 Tuesday 07:00 — New Year's Eve
            assertEquals(0, calculator.getTollFee(LocalDateTime.of(2024, 12, 31, 7, 0), car));
        }

        @Test
        void midsummerEve_isFree() {
            // 2024-06-21 Friday 07:00 — Midsummer Eve
            assertEquals(0, calculator.getTollFee(LocalDateTime.of(2024, 6, 21, 7, 0), car));
        }

        @Test
        void midsummerEve_isFree_differentYear() {
            // 2025-06-20 Friday 07:00 — Midsummer Eve
            assertEquals(0, calculator.getTollFee(LocalDateTime.of(2025, 6, 20, 7, 0), car));
        }
    }

    @Nested
    class TollFreeVehicles {

        @Test
        void motorbike_isFree() {
            // 2024-02-05 Monday 07:00 — rush hour, but Motorbike is toll-free
            assertEquals(0, calculator.getTollFee(LocalDateTime.of(2024, 2, 5, 7, 0), new Motorbike()));
        }

        @Test
        void tractor_isFree() {
            // 2024-02-05 Monday 07:00 — rush hour, but Tractor is toll-free
            assertEquals(0, calculator.getTollFee(LocalDateTime.of(2024, 2, 5, 7, 0), new Tractor()));
        }

        @Test
        void emergency_isFree() {
            // 2024-02-05 Monday 07:00 — rush hour, but Emergency is toll-free
            assertEquals(0, calculator.getTollFee(LocalDateTime.of(2024, 2, 5, 7, 0), new Emergency()));
        }

        @Test
        void diplomat_isFree() {
            // 2024-02-05 Monday 07:00 — rush hour, but Diplomat is toll-free
            assertEquals(0, calculator.getTollFee(LocalDateTime.of(2024, 2, 5, 7, 0), new Diplomat()));
        }

        @Test
        void foreign_isFree() {
            // 2024-02-05 Monday 07:00 — rush hour, but Foreign is toll-free
            assertEquals(0, calculator.getTollFee(LocalDateTime.of(2024, 2, 5, 7, 0), new Foreign()));
        }

        @Test
        void military_isFree() {
            // 2024-02-05 Monday 07:00 — rush hour, but Military is toll-free
            assertEquals(0, calculator.getTollFee(LocalDateTime.of(2024, 2, 5, 7, 0), new Military()));
        }

        @Test
        void car_isNotFree() {
            // 2024-02-05 Monday 07:00 — Car is not toll-free
            assertEquals(18, calculator.getTollFee(LocalDateTime.of(2024, 2, 5, 7, 0), new Car()));
        }
    }

    @Nested
    class FeeSchedule {

        // Full schedule: 0 SEK outside toll hours, 8/13/18 SEK within

        @Test
        void before0600_charges0() {
            // 2024-02-05 Monday 05:59 — before toll hours begin
            assertEquals(0, calculator.getTollFee(LocalDateTime.of(2024, 2, 5, 5, 59), car));
        }

        @Test
        void at0600_charges8() {
            // 2024-02-05 Monday 06:00 — start of toll hours
            assertEquals(8, calculator.getTollFee(LocalDateTime.of(2024, 2, 5, 6, 0), car));
        }

        @Test
        void at0629_charges8() {
            // 2024-02-05 Monday 06:29 — end of first 8 SEK slot
            assertEquals(8, calculator.getTollFee(LocalDateTime.of(2024, 2, 5, 6, 29), car));
        }

        @Test
        void at0630_charges13() {
            // 2024-02-05 Monday 06:30 — start of pre-morning-rush 13 SEK slot
            assertEquals(13, calculator.getTollFee(LocalDateTime.of(2024, 2, 5, 6, 30), car));
        }

        @Test
        void at0900_charges8() {
            // 2024-02-05 Monday 09:00 — mid-day off-peak
            assertEquals(8, calculator.getTollFee(LocalDateTime.of(2024, 2, 5, 9, 0), car));
        }

        @Test
        void at1015_charges8() {
            // 2024-02-05 Monday 10:15 — mid-day off-peak
            assertEquals(8, calculator.getTollFee(LocalDateTime.of(2024, 2, 5, 10, 15), car));
        }

        @Test
        void at1300_charges8() {
            // 2024-02-05 Monday 13:00 — mid-day off-peak
            assertEquals(8, calculator.getTollFee(LocalDateTime.of(2024, 2, 5, 13, 0), car));
        }

        @Test
        void at1429_charges8() {
            // 2024-02-05 Monday 14:29 — end of mid-day off-peak
            assertEquals(8, calculator.getTollFee(LocalDateTime.of(2024, 2, 5, 14, 29), car));
        }

        @Test
        void at1759_charges13() {
            // 2024-02-05 Monday 17:59 — end of post-afternoon-rush 13 SEK slot
            assertEquals(13, calculator.getTollFee(LocalDateTime.of(2024, 2, 5, 17, 59), car));
        }

        @Test
        void at1800_charges8() {
            // 2024-02-05 Monday 18:00 — start of evening 8 SEK slot
            assertEquals(8, calculator.getTollFee(LocalDateTime.of(2024, 2, 5, 18, 0), car));
        }

        @Test
        void at1829_charges8() {
            // 2024-02-05 Monday 18:29 — last charged minute of the day
            assertEquals(8, calculator.getTollFee(LocalDateTime.of(2024, 2, 5, 18, 29), car));
        }

        @Test
        void at1830_charges0() {
            // 2024-02-05 Monday 18:30 — toll hours have ended
            assertEquals(0, calculator.getTollFee(LocalDateTime.of(2024, 2, 5, 18, 30), car));
        }
    }

    @Nested
    class MaxDailyFee {

        // The total fee for one day is capped at 60 SEK regardless of
        // how many passes are made

        @Test
        void totalFee_isCappedAt60() {
            // 7 separate windows totalling 71 SEK before cap:
            // 06:30(13) + 07:31(18) + 08:32(8) + 09:33(8) + 10:34(8) + 11:35(8) + 12:36(8) = 71
            assertEquals(60, calculator.getTollFee(car,
                LocalDateTime.of(2024, 2, 5,  6, 30),
                LocalDateTime.of(2024, 2, 5,  7, 31),
                LocalDateTime.of(2024, 2, 5,  8, 32),
                LocalDateTime.of(2024, 2, 5,  9, 33),
                LocalDateTime.of(2024, 2, 5, 10, 34),
                LocalDateTime.of(2024, 2, 5, 11, 35),
                LocalDateTime.of(2024, 2, 5, 12, 36)));
        }

        @Test
        void totalFee_isNotCapped_whenUnder60() {
            // 07:00 (18) + 15:30 (18) = 36 SEK — no cap applied
            assertEquals(36, calculator.getTollFee(car,
                LocalDateTime.of(2024, 2, 5,  7,  0),
                LocalDateTime.of(2024, 2, 5, 15, 30)));
        }
    }

    @Nested
    class SingleChargePerHour {

        // A vehicle is charged only once per 60-minute window.
        // Multiple passes within the same window result in the highest fee only.
        // A new window starts when the next pass is more than 60 minutes after
        // the first pass of the current window.

        @Test
        void passesWithinSameWindow_chargeOnce() {
            // 07:00 (18) and 07:30 (18) are 30 min apart — charge once: 18
            assertEquals(18, calculator.getTollFee(car,
                LocalDateTime.of(2024, 2, 5, 7,  0),
                LocalDateTime.of(2024, 2, 5, 7, 30)));
        }

        @Test
        void closePasses_afterGap_groupedIntoNewWindow() {
            // 07:00 (18) → window 1
            // 08:10 (13) and 08:30 (8) are 20 min apart → window 2, charge max: 13
            // Total: 18 + 13 = 31
            assertEquals(31, calculator.getTollFee(car,
                LocalDateTime.of(2024, 2, 5, 7,  0),
                LocalDateTime.of(2024, 2, 5, 8, 10),
                LocalDateTime.of(2024, 2, 5, 8, 30)));
        }

        @Test
        void secondWindowWithTwoPasses_chargesHighestOnce() {
            // 06:00 (8) → window 1
            // 07:05 (18) and 07:45 (18) are 40 min apart → window 2, charge max: 18
            // Total: 8 + 18 = 26
            assertEquals(26, calculator.getTollFee(car,
                LocalDateTime.of(2024, 2, 5, 6,  0),
                LocalDateTime.of(2024, 2, 5, 7,  5),
                LocalDateTime.of(2024, 2, 5, 7, 45)));
        }
    }

    @Nested
    class RushHour {

        // Morning rush: 07:00–07:59 → 18 SEK

        @Test
        void morningRushStart_charges18() {
            // 2024-02-05 Monday 07:00
            assertEquals(18, calculator.getTollFee(LocalDateTime.of(2024, 2, 5, 7, 0), car));
        }

        @Test
        void morningRushEnd_charges18() {
            // 2024-02-05 Monday 07:59
            assertEquals(18, calculator.getTollFee(LocalDateTime.of(2024, 2, 5, 7, 59), car));
        }

        @Test
        void justBeforeMorningRush_charges13() {
            // 2024-02-05 Monday 06:59 — not yet rush hour
            assertEquals(13, calculator.getTollFee(LocalDateTime.of(2024, 2, 5, 6, 59), car));
        }

        @Test
        void justAfterMorningRush_charges13() {
            // 2024-02-05 Monday 08:00 — rush hour has ended
            assertEquals(13, calculator.getTollFee(LocalDateTime.of(2024, 2, 5, 8, 0), car));
        }

        // Afternoon rush: 15:30–16:59 → 18 SEK

        @Test
        void afternoonRushStart_charges18() {
            // 2024-02-05 Monday 15:30
            assertEquals(18, calculator.getTollFee(LocalDateTime.of(2024, 2, 5, 15, 30), car));
        }

        @Test
        void afternoonRushEnd_charges18() {
            // 2024-02-05 Monday 16:59
            assertEquals(18, calculator.getTollFee(LocalDateTime.of(2024, 2, 5, 16, 59), car));
        }

        @Test
        void justBeforeAfternoonRush_charges13() {
            // 2024-02-05 Monday 15:29 — not yet rush hour
            assertEquals(13, calculator.getTollFee(LocalDateTime.of(2024, 2, 5, 15, 29), car));
        }

        @Test
        void justAfterAfternoonRush_charges13() {
            // 2024-02-05 Monday 17:00 — rush hour has ended
            assertEquals(13, calculator.getTollFee(LocalDateTime.of(2024, 2, 5, 17, 0), car));
        }
    }

    @Nested
    class ChargeableWeekdays {

        @Test
        void regularMonday_isCharged() {
            // 2024-02-05 Monday 07:00
            assertEquals(18, calculator.getTollFee(LocalDateTime.of(2024, 2, 5, 7, 0), car));
        }

        @Test
        void regularFriday_isCharged() {
            // 2025-03-07 Friday 07:00
            assertEquals(18, calculator.getTollFee(LocalDateTime.of(2025, 3, 7, 7, 0), car));
        }
    }
}
