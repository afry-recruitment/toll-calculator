export interface TollInterval {
    from: number
    to: number
    price: number
}

export interface TollRatesRepository {
    getIntervals(): TollInterval[]
}

export class InMemoryTollRatesRepository implements TollRatesRepository {
    getIntervals(): TollInterval[] {
        return [
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
}
