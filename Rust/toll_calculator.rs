use chrono::{DateTime, Datelike, Local, NaiveDate, Timelike, Weekday};
use std::collections::HashSet;

use crate::vehicle::Vehicle;

pub struct TollCalculator;

impl TollCalculator {
    pub fn get_toll_fee<V: Vehicle>(&self, vehicle: &V, mut dates: Vec<DateTime<Local>>) -> i32 {
        if dates.is_empty() {
            return 0;
        }

        dates.sort();

        let first_day = dates[0].date_naive();
        for date in &dates {
            if date.date_naive() != first_day {
                panic!("All dates must be on the same day");
            }
        }

        let mut total_fee = 0;
        let mut interval_start = dates[0];
        let mut interval_fee = self.get_toll_fee_at(interval_start, vehicle);
        total_fee += interval_fee;

        for date in dates.into_iter().skip(1) {
            let seconds = (date - interval_start).num_seconds();
            let next_fee = self.get_toll_fee_at(date, vehicle);

            if seconds <= 3600 {
                if next_fee > interval_fee {
                    total_fee += next_fee - interval_fee;
                    interval_fee = next_fee;
                }
            } else {
                interval_start = date;
                interval_fee = next_fee;
                total_fee += next_fee;
            }
        }

        total_fee.min(60)
    }

    pub fn get_toll_fee_at<V: Vehicle>(&self, date: DateTime<Local>, vehicle: &V) -> i32 {
        if self.is_toll_free_date(date) || self.is_toll_free_vehicle(vehicle) {
            return 0;
        }

        let hour = date.hour();
        let minute = date.minute();

        if hour == 6 && minute <= 29 { return 8; }
        if hour == 6 { return 13; }
        if hour == 7 { return 18; }
        if hour == 8 && minute <= 29 { return 13; }
        if (8..=14).contains(&hour) { return 8; }
        if hour == 15 && minute <= 29 { return 13; }
        if (hour == 15 && minute >= 30) || hour == 16 { return 18; }
        if hour == 17 { return 13; }
        if hour == 18 && minute <= 29 { return 8; }
        0
    }

    fn is_toll_free_vehicle<V: Vehicle>(&self, vehicle: &V) -> bool {
        matches!(
            vehicle.vehicle_type(),
            "Motorbike" | "Tractor" | "Emergency" | "Diplomat" | "Foreign" | "Military"
        )
    }

    fn is_toll_free_date(&self, date: DateTime<Local>) -> bool {
        let weekday = date.weekday();
        if weekday == chrono::Weekday::Sat || weekday == chrono::Weekday::Sun {
            return true;
        }

        let year = date.year();
        let local_date = date.date_naive();
        self.get_toll_free_dates(year).contains(&local_date)
    }

    fn get_toll_free_dates(&self, year: i32) -> HashSet<NaiveDate> {
        let mut dates = HashSet::new();

        dates.insert(NaiveDate::from_ymd_opt(year, 1, 1).unwrap());
        dates.insert(NaiveDate::from_ymd_opt(year, 1, 6).unwrap());
        dates.insert(NaiveDate::from_ymd_opt(year, 5, 1).unwrap());
        dates.insert(NaiveDate::from_ymd_opt(year, 6, 6).unwrap());
        dates.insert(NaiveDate::from_ymd_opt(year, 12, 24).unwrap());
        dates.insert(NaiveDate::from_ymd_opt(year, 12, 25).unwrap());
        dates.insert(NaiveDate::from_ymd_opt(year, 12, 26).unwrap());
        dates.insert(NaiveDate::from_ymd_opt(year, 12, 31).unwrap());

        let easter_sunday = self.get_easter_sunday(year);
        dates.insert(easter_sunday.pred_opt().unwrap().pred_opt().unwrap()); // Good Friday
        dates.insert(easter_sunday); // Easter Sunday
        dates.insert(easter_sunday.succ_opt().unwrap()); // Easter Monday
        dates.insert(easter_sunday + chrono::Duration::days(39)); // Ascension Day
        dates.insert(easter_sunday + chrono::Duration::days(49)); // Pentecost Sunday

        let midsummer_eve = self.get_midsummer_eve(year);
        dates.insert(midsummer_eve);
        dates.insert(midsummer_eve.succ_opt().unwrap()); // Midsummer Day

        let july_days = if Self::is_leap_year(year) { 31 } else { 31 };
        for day in 1..=july_days {
            dates.insert(NaiveDate::from_ymd_opt(year, 7, day).unwrap());
        }

        dates
    }

    fn get_midsummer_eve(&self, year: i32) -> NaiveDate {
        let june_19 = NaiveDate::from_ymd_opt(year, 6, 19).unwrap();
        let days_until_friday =
            (Weekday::Fri.number_from_monday() + 7 - june_19.weekday().number_from_monday()) % 7;
        june_19 + chrono::Duration::days(days_until_friday as i64)
    }

    fn get_easter_sunday(&self, year: i32) -> NaiveDate {
        let a = year % 19;
        let b = year / 100;
        let c = year % 100;
        let d = b / 4;
        let e = b % 4;
        let f = (b + 8) / 25;
        let g = (b - f + 1) / 3;
        let h = (19 * a + b - d - g + 15) % 30;
        let i = c / 4;
        let k = c % 4;
        let l = (32 + 2 * e + 2 * i - h - k) % 7;
        let m = (a + 11 * h + 22 * l) / 451;
        let month = (h + l - 7 * m + 114) / 31; // 3=March, 4=April
        let day = ((h + l - 7 * m + 114) % 31) + 1;
        NaiveDate::from_ymd_opt(year, month as u32, day as u32).unwrap()
    }

    fn is_leap_year(year: i32) -> bool {
        (year % 4 == 0 && year % 100 != 0) || (year % 400 == 0)
    }
}
