import { getModelToken } from '@nestjs/mongoose';
import { Test } from '@nestjs/testing';
import mongoose from 'mongoose';
import type { Model } from 'mongoose';
import { INestApplication, ValidationPipe } from '@nestjs/common';
import request from 'supertest';
import { App } from 'supertest/types.js';
import { AppModule } from './../src/app.module.js';
import { TollEntry } from './../src/toll/toll-entry.schema.js';
import { TransportstyrelsenClient } from './../src/transportstyrelsen/transportstyrelsen.client.js';

const at = (time: string) => `2026-10-01T${time}:00+02:00`;

async function createApp(): Promise<INestApplication<App>> {
  const moduleFixture = await Test.createTestingModule({
    imports: [AppModule],
  }).compile();
  const app = moduleFixture.createNestApplication<INestApplication<App>>();

  app.useGlobalPipes(
    new ValidationPipe({
      whitelist: true,
      forbidNonWhitelisted: true,
      transform: true,
    }),
  );
  await app.init();
  await app.get<Model<TollEntry>>(getModelToken(TollEntry.name)).deleteMany({});
  return app;
}

const originalEnv = new Map<string, string | undefined>();
function stubEnv(name: string, value: string) {
  if (!originalEnv.has(name)) originalEnv.set(name, process.env[name]);
  process.env[name] = value;
}
function restoreEnv() {
  for (const [name, value] of originalEnv) {
    if (value === undefined) delete process.env[name];
    else process.env[name] = value;
  }
  originalEnv.clear();
}

describe('Toll API (e2e)', () => {
  let app: INestApplication<App>;

  beforeEach(async () => {
    app = await createApp();
  });

  afterEach(async () => {
    await app.close();
    await Promise.all(mongoose.connections.map((c) => c.close()));
    restoreEnv();
  });

  const pass = (body: object) =>
    request(app.getHttpServer()).post('/toll/payfee').send(body);

  const passCar = async (vehicleNumber: string, timestamp: string) =>
    (await pass({ vehicleNumber, vehicleType: 'Car', timestamp }).expect(201))
      .body;

  const DAILY_TOTAL_QUERY = `
    query ($vehicleNumber: String!, $registrationCountry: String, $date: String) {
      dailyTotal(vehicleNumber: $vehicleNumber, registrationCountry: $registrationCountry, date: $date) {
        vehicleNumber
        registrationCountry
        date
        totalFee
        currency
        passages { id vehicleType passedAt fee }
      }
    }`;

  const graphql = (variables: object) =>
    request(app.getHttpServer())
      .post('/graphql')
      .send({ query: DAILY_TOTAL_QUERY, variables });

  const dailyTotal = async (vehicleNumber: string, date = '2026-10-01') => {
    const res = await graphql({ vehicleNumber, date }).expect(200);
    expect(res.body.errors).toBeUndefined();
    return res.body.data.dailyTotal;
  };

  describe('POST /toll/payfee', () => {
    it("reports the vehicle's total for the day after each passage", async () => {
      expect(await passCar('abc 123', at('06:20'))).toMatchObject({
        vehicleNumber: 'ABC123',
        date: '2026-10-01',
        dailyTotal: 8,
        currency: 'SEK',
      });

      expect(await passCar('ABC123', at('07:10'))).toMatchObject({
        dailyTotal: 18,
      });
    });

    it.each([
      ['05:59', 0],
      ['06:00', 8],
      ['06:29', 8],
      ['06:30', 13],
      ['07:00', 18],
      ['07:59', 18],
      ['08:00', 13],
      ['08:30', 8],
      ['09:15', 8],
      ['14:59', 8],
      ['15:00', 13],
      ['15:30', 18],
      ['16:59', 18],
      ['17:00', 13],
      ['18:00', 8],
      ['18:29', 8],
      ['18:30', 0],
      ['23:00', 0],
    ])('charges a car at %s %i SEK', async (time, fee) => {
      expect((await passCar('CAR1', at(time))).dailyTotal).toBe(fee);
    });

    it.each([
      'Motorbike',
      'Tractor',
      'Emergency',
      'Diplomat',
      'Foreign',
      'Military',
    ])('does not charge a %s', async (vehicleType) => {
      const res = await pass({
        vehicleNumber: 'FREE1',
        vehicleType,
        timestamp: at('07:30'),
      }).expect(201);
      expect(res.body).toMatchObject({ dailyTotal: 0 });
    });

    it.each([
      ['a Saturday', '2026-10-03T07:30:00+02:00'],
      ['Christmas Day', '2026-12-25T07:30:00+01:00'],
      ['July', '2026-07-15T07:30:00+02:00'],
      ['Good Friday', '2026-04-03T07:30:00+02:00'],
      ['the day before Good Friday', '2026-04-02T07:30:00+02:00'],
      ['Midsummer Eve', '2026-06-19T07:30:00+02:00'],
      ['the day before Ascension Day (2013)', '2013-05-08T07:30:00+02:00'],
    ])('does not charge on %s', async (_, timestamp) => {
      expect((await passCar('DAY1', timestamp)).dailyTotal).toBe(0);
    });

    it('uses Stockholm local time regardless of the input offset', async () => {
      expect(await passCar('TZ1', '2026-10-01T05:30:00Z')).toMatchObject({
        dailyTotal: 18,
        date: '2026-10-01',
      });

      expect((await passCar('TZ1', '2026-10-01T22:30:00Z')).date).toBe(
        '2026-10-02',
      );
    });

    it('defaults the timestamp to now', async () => {
      const res = await pass({
        vehicleNumber: 'NOW1',
        vehicleType: 'Car',
      }).expect(201);
      expect(Date.now() - Date.parse(res.body.passedAt)).toBeLessThan(5_000);
    });

    it.each([
      [{ vehicleType: 'Car' }],
      [{ vehicleNumber: 'ABC123', vehicleType: 'Bus' }],
      [{ vehicleNumber: 'ABC123', vehicleType: 'Car', timestamp: 'yesterday' }],
      [
        {
          vehicleNumber: 'ABC123',
          vehicleType: 'Car',
          timestamp: '2026-10-01T07:00:00',
        },
      ],
      [{ vehicleNumber: 'ABC123', vehicleType: 'Car', extra: true }],
    ])('rejects invalid input %j', async (body) => {
      await pass(body).expect(400);
    });
  });

  describe('registration country', () => {
    const passIn = (registrationCountry: string, time: string) =>
      pass({
        vehicleNumber: 'ABC123',
        registrationCountry,
        vehicleType: 'Car',
        timestamp: at(time),
      });

    it('defaults to SE and is returned and stored with the entry', async () => {
      const res = await pass({
        vehicleNumber: 'ABC123',
        vehicleType: 'Car',
        timestamp: at('07:00'),
      }).expect(201);
      expect(res.body.registrationCountry).toBe('SE');

      const total = await dailyTotal('ABC123');
      expect(total).toMatchObject({
        registrationCountry: 'SE',
        totalFee: 18,
      });
    });

    it('is upper-cased, and the same number in another country is another vehicle', async () => {
      const norway = await passIn('no', '07:00').expect(201);
      expect(norway.body.registrationCountry).toBe('NO');
      await passIn('SE', '07:10').expect(201);

      const noTotal = await graphql({
        vehicleNumber: 'ABC123',
        registrationCountry: 'no',
        date: '2026-10-01',
      }).expect(200);
      expect(noTotal.body.data.dailyTotal).toMatchObject({
        registrationCountry: 'NO',
        totalFee: 18,
      });
      expect(noTotal.body.data.dailyTotal.passages).toHaveLength(1);
      expect((await dailyTotal('ABC123')).passages).toHaveLength(1);
    });

    it('is sent to Transportstyrelsen with the saved entry', async () => {
      const entry = (await passIn('DK', '06:20').expect(201)).body;
      expect(
        app.get(TransportstyrelsenClient).getTollTransaction(entry.id),
      ).toMatchObject({ registrationCountry: 'DK', vehicleNumber: 'ABC123' });
    });

    it.each(['SWE', 'S', 'XX', '12', ''])(
      'rejects the country %j',
      async (c) => {
        await passIn(c, '07:00').expect(400);
      },
    );

    it('rejects an invalid country in the query', async () => {
      const res = await graphql({
        vehicleNumber: 'ABC123',
        registrationCountry: 'XX',
      });
      expect(res.body.errors).toHaveLength(1);
    });
  });

  describe('GraphQL dailyTotal query', () => {
    it('sums one vehicle on one day, listing each passage', async () => {
      await passCar('ABC123', at('06:20'));
      await passCar('ABC123', at('07:10'));
      await passCar('ABC123', at('15:45'));

      await passCar('XYZ999', at('07:30'));
      await passCar('ABC123', '2026-10-02T07:30:00+02:00');

      const total = await dailyTotal('abc123');
      expect(total).toMatchObject({
        vehicleNumber: 'ABC123',
        date: '2026-10-01',
        totalFee: 36,
        currency: 'SEK',
      });
      expect(total.passages.map((p: { fee: number }) => p.fee)).toEqual([
        8, 18, 18,
      ]);
    });

    it('starts a new hour 60 minutes after the first charged passage', async () => {
      for (const time of ['06:00', '06:30', '07:00']) {
        await passCar('HOUR1', at(time));
      }

      expect((await dailyTotal('HOUR1')).totalFee).toBe(31);
    });

    it('does not let a free passage open a charge window', async () => {
      for (const time of ['05:50', '06:20', '06:55']) {
        await passCar('FREEWIN1', at(time));
      }
      expect((await dailyTotal('FREEWIN1')).totalFee).toBe(13);
    });

    it('handles passages registered out of order', async () => {
      for (const time of ['15:45', '06:10', '15:10']) {
        await passCar('ORDER1', at(time));
      }
      expect((await dailyTotal('ORDER1')).totalFee).toBe(8 + 18);
    });

    it('caps the daily total at the maximum fee', async () => {
      const times = ['06:30', '07:31', '08:32', '15:00', '16:01', '17:02'];
      const responses = [];
      for (const time of times) responses.push(await passCar('CAP1', at(time)));

      expect(responses.at(-1)).toMatchObject({ dailyTotal: 60 });
      expect((await dailyTotal('CAP1')).totalFee).toBe(60);
    });

    it('returns 0 for an unknown vehicle', async () => {
      expect(await dailyTotal('NONE1')).toMatchObject({
        totalFee: 0,
        passages: [],
      });
    });

    it('defaults the date to today', async () => {
      await pass({ vehicleNumber: 'TODAY1', vehicleType: 'Car' }).expect(201);
      const res = await graphql({ vehicleNumber: 'TODAY1' }).expect(200);
      expect(res.body.data.dailyTotal.passages).toHaveLength(1);
    });

    it.each([
      [{ vehicleNumber: 'ABC123', date: '2026-13-45' }],
      [{ vehicleNumber: 'ABC123', date: 'today' }],
      [{ vehicleNumber: 'A' }],
      [{ vehicleNumber: 'AB C!' }],
    ])('rejects invalid arguments %j', async (variables) => {
      const res = await graphql(variables);
      expect(res.body.errors).toHaveLength(1);
      expect(res.body.data).toBeNull();
    });

    it('no longer serves the daily total over REST', async () => {
      await request(app.getHttpServer())
        .get('/toll/vehicles/ABC123/daily-total')
        .expect(404);
    });
  });

  describe('Transportstyrelsen transaction', () => {
    it('is created for each toll entrance, and updated when resent', async () => {
      const client = app.get(TransportstyrelsenClient);
      const entry = await passCar('TS1', at('06:20'));

      expect(client.getTollTransaction(entry.id)).toEqual({
        id: entry.id,
        vehicleNumber: 'TS1',
        registrationCountry: 'SE',
        vehicleType: 'Car',
        passedAt: new Date('2026-10-01T04:20:00.000Z'),
        date: '2026-10-01',
        maxTollAmount: 8,
      });

      const saved = client.getTollTransaction(entry.id)!;
      const first = await client.upsertTollTransaction(saved);
      const second = await client.upsertTollTransaction(
        Object.assign(new TollEntry(), saved, {
          maxTollAmount: 13,
        }),
      );
      expect(first.status).toBe('UPDATED');
      expect(second).toMatchObject({
        status: 'UPDATED',
        reference: first.reference,
      });
      expect(client.getTollTransaction(entry.id)?.maxTollAmount).toBe(13);
    });

    it('receives the capped total once the daily maximum is reached', async () => {
      const client = app.get(TransportstyrelsenClient);
      let last;
      for (const time of [
        '06:30',
        '07:31',
        '08:32',
        '15:00',
        '16:01',
        '17:02',
      ]) {
        last = await passCar('TS3', at(time));
      }
      expect(client.getTollTransaction(last.id)).toMatchObject({
        maxTollAmount: 60,
      });
    });

    it('is not created when the request is invalid', async () => {
      await pass({ vehicleNumber: 'TS2', vehicleType: 'Bus' }).expect(400);
      expect(
        (await dailyTotal('TS2', at('06:20').slice(0, 10))).passages,
      ).toEqual([]);
    });
  });

  describe('fee configuration', () => {
    it('uses the fees from the environment', async () => {
      await app.close();
      stubEnv('TOLL_FEE_LOW', '10');
      stubEnv('TOLL_FEE_MEDIUM', '20');
      stubEnv('TOLL_FEE_HIGH', '30');
      stubEnv('TOLL_MAX_DAILY_FEE', '45');
      app = await createApp();

      expect((await passCar('CFG1', at('06:10'))).dailyTotal).toBe(10);
      expect((await passCar('CFG1', at('07:30'))).dailyTotal).toBe(40);

      expect((await passCar('CFG1', at('15:45'))).dailyTotal).toBe(45);
      expect((await passCar('CFG2', at('06:40'))).dailyTotal).toBe(20);
    });

    it('refuses to start without a MongoDB connection string', async () => {
      stubEnv('MONGODB_URI', '');
      await expect(createApp()).rejects.toThrow('MONGODB_URI');
    });

    it.each([
      ['TOLL_FEE_HIGH', 'abc'],
      ['TOLL_FEE_LOW', '-1'],
      ['TOLL_FEE_MEDIUM', '2.5'],
      ['TOLL_MAX_DAILY_FEE', '10'],
    ])('refuses to start when %s=%s', async (name, value) => {
      stubEnv(name, value);
      await expect(createApp()).rejects.toThrow(name);
    });
  });
});
