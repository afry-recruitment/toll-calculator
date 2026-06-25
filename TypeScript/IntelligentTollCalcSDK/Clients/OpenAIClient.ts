// Non-streaming model client backed by OpenAI's API.
// Classifies a vehicle model string into a general VehicleType.

import OpenAI from "openai";
import { IModelClient } from "./ModelClient";
import { AllVehicleTypes, toVehicleType, VehicleType } from "../Core/tollConfig";

export class OpenAIClient implements IModelClient {
    private client?: OpenAI;

    // apiKey is optional; when omitted the SDK reads OPENAI_API_KEY from the
    // environment. The underlying client is created lazily so registering the
    // provider never requires credentials up front.
    constructor(private readonly apiKey?: string, private readonly model: string = "gpt-4o-mini") {}

    private getClient(): OpenAI {
        if (!this.client) {
            this.client = this.apiKey ? new OpenAI({ apiKey: this.apiKey }) : new OpenAI();
        }
        return this.client;
    }

    async classifyVehicleType(vehicleModel: string): Promise<VehicleType> {
        if (!vehicleModel?.trim()) {
            return VehicleType.Unknown;
        }

        try {
            // Constrain the response to a JSON object whose only field is one of
            // the known vehicle types.
            const completion = await this.getClient().chat.completions.create({
                model: this.model,
                messages: [
                    { role: "system", content: "You classify vehicles into a fixed set of general categories." },
                    { role: "user", content: `Classify this vehicle: "${vehicleModel}".` }
                ],
                response_format: {
                    type: "json_schema",
                    json_schema: {
                        name: "vehicle_type",
                        strict: true,
                        schema: {
                            type: "object",
                            properties: {
                                vehicleType: { type: "string", enum: AllVehicleTypes }
                            },
                            required: ["vehicleType"],
                            additionalProperties: false
                        }
                    }
                }
            });

            const content = completion.choices[0]?.message?.content;
            if (!content) {
                return VehicleType.Unknown;
            }
            const parsed = JSON.parse(content) as { vehicleType?: string };
            return toVehicleType(parsed.vehicleType);
        } catch (error) {
            // Fail closed: if the provider is unreachable, unauthenticated, or
            // returns unparseable output, treat the vehicle as Unknown.
            console.warn(`OpenAI classification unavailable for "${vehicleModel}", treating as Unknown: ${(error as Error).message}`);
            return VehicleType.Unknown;
        }
    }
}
