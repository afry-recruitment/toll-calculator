import { describe, it, expect } from 'vitest'
import request from 'supertest'
import app from '../app.js'

describe('POST /api/toll/calculate', () => {
    it('returns 18 for Car at 07:00', async () => {
        const res = await request(app)
            .post('/api/toll/calculate')
            .send({ vehicle: 'Car', passages: ['2026-06-12T07:00[Europe/Stockholm]'] })

        expect(res.status).toBe(200)
        expect(res.body).toEqual({ total: 18, currency: 'SEK' })
    })

    it('returns 0 for toll free vehicle', async () => {
        const res = await request(app)
            .post('/api/toll/calculate')
            .send({ vehicle: 'Motorbike', passages: ['2026-06-12T07:00[Europe/Stockholm]'] })

        expect(res.status).toBe(200)
        expect(res.body).toEqual({ total: 0, currency: 'SEK' })
    })

    it('returns 400 for invalid vehicle', async () => {
        const res = await request(app)
            .post('/api/toll/calculate')
            .send({ vehicle: 'Bicycle', passages: [] })

        expect(res.status).toBe(400)
    })

    it('returns 400 for invalid date format', async () => {
        const res = await request(app)
            .post('/api/toll/calculate')
            .send({ vehicle: 'Car', passages: ['not-a-date'] })

        expect(res.status).toBe(400)
    })

    it('returns 400 when passages is missing', async () => {
        const res = await request(app)
            .post('/api/toll/calculate')
            .send({ vehicle: 'Car' })

        expect(res.status).toBe(400)
    })
})
