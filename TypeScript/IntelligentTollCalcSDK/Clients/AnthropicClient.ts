// Non-streaming model client backed by Anthropic's API.
// Classifies a vehicle model string into a general VehicleType.

import Anthropic from "@anthropic-ai/sdk";
import { IModelClient } from "./ModelClient";
import { AllVehicleTypes, toVehicleType, VehicleType } from "../Core/tollConfig";

export class AnthropicClient implements IModelClient {
    private client?: Anthropic;

    // apiKey is optional; when omitted the SDK reads ANTHROPIC_API_KEY from the
    // environment. The underlying client is created lazily so registering the
    // provider never requires credentials up front.
    constructor(private readonly apiKey?: string, private readonly model: string = "claude-opus-4-8") {}

    private getClient(): Anthropic {
        if (!this.client) {
            this.client = this.apiKey ? new Anthropic({ apiKey: this.apiKey }) : new Anthropic();
        }
        return this.client;
    }

    async classifyVehicleType(vehicleModel: string): Promise<VehicleType> {
        if (!vehicleModel?.trim()) {
            return VehicleType.Unknown;
        }

        try {
            // Force a single tool call whose input is constrained to the known
            // vehicle types, so the model can only return a valid category.
            const response = await this.getClient().messages.create({
                model: this.model,
                max_tokens: 256,
                tools: [{
                    name: "report_vehicle_type",
                    description: "Report the general category of a vehicle given its make and model.",
                    input_schema: {
                        type: "object",
                        properties: {
                            vehicleType: {
                                type: "string",
                                enum: AllVehicleTypes,
                                description: "The general category the vehicle belongs to."
                            }
                        },
                        required: ["vehicleType"],
                        additionalProperties: false
                    }
                }],
                tool_choice: { type: "tool", name: "report_vehicle_type" },
                messages: [{
                    role: "user",
                    content: `Classify this vehicle into one of the known categories: "${vehicleModel}".`
                }]
            });

            const toolUse = response.content.find(block => block.type === "tool_use");
            if (toolUse && toolUse.type === "tool_use") {
                const input = toolUse.input as { vehicleType?: string };
                return toVehicleType(input.vehicleType);
            }
            return VehicleType.Unknown;
        } catch (error) {
            // Fail closed: if the provider is unreachable or unauthenticated,
            // treat the vehicle as Unknown (chargeable) rather than aborting.
            console.warn(`Anthropic classification unavailable for "${vehicleModel}", treating as Unknown: ${(error as Error).message}`);
            return VehicleType.Unknown;
        }
    }
}
