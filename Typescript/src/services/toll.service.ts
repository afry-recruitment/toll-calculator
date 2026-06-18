import Holidays from 'date-holidays'
import {Temporal} from '@js-temporal/polyfill'
import type {TollRatesRepository} from '../repositories/toll-rates.repository.js'
import {TOLL_FREE_VEHICLES} from '../models/vehicle.js'
import type {Vehicle} from '../models/vehicle.js'

const hd = new Holidays('SE')
const TOLL_FREE = new Set<Vehicle>(TOLL_FREE_VEHICLES)

function isJuly(date: Temporal.ZonedDateTime): boolean {
    return date.month === 7
}

function isWeekend(date: Temporal.ZonedDateTime): boolean {
    return date.dayOfWeek >= 6
}

function isTollFree(date: Temporal.ZonedDateTime): boolean {
    const jsDate = new Date(date.epochMilliseconds)
    return isJuly(date) || isWeekend(date) || hd.isHoliday(jsDate) !== false
}

function getPrice(date: Temporal.ZonedDateTime, ratesRepo: TollRatesRepository): number {
    const minutes = date.hour * 60 + date.minute
    const match = ratesRepo.getIntervals().find(i => minutes >= i.from && minutes <= i.to)
    return match?.price ?? 0
}

export function calculateToll(
    vehicle: Vehicle,
    dates: Temporal.ZonedDateTime[],
    ratesRepo: TollRatesRepository
): number {
    if (TOLL_FREE.has(vehicle)) return 0
    const sorted = [...dates].sort(Temporal.ZonedDateTime.compare)
    let total = 0
    let windowStart: Temporal.ZonedDateTime | null = null
    let windowMax = 0
    for (const d of sorted) {
        if (isTollFree(d)) continue
        const newWindow = windowStart === null ||
            d.since(windowStart).total('minutes') >= 60
        if (newWindow) {
            total += windowMax
            windowStart = d
            windowMax = getPrice(d, ratesRepo)
        } else {
            windowMax = Math.max(windowMax, getPrice(d, ratesRepo))
        }
    }
    return Math.min(total + windowMax, 60)
}
