import type {Request, Response} from 'express'
import {calculateToll} from '../services/toll.service.js'
import {Temporal} from '@js-temporal/polyfill'
import {z, prettifyError} from 'zod'
import type {TollRatesRepository} from '../repositories/toll-rates.repository.js'
import {TOLLABLE_VEHICLES, TOLL_FREE_VEHICLES} from '../models/vehicle.js'


const TollRequestSchema = z.object({
    vehicle: z.enum([...TOLLABLE_VEHICLES, ...TOLL_FREE_VEHICLES]),
    passages: z.array(z.string())
})

export class TollController {
    constructor(private readonly ratesRepo: TollRatesRepository) {}


    calculate = (req: Request, res: Response): void => {
        const parsed = TollRequestSchema.safeParse(req.body)

        if (!parsed.success) {
            res.status(400).json({error: prettifyError(parsed.error)})
            return
        }

        const {vehicle, passages} = parsed.data

        let zonedDates: Temporal.ZonedDateTime[]
        try {
            zonedDates = passages.map((p: string) => Temporal.ZonedDateTime.from(p))
        } catch {
            res.status(400).json({error: 'Invalid date/time format. Eg \"2026-06-17T07:00[Europe/Stockholm]\"'})
            return
        }

        const total = calculateToll(vehicle, zonedDates, this.ratesRepo)
        res.json({total, currency: 'SEK'})
    }
}
