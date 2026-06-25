// API for Intelligent Toll Calculation SDK

// This file defines the IntelligentTollCalc class, which provides methods for
// calculating tolls based on time of day, vehicle model, and model provider, and
// for registering the model providers used to classify vehicles.

import { TollModel } from '../Core/TollModel';
import { ModelProvider, TollAssessment } from '../Core/tollConfig';
import { OpenAIClient } from '../Clients/OpenAIClient';
import { AnthropicClient } from '../Clients/AnthropicClient';
import { OllamaClient } from '../Clients/OllamaClient';

export { ModelProvider, VehicleType } from '../Core/tollConfig';
export type { TollAssessment } from '../Core/tollConfig';

export class IntelligentTollCalc {
    private static tollModel: TollModel = new TollModel();

    // calculate the toll for a passage based on the provided parameters
    public static async calculateToll(dateTime: Date, vehicleID: string, vehicleModel: string, modelProvider: ModelProvider): Promise<number> {
        return this.tollModel.calculateToll(dateTime, vehicleID, vehicleModel, modelProvider);
    }

    // assess a passage, returning both the toll charged and the classified vehicle type
    public static async assessToll(dateTime: Date, vehicleID: string, vehicleModel: string, modelProvider: ModelProvider): Promise<TollAssessment> {
        return this.tollModel.assessToll(dateTime, vehicleID, vehicleModel, modelProvider);
    }

    // register the OpenAI model provider. An empty apiKey falls back to the
    // OPENAI_API_KEY environment variable.
    public static addOpenAI(apiKey: string): void {
        this.tollModel.registerModelClient(ModelProvider.OpenAI, new OpenAIClient(apiKey || undefined));
    }

    // register the Anthropic model provider. An empty apiKey falls back to the
    // ANTHROPIC_API_KEY environment variable.
    public static addAnthropic(apiKey: string): void {
        this.tollModel.registerModelClient(ModelProvider.Anthropic, new AnthropicClient(apiKey || undefined));
    }

    // register the Ollama model provider (local, no API key). An optional baseUrl
    // overrides the default http://localhost:11434.
    public static addOllama(baseUrl?: string): void {
        this.tollModel.registerModelClient(ModelProvider.Ollama, new OllamaClient(baseUrl));
    }
}
