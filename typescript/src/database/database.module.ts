import { Module } from '@nestjs/common';
import { ConfigModule, ConfigService } from '@nestjs/config';
import { MongooseModule } from '@nestjs/mongoose';

@Module({
  imports: [
    MongooseModule.forRootAsync({
      imports: [ConfigModule],
      inject: [ConfigService],
      useFactory: (config: ConfigService) => {
        const uri = config.get<string>('MONGODB_URI');
        if (!uri) {
          throw new Error('Environment variable MONGODB_URI must be set');
        }
        return {
          uri,
          dbName: config.get<string>('MONGODB_DB') || 'toll',
          user: config.get<string>('MONGODB_USERNAME') || undefined,
          pass: config.get<string>('MONGODB_PASSWORD') || undefined,
        };
      },
    }),
  ],
})
export class DatabaseModule {}
