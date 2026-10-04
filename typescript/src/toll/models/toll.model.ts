import { Field, Int, ObjectType, registerEnumType } from '@nestjs/graphql';
import { ApiProperty } from '@nestjs/swagger';
import { VehicleType } from '../types/toll.types.js';
import type { DailyTotal } from '../types/toll.types.js';
import { TollEntry } from '../toll-entry.schema.js';

registerEnumType(VehicleType, { name: 'VehicleType' });

export class PassageView {
  @ApiProperty({ format: 'uuid', description: 'Id of the toll entry' })
  id: string;

  @ApiProperty({ example: 'ABC123' })
  vehicleNumber: string;

  @ApiProperty({
    example: 'SE',
    description: 'ISO 3166-1 alpha-2 code of the registration country',
  })
  registrationCountry: string;

  @ApiProperty({ enum: VehicleType, enumName: 'VehicleType' })
  vehicleType: VehicleType;

  @ApiProperty({
    example: '2026-10-01T05:15:00.000Z',
    description: 'UTC instant',
  })
  passedAt: string;

  @ApiProperty({
    example: '2026-10-01',
    description: 'Local date (Europe/Stockholm)',
  })
  date: string;

  @ApiProperty({
    example: 18,
    description: "The vehicle's total for the day so far, in SEK",
  })
  dailyTotal: number;

  @ApiProperty({ enum: ['SEK'] })
  currency: 'SEK';
}

@ObjectType('DailyTotalPassage')
export class DailyTotalPassageView {
  @Field(() => String)
  id: string;

  @Field(() => VehicleType)
  vehicleType: VehicleType;

  @Field(() => String, { description: 'ISO 8601 instant (UTC)' })
  passedAt: string;

  @Field(() => Int, { description: 'Fee for this passage on its own, in SEK' })
  fee: number;
}

@ObjectType('DailyTotal')
export class DailyTotalView {
  @Field(() => String)
  vehicleNumber: string;

  @Field(() => String, {
    description: 'ISO 3166-1 alpha-2 code of the registration country',
  })
  registrationCountry: string;

  @Field(() => String, { description: 'Local date, YYYY-MM-DD' })
  date: string;

  @Field(() => Int, {
    description: 'Total fee for the day after the hourly rule and daily cap',
  })
  totalFee: number;

  @Field(() => String)
  currency: 'SEK';

  @Field(() => [DailyTotalPassageView])
  passages: DailyTotalPassageView[];
}

export function toPassageView(entry: TollEntry): PassageView {
  return {
    id: entry.id,
    vehicleNumber: entry.vehicleNumber,
    registrationCountry: entry.registrationCountry,
    vehicleType: entry.vehicleType,
    passedAt: entry.passedAt.toISOString(),
    date: entry.date,
    dailyTotal: entry.maxTollAmount,
    currency: 'SEK',
  };
}

export function toDailyTotalView(total: DailyTotal): DailyTotalView {
  return {
    vehicleNumber: total.vehicleNumber,
    registrationCountry: total.registrationCountry,
    date: total.date,
    totalFee: total.totalFee,
    currency: 'SEK',
    passages: total.passages.map(({ passage, fee }) => ({
      id: passage.id,
      vehicleType: passage.vehicleType,
      passedAt: passage.passedAt.toISOString(),
      fee,
    })),
  };
}
