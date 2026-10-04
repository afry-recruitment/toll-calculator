import { Module } from '@nestjs/common';
import { ConfigModule } from '@nestjs/config';
import { MongooseModule } from '@nestjs/mongoose';
import { tollFeeConfig } from './config/toll-fee.config.js';
import { TollEntry, TollEntrySchema } from './toll-entry.schema.js';
import { TollController } from './toll.controller.js';
import { TollFeeCalculator } from './toll-fee.calculator.js';
import { TollResolver } from './toll.gql.resolver.js';
import { TollService } from './toll.service.js';
import { TransportstyrelsenClient } from '../transportstyrelsen/transportstyrelsen.client.js';

@Module({
  imports: [
    ConfigModule.forFeature(tollFeeConfig),
    MongooseModule.forFeature([
      { name: TollEntry.name, schema: TollEntrySchema },
    ]),
  ],
  controllers: [TollController],
  providers: [
    TollService,
    TollResolver,
    TollFeeCalculator,
    TransportstyrelsenClient,
  ],
})
export class TollModule {}
