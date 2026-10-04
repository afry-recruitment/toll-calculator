import { randomUUID } from 'node:crypto';
import { Injectable } from '@nestjs/common';
import { InjectModel } from '@nestjs/mongoose';
import type { Model } from 'mongoose';
import { toLocalDateTime } from '../utils/calendar/stockholm-time.js';
import { VehicleType } from './types/toll.types.js';
import type { DailyTotal } from './types/toll.types.js';
import { TollFeeCalculator } from './toll-fee.calculator.js';
import { TollEntry } from './toll-entry.schema.js';
import { TransportstyrelsenClient } from '../transportstyrelsen/transportstyrelsen.client.js';

@Injectable()
export class TollService {
  constructor(
    @InjectModel(TollEntry.name) private readonly tollEntries: Model<TollEntry>,
    private readonly calculator: TollFeeCalculator,
    private readonly transportstyrelsen: TransportstyrelsenClient,
  ) {}

  async registerTollEntrance(
    vehicleNumber: string,
    registrationCountry: string,
    vehicleType: VehicleType,
    passedAt: Date,
  ): Promise<TollEntry> {
    const date = toLocalDateTime(passedAt).date;
    const earlier = await this.findForDay(
      vehicleNumber,
      registrationCountry,
      date,
    );
    const newEntry = { vehicleType, passedAt };

    const maxTollAmount = this.calculator.getDailyFee([...earlier, newEntry]);

    const entry = this.toEntry(
      await this.tollEntries.create({
        id: randomUUID(),
        vehicleNumber,
        registrationCountry,
        vehicleType,
        passedAt,
        date,
        maxTollAmount,
      }),
    );

    await this.transportstyrelsen.upsertTollTransaction(entry);
    return entry;
  }

  async getDailyTotal(
    vehicleNumber: string,
    registrationCountry: string,
    date: string,
  ): Promise<DailyTotal> {
    const saved = await this.findForDay(
      vehicleNumber,
      registrationCountry,
      date,
    );

    const totalFee = saved.at(-1)?.maxTollAmount ?? 0;
    const passages = [...saved].sort(
      (a, b) => a.passedAt.getTime() - b.passedAt.getTime(),
    );

    return {
      vehicleNumber,
      registrationCountry,
      date,
      totalFee,
      passages: passages.map((passage) => ({
        passage,
        fee: this.calculator.getPassageFee(
          passage.vehicleType,
          passage.passedAt,
        ),
      })),
    };
  }

  private async findForDay(
    vehicleNumber: string,
    registrationCountry: string,
    date: string,
  ): Promise<TollEntry[]> {
    const docs = await this.tollEntries
      .find({ vehicleNumber, registrationCountry, date })
      .sort({ _id: 1 })
      .exec();
    return docs.map((doc) => this.toEntry(doc));
  }

  private toEntry(doc: TollEntry): TollEntry {
    return {
      id: doc.id,
      vehicleNumber: doc.vehicleNumber,
      registrationCountry: doc.registrationCountry,
      vehicleType: doc.vehicleType,
      passedAt: doc.passedAt,
      date: doc.date,
      maxTollAmount: doc.maxTollAmount,
    };
  }
}
