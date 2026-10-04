# Toll calculator

NestJS service that calculates toll fees for vehicles passing toll stations. Entries are stored in MongoDB, and each one is forwarded to Transportstyrelsen (currently a stub).

- `POST /toll/payfee` registers a toll entrance.
- GraphQL query `dailyTotal` returns a vehicle's total for a day.

## Run

Needs Node.js 24 and a MongoDB.

```bash
npm install
cp .env.example .env      # set MONGODB_URI, adjust fees if needed
npm run start:dev         # http://localhost:3000
```

With Docker, which starts the service and a MongoDB:

```bash
cp .env.example .env
docker compose up --build
```

## Configuration

Read from environment variables or `.env`; variables set in the environment win. The app won't start if a required one is missing or invalid.

| Variable | Required | Meaning |
|---|---|---|
| `MONGODB_URI` | yes | MongoDB connection string |
| `MONGODB_DB` | no | Database name, default `toll` |
| `MONGODB_USERNAME`, `MONGODB_PASSWORD` | no | Credentials, kept out of the URI |
| `TOLL_FEE_LOW`, `TOLL_FEE_MEDIUM`, `TOLL_FEE_HIGH` | yes | Fees in SEK |
| `TOLL_MAX_DAILY_FEE` | yes | Daily maximum in SEK, at least `TOLL_FEE_HIGH` |
| `PORT` | no | HTTP port, default `3000` |

## API

Swagger UI is at `/api` and the GraphQL explorer at `/graphql`.

**`POST /toll/payfee`**

```json
{ "vehicleNumber": "ABC123", "registrationCountry": "SE", "vehicleType": "Car", "timestamp": "2026-10-01T07:15:00+02:00" }
```

- `registrationCountry`: optional, ISO 3166-1 alpha-2, default `SE`. The same number in two countries is two vehicles.
- `vehicleType`: `Car`, `Motorbike`, `Tractor`, `Emergency`, `Diplomat`, `Foreign` or `Military`.
- `timestamp`: optional, ISO 8601 with a timezone, default now.

Returns `{ id, vehicleNumber, registrationCountry, vehicleType, passedAt, date, dailyTotal, currency }`. `dailyTotal` is the vehicle's total for that day including this entrance.

**GraphQL `dailyTotal(vehicleNumber, registrationCountry?, date?)`**

```graphql
{ dailyTotal(vehicleNumber: "ABC123", date: "2026-10-01") { totalFee currency passages { vehicleType passedAt fee } } }
```

`date` is `YYYY-MM-DD` and defaults to today. Invalid arguments come back in `errors` with HTTP 200.

## Fee rules

Times are Stockholm local time.

| Time | Fee |
|---|---|
| 06:00–06:29, 08:30–14:59, 18:00–18:29 | low |
| 06:30–06:59, 08:00–08:29, 15:00–15:29, 17:00–17:59 | medium |
| 07:00–07:59, 15:30–16:59 | high |
| otherwise | 0 |

- Within 60 minutes of the first charged entrance, only the highest fee counts.
- A vehicle never pays more than `TOLL_MAX_DAILY_FEE` per day.
- Free: weekends, July, Swedish public holidays (calculated for any year), the day before a public holiday, and the vehicle types `Motorbike`, `Tractor`, `Emergency`, `Diplomat`, `Foreign` and `Military`.

The fee is worked out when an entry is saved and stored on it as `maxTollAmount`, the vehicle's capped total for the day. The saved entry is then sent to Transportstyrelsen.

Compared with the original Java code, this fixes the hourly window never moving forward, the missing 8 SEK fee for 09:00–14:29, and holidays being hard-coded for 2013 only.

## Development

```bash
npm test        # e2e tests with their own temporary MongoDB
npm run lint
npm run format
npm run build
```

The first test run downloads a MongoDB binary of about 150 MB.

## Layout

| Path | Contents |
|---|---|
| `src/toll/toll.controller.ts`, `toll.gql.resolver.ts` | REST and GraphQL entry points |
| `src/toll/toll.service.ts` | Registers entries and builds daily totals |
| `src/toll/toll-fee.calculator.ts` | Fee rules |
| `src/toll/toll-entry.schema.ts` | Mongoose schema of a toll entry |
| `src/toll/dto`, `models`, `types`, `config` | Request validation, response and GraphQL types, shared types, fee settings |
| `src/transportstyrelsen` | Client stub for Transportstyrelsen |
| `src/database` | MongoDB connection |
| `src/utils/calendar` | Stockholm time and Swedish holidays |

## Deployment

`.github/workflows/deploy-typescript.yml` builds the Docker image and deploys it with Kustomize to `dev`, `stage` or `prod`. See [.github/workflows/kustomize/README.md](.github/workflows/kustomize/README.md).
