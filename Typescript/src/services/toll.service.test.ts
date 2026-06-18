import {describe, it, expect} from 'vitest'
import {Temporal} from '@js-temporal/polyfill'
import {calculateToll} from './toll.service.js'
import type {TollRatesRepository} from '../repositories/toll-rates.repository.js'

const fakeRepo: TollRatesRepository = {
    getIntervals: () => [
        {from: 6 * 60, to: 6 * 60 + 29, price: 8},
        {from: 6 * 60 + 30, to: 6 * 60 + 59, price: 13},
        {from: 7 * 60, to: 7 * 60 + 59, price: 18},
        {from: 8 * 60, to: 8 * 60 + 29, price: 13},
        {from: 8 * 60 + 30, to: 14 * 60 + 59, price: 8},
        {from: 15 * 60, to: 15 * 60 + 29, price: 13},
        {from: 15 * 60 + 30, to: 16 * 60 + 59, price: 18},
        {from: 17 * 60, to: 17 * 60 + 59, price: 13},
        {from: 18 * 60, to: 18 * 60 + 29, price: 8},
    ]
}

const sthlm = (s: string) => Temporal.ZonedDateTime.from(`${s}[Europe/Stockholm]`)

describe('calculateToll', () => {
    it('single passage at 07:00 -> 18 SEK', () => {
        expect(calculateToll('Truck', [sthlm('2026-06-12T07:00')], fakeRepo)).toBe(18)
    })
    it('passage saturday -> 0 SEK', () => {
        expect(calculateToll('Car', [sthlm('2026-06-13T07:00')], fakeRepo)).toBe(0)
    })

    it('passage on public holiday -> 0 SEK', () => {
        expect(calculateToll('Car', [sthlm('2026-04-06 07:00')], fakeRepo)).toBe(0)
    })

    it('passage with toll free vehicle -> 0 SEK', () => {
        expect(calculateToll('Motorbike', [sthlm('2026-06-12 07:00')], fakeRepo)).toBe(0)
    })

    it('two passages at 07:00 and 07:59 -> 18 SEK', () => {
        expect(calculateToll('Truck', [sthlm('2026-06-12 07:00'), sthlm('2026-06-12 07:59')], fakeRepo)).toBe(18)
    })

    it('two passages at 07:00 and 08:00 -> 31 SEK', () => {
        expect(calculateToll('Van', [sthlm('2026-06-12 07:00'), sthlm('2026-06-12 08:00')], fakeRepo)).toBe(31)
    })

    it('two passages at 07:00 and 08:01 -> 31 SEK', () => {
        expect(calculateToll('Car', [sthlm('2026-06-12 07:00'), sthlm('2026-06-12 08:01')], fakeRepo)).toBe(31)
    })

    it('two passages at 07:00 and 08:59 -> 26 SEK', () => {
        expect(calculateToll('Car', [sthlm('2026-06-12 07:00'), sthlm('2026-06-12 08:59')], fakeRepo)).toBe(26)
    })

    it('two passages at in July -> 0 SEK', () => {
        expect(calculateToll('Car', [sthlm('2026-07-12 07:00'), sthlm('2026-07-12 08:59')], fakeRepo)).toBe(0)
    })

    it('daily max -> 60 SEK', () => {
        const result = calculateToll('Car', [sthlm('2026-06-12 07:00'),
        sthlm('2026-06-12 08:59'),
        sthlm('2026-06-12 10:00'),
        sthlm('2026-06-12 12:00'),
        sthlm('2026-06-12 14:00'),
        sthlm('2026-06-12 17:00'),
        sthlm('2026-06-12 19:00'),
        sthlm('2026-06-12 21:00'),
        ], fakeRepo)
        expect(result).toBe(60)
    })

    it('empty array -> 0 SEK', () => {
        const result = calculateToll('Car', [], fakeRepo)
        expect(result).toBe(0)
    })
})
