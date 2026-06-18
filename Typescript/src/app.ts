import express from 'express'
import morgan from 'morgan'
import tollRouter from './routes/toll.routes.js'

const app = express()

app.use(morgan('dev'))
app.use(express.json())
app.use('/api/toll', tollRouter)

export default app
