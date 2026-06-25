import { DefaultTollConfig, OfficialHolidays, ModelProvider, VehicleType, TollAssessment, feeForTime } from './tollConfig';
import TollCache from '../Data/TollCache';
import { IModelClient } from '../Clients/ModelClient';

export class TollModel {

    private modelClients: { [key in ModelProvider]?: IModelClient } = {};

    private tollCache: TollCache = new TollCache(DefaultTollConfig.maximumDailyToll);

    public registerModelClient(modelProvider: ModelProvider, modelClient: IModelClient): void {
        this.modelClients[modelProvider] = modelClient;
    }

    // weekends and configured public holidays are toll-free
    private isTollFreeDate(dateTime: Date): boolean {
        const day = dateTime.getDay();
        if (day === 0 || day === 6) {
            return true;
        }
        return OfficialHolidays.some(holiday =>
            holiday.getMonth() === dateTime.getMonth() &&
            holiday.getDate() === dateTime.getDate());
    }

    // use the registered model client (if any) to classify the vehicle model;
    // returns Unknown when no provider is registered for the given provider
    private async classifyVehicle(vehicleModel: string, modelProvider: ModelProvider): Promise<VehicleType> {
        const modelClient = this.modelClients[modelProvider];
        if (!modelClient) {
            return VehicleType.Unknown;
        }
        return modelClient.classifyVehicleType(vehicleModel);
    }

    // Calculate the toll charged for a single passage. See assessToll for details.
    public async calculateToll(dateTime: Date, vehicleID: string, vehicleModel: string, modelProvider: ModelProvider): Promise<number> {
        return (await this.assessToll(dateTime, vehicleID, vehicleModel, modelProvider)).toll;
    }

    // Assess a single passage, returning the toll charged and the vehicle type it
    // was classified as. Applies the once-per-hour (highest fee) rule and the
    // daily maximum via the cache. Classification (and thus an LLM call) only
    // happens when the passage is otherwise chargeable.
    public async assessToll(dateTime: Date, vehicleID: string, vehicleModel: string, modelProvider: ModelProvider): Promise<TollAssessment> {
        // weekends and holidays are free
        if (this.isTollFreeDate(dateTime)) {
            return { toll: 0, vehicleType: VehicleType.Unknown };
        }

        // free outside the chargeable hours of the day
        const fee = feeForTime(dateTime, DefaultTollConfig.feeSchedule);
        if (fee <= 0) {
            return { toll: 0, vehicleType: VehicleType.Unknown };
        }

        // classify the vehicle and exempt toll-free types
        const vehicleType = await this.classifyVehicle(vehicleModel, modelProvider);
        if (DefaultTollConfig.tollFreeVehicleTypes.includes(vehicleType)) {
            return { toll: 0, vehicleType };
        }

        const toll = this.tollCache.registerCharge(vehicleID, dateTime, fee);
        return { toll, vehicleType };
    }
}
