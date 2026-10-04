import { registerAs } from '@nestjs/config';
import type { TollFeeConfig } from '../types/toll.types.js';

function readFee(name: string): number {
  const raw = process.env[name];
  const value = Number(raw);
  if (
    raw === undefined ||
    raw.trim() === '' ||
    !Number.isInteger(value) ||
    value < 0
  ) {
    throw new Error(
      `Environment variable ${name} must be a non-negative integer (got ${JSON.stringify(raw)})`,
    );
  }
  return value;
}

export function loadTollFeeConfig(): TollFeeConfig {
  const config: TollFeeConfig = {
    lowFee: readFee('TOLL_FEE_LOW'),
    mediumFee: readFee('TOLL_FEE_MEDIUM'),
    highFee: readFee('TOLL_FEE_HIGH'),
    maxDailyFee: readFee('TOLL_MAX_DAILY_FEE'),
  };
  if (config.maxDailyFee < config.highFee) {
    throw new Error('TOLL_MAX_DAILY_FEE must be at least TOLL_FEE_HIGH');
  }
  return config;
}

export const tollFeeConfig = registerAs('tollFee', loadTollFeeConfig);
