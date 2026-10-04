import { ArgsType, Field } from '@nestjs/graphql';
import { Transform } from 'class-transformer';
import {
  IsAlphanumeric,
  IsISO31661Alpha2,
  IsISO8601,
  IsOptional,
  Length,
  Matches,
} from 'class-validator';
import {
  normalizeCountry,
  normalizeVehicleNumber,
} from './register-toll-entrance.dto.js';

@ArgsType()
export class DailyTotalArgs {
  @Field(() => String)
  @Transform(({ value }) => normalizeVehicleNumber(value))
  @IsAlphanumeric()
  @Length(2, 10)
  vehicleNumber: string;

  @Field(() => String, {
    nullable: true,
    description:
      'ISO 3166-1 alpha-2 code of the registration country; defaults to SE',
  })
  @Transform(({ value }) => normalizeCountry(value))
  @IsOptional()
  @IsISO31661Alpha2()
  registrationCountry?: string;

  @Field(() => String, { nullable: true })
  @IsOptional()
  @Matches(/^\d{4}-\d{2}-\d{2}$/, { message: 'date must be YYYY-MM-DD' })
  @IsISO8601({ strict: true })
  date?: string;
}
