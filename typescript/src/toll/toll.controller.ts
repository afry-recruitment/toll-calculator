import { BadRequestException, Body, Controller, Post } from '@nestjs/common';
import {
  ApiBadRequestResponse,
  ApiCreatedResponse,
  ApiOperation,
  ApiTags,
} from '@nestjs/swagger';
import {
  DEFAULT_REGISTRATION_COUNTRY,
  RegisterTollEntranceDto,
} from './dto/register-toll-entrance.dto.js';
import { TollService } from './toll.service.js';
import { PassageView, toPassageView } from './models/toll.model.js';

@ApiTags('toll')
@Controller('toll')
export class TollController {
  constructor(private readonly tollService: TollService) {}

  @Post('payfee')
  @ApiOperation({
    summary: 'Register a toll entrance',
    description:
      "Stores the entrance, applies the one-charge-per-hour and daily maximum rules, and returns the vehicle's total for the day including this entrance. The total for a day is read with the GraphQL query `dailyTotal` at /graphql.",
  })
  @ApiCreatedResponse({ type: PassageView })
  @ApiBadRequestResponse({
    description: 'Invalid vehicle number, country, type or timestamp',
  })
  async registerTollEntrance(
    @Body() dto: RegisterTollEntranceDto,
  ): Promise<PassageView> {
    const passedAt = dto.timestamp ? new Date(dto.timestamp) : new Date();
    if (Number.isNaN(passedAt.getTime())) {
      throw new BadRequestException('timestamp is not a valid date');
    }
    return toPassageView(
      await this.tollService.registerTollEntrance(
        dto.vehicleNumber,
        dto.registrationCountry ?? DEFAULT_REGISTRATION_COUNTRY,
        dto.vehicleType,
        passedAt,
      ),
    );
  }
}
