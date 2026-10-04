import { Args, Query, Resolver } from '@nestjs/graphql';
import { toLocalDateTime } from '../utils/calendar/stockholm-time.js';
import { DailyTotalArgs } from './dto/daily-total.args.js';
import { DEFAULT_REGISTRATION_COUNTRY } from './dto/register-toll-entrance.dto.js';
import { TollService } from './toll.service.js';
import { DailyTotalView, toDailyTotalView } from './models/toll.model.js';

@Resolver()
export class TollResolver {
  constructor(private readonly tollService: TollService) {}

  @Query(() => DailyTotalView, {
    description: "A vehicle's total toll fee for one day",
  })
  async dailyTotal(@Args() args: DailyTotalArgs): Promise<DailyTotalView> {
    const date = args.date ?? toLocalDateTime(new Date()).date;
    return toDailyTotalView(
      await this.tollService.getDailyTotal(
        args.vehicleNumber,
        args.registrationCountry ?? DEFAULT_REGISTRATION_COUNTRY,
        date,
      ),
    );
  }
}
