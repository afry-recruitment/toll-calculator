import { IsEnum, IsNotEmpty, IsArray, IsDate } from 'class-validator';
import { Vehicle } from '../entities/vehicle.entity';

// DTO is redundant in the current implementation, but it's a good practice to use it
export class CheckTollRequestDto {
  @IsEnum(Vehicle)
  @IsNotEmpty()
  vehicle: Vehicle;

  @IsArray()
  @IsDate({ each: true })
  dateTimes: Date[];
}
