import {Router} from 'express'
import {TollController} from '../controllers/toll.controller.js'
import {InMemoryTollRatesRepository} from '../repositories/toll-rates.repository.js'


const router = Router()
const tollController = new TollController(new InMemoryTollRatesRepository())

router.post('/calculate', tollController.calculate)

export default router
