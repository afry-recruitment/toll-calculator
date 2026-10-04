import { Prop, Schema, SchemaFactory } from '@nestjs/mongoose';
import { VehicleType } from './types/toll.types.js';

@Schema({ collection: 'toll_entries', id: false, versionKey: false })
export class TollEntry {
  @Prop({ type: String, required: true, unique: true })
  id: string;

  @Prop({ type: String, required: true })
  vehicleNumber: string;

  @Prop({ type: String, required: true })
  registrationCountry: string;

  @Prop({ type: String, enum: Object.values(VehicleType), required: true })
  vehicleType: VehicleType;

  @Prop({ type: Date, required: true })
  passedAt: Date;

  @Prop({ type: String, required: true })
  date: string;

  @Prop({ type: Number, required: true, min: 0 })
  maxTollAmount: number;
}

export const TollEntrySchema = SchemaFactory.createForClass(TollEntry);

TollEntrySchema.index({ vehicleNumber: 1, registrationCountry: 1, date: 1 });
