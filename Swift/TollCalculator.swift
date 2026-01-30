import Foundation

final class TollCalculator {
    func getTollFee(vehicle: Vehicle, dates: [Date]) -> Int {
        guard !dates.isEmpty else { return 0 }
        let sortedDates = dates.sorted()

        let calendar = Calendar.current
        let firstDay = calendar.startOfDay(for: sortedDates[0])
        for date in sortedDates {
            if calendar.startOfDay(for: date) != firstDay {
                preconditionFailure("All dates must be on the same day")
            }
        }

        var totalFee = 0
        var intervalStart = sortedDates[0]
        var intervalFee = getTollFee(date: intervalStart, vehicle: vehicle)
        totalFee += intervalFee

        for date in sortedDates.dropFirst() {
            let minutes = minutesBetween(from: intervalStart, to: date)
            let nextFee = getTollFee(date: date, vehicle: vehicle)

            if minutes <= 60 {
                if nextFee > intervalFee {
                    totalFee += (nextFee - intervalFee)
                    intervalFee = nextFee
                }
            } else {
                intervalStart = date
                intervalFee = nextFee
                totalFee += nextFee
            }
        }

        return min(totalFee, 60)
    }

    func getTollFee(date: Date, vehicle: Vehicle) -> Int {
        if isTollFreeDate(date) || isTollFreeVehicle(vehicle) { return 0 }

        let calendar = Calendar.current
        let hour = calendar.component(.hour, from: date)
        let minute = calendar.component(.minute, from: date)

        if hour == 6 && minute <= 29 { return 8 }
        if hour == 6 { return 13 }
        if hour == 7 { return 18 }
        if hour == 8 && minute <= 29 { return 13 }
        if hour >= 8 && hour <= 14 { return 8 }
        if hour == 15 && minute <= 29 { return 13 }
        if (hour == 15 && minute >= 30) || hour == 16 { return 18 }
        if hour == 17 { return 13 }
        if hour == 18 && minute <= 29 { return 8 }
        return 0
    }

    private func isTollFreeVehicle(_ vehicle: Vehicle) -> Bool {
        switch vehicle.vehicleType {
        case "Motorbike", "Tractor", "Emergency", "Diplomat", "Foreign", "Military":
            return true
        default:
            return false
        }
    }

    private func isTollFreeDate(_ date: Date) -> Bool {
        let calendar = Calendar.current
        let weekday = calendar.component(.weekday, from: date)
        if weekday == 1 || weekday == 7 { return true }

        let year = calendar.component(.year, from: date)
        let localDate = calendar.startOfDay(for: date)
        return getTollFreeDates(year: year).contains(localDate)
    }

    private func getTollFreeDates(year: Int) -> Set<Date> {
        var dates = Set<Date>()
        let calendar = Calendar.current

        func add(_ month: Int, _ day: Int) {
            let components = DateComponents(year: year, month: month, day: day)
            if let date = calendar.date(from: components) {
                dates.insert(calendar.startOfDay(for: date))
            }
        }

        add(1, 1)
        add(1, 6)
        add(5, 1)
        add(6, 6)
        add(12, 24)
        add(12, 25)
        add(12, 26)
        add(12, 31)

        let easterSunday = getEasterSunday(year: year)
        dates.insert(calendar.startOfDay(for: calendar.date(byAdding: .day, value: -2, to: easterSunday)!))
        dates.insert(calendar.startOfDay(for: easterSunday))
        dates.insert(calendar.startOfDay(for: calendar.date(byAdding: .day, value: 1, to: easterSunday)!))
        dates.insert(calendar.startOfDay(for: calendar.date(byAdding: .day, value: 39, to: easterSunday)!))
        dates.insert(calendar.startOfDay(for: calendar.date(byAdding: .day, value: 49, to: easterSunday)!))

        let midsummerEve = getMidsummerEve(year: year)
        dates.insert(calendar.startOfDay(for: midsummerEve))
        dates.insert(calendar.startOfDay(for: calendar.date(byAdding: .day, value: 1, to: midsummerEve)!))

        if let julyStart = calendar.date(from: DateComponents(year: year, month: 7, day: 1)),
           let range = calendar.range(of: .day, in: .month, for: julyStart) {
            for day in range {
                add(7, day)
            }
        }

        return dates
    }

    private func getMidsummerEve(year: Int) -> Date {
        let calendar = Calendar.current
        let june19 = calendar.date(from: DateComponents(year: year, month: 6, day: 19))!
        let weekday = calendar.component(.weekday, from: june19) // 1=Sun ... 7=Sat
        let friday = 6
        let daysUntilFriday = (friday - weekday + 7) % 7
        return calendar.date(byAdding: .day, value: daysUntilFriday, to: june19)!
    }

    private func getEasterSunday(year: Int) -> Date {
        let a = year % 19
        let b = year / 100
        let c = year % 100
        let d = b / 4
        let e = b % 4
        let f = (b + 8) / 25
        let g = (b - f + 1) / 3
        let h = (19 * a + b - d - g + 15) % 30
        let i = c / 4
        let k = c % 4
        let l = (32 + 2 * e + 2 * i - h - k) % 7
        let m = (a + 11 * h + 22 * l) / 451
        let month = (h + l - 7 * m + 114) / 31
        let day = ((h + l - 7 * m + 114) % 31) + 1
        let calendar = Calendar.current
        return calendar.date(from: DateComponents(year: year, month: month, day: day))!
    }

    private func minutesBetween(from start: Date, to end: Date) -> Int {
        let delta = end.timeIntervalSince(start)
        return Int(ceil(delta / 60.0))
    }
}
