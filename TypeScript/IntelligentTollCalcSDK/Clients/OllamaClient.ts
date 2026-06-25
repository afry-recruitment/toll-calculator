// Non-streaming model client backed by a local Ollama instance (no API key).
// Classifies a vehicle model string into a general VehicleType.

import { IModelClient } from "./ModelClient";
import { AllVehicleTypes, VehicleType } from "../Core/tollConfig";

export class OllamaClient implements IModelClient {
    constructor(
        private readonly baseUrl: string = "http://localhost:11434",
        private readonly model: string = "gemma3:1b"
    ) {}

    async classifyVehicleType(vehicleModel: string): Promise<VehicleType> {
        if (!vehicleModel?.trim()) {
            return VehicleType.Unknown;
        }

        try {
            const response = await fetch(`${this.baseUrl}/api/generate`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({
                    model: this.model,
                    prompt: `Classify the vehicle "${vehicleModel}" into exactly one of these categories: ` +
                        `${AllVehicleTypes.join(", ")}. Respond with only the category name.`,
                    stream: false
                })
            });

            if (!response.ok) {
                console.warn(`Ollama classification unavailable for "${vehicleModel}", treating as Unknown: ${response.statusText}`);
                return VehicleType.Unknown;
            }

            const data = await response.json() as { response?: string };
            return OllamaClient.parseVehicleType(data.response);
        } catch (error) {
            // Fail closed: if the provider is unreachable, treat the vehicle as
            // Unknown (chargeable) rather than aborting the toll calculation.
            console.warn(`Ollama classification unavailable for "${vehicleModel}", treating as Unknown: ${(error as Error).message}`);
            return VehicleType.Unknown;
        }
    }

    // Free-text local models may answer with a word or short sentence, so match
    // the first known vehicle type that appears in the response.
    private static parseVehicleType(text: string | undefined): VehicleType {
        if (!text) {
            return VehicleType.Unknown;
        }
        const lower = text.toLowerCase();
        return AllVehicleTypes.find(t => lower.includes(t.toLowerCase())) ?? VehicleType.Unknown;
    }
}
