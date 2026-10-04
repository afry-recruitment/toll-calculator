import { ApiProperty, ApiPropertyOptional } from '@nestjs/swagger';
import { Transform } from 'class-transformer';
import {
  IsAlphanumeric,
  IsEnum,
  IsISO31661Alpha2,
  IsISO8601,
  IsOptional,
  Length,
  Matches,
} from 'class-validator';
import { VehicleType } from '../types/toll.types.js';

export const normalizeVehicleNumber = (value: unknown): unknown =>
  typeof value === 'string' ? value.replace(/[\s-]/g, '').toUpperCase() : value;

export const DEFAULT_REGISTRATION_COUNTRY = 'SE';

export const normalizeCountry = (value: unknown): unknown =>
  typeof value === 'string' ? value.trim().toUpperCase() : value;

export class RegisterTollEntranceDto {
  @ApiProperty({
    example: 'ABC123',
    description:
      'Registration number, 2-10 letters or digits. Spaces and dashes are removed and letters are upper-cased.',
  })
  @Transform(({ value }) => normalizeVehicleNumber(value))
  @IsAlphanumeric()
  @Length(2, 10)
  vehicleNumber: string;

  @ApiPropertyOptional({
    example: 'SE',
    default: DEFAULT_REGISTRATION_COUNTRY,
    description:
      'Country the vehicle is registered in, as an ISO 3166-1 alpha-2 code. Defaults to SE.',
  })
  @Transform(({ value }) => normalizeCountry(value))
  @IsOptional()
  @IsISO31661Alpha2()
  registrationCountry?: string;

  @ApiProperty({ enum: VehicleType, enumName: 'VehicleType' })
  @IsEnum(VehicleType)
  vehicleType: VehicleType;

  @ApiPropertyOptional({
    example: '2026-10-01T07:15:00+02:00',
    description:
      'Time of the toll entrance, ISO 8601 with a timezone (Z or ±hh:mm). Defaults to the current time.',
  })
  @IsOptional()
  @IsISO8601({ strict: true, strictSeparator: true })
  @Matches(/(Z|[+-]\d{2}:\d{2})$/, {
    message: 'timestamp must include a timezone (Z or ±hh:mm)',
  })
  timestamp?: string;
}
