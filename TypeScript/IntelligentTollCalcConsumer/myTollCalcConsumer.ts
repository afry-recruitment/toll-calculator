import { IntelligentTollCalc, ModelProvider } from '../IntelligentTollCalcSDK/API/IntelligentTollCalc';

// Register the model providers. An empty key falls back to the provider's
// standard environment variable (OPENAI_API_KEY / ANTHROPIC_API_KEY); Ollama is
// local and needs no key.
IntelligentTollCalc.addOpenAI(process.env.OPENAI_API_KEY ?? "");
IntelligentTollCalc.addAnthropic(process.env.ANTHROPIC_API_KEY ?? "");
IntelligentTollCalc.addOllama();

// assessToll never throws on provider issues — clients fail closed, so an
// unavailable provider just classifies the vehicle as Unknown (chargeable).
async function demo(vehicleID: string, vehicleModel: string, modelProvider: ModelProvider): Promise<void> {
    const { toll, vehicleType } = await IntelligentTollCalc.assessToll(new Date(), vehicleID, vehicleModel, modelProvider);
    console.log(`${modelProvider}: ${vehicleModel} (${vehicleID}) classified as ${vehicleType} -> ${toll} SEK`);
}

// A regular car is charged; a motorcycle and a bus are toll-free vehicle types.
await demo("ABC123", "Volvo XC90", ModelProvider.OpenAI);
await demo("DEF456", "Harley-Davidson Street 750", ModelProvider.Anthropic);
await demo("GHI789", "Volvo 9700 coach bus", ModelProvider.Ollama);

// Without a provider the vehicle type can't be classified, so it is charged.
await demo("JKL012", "Volvo Amazon", ModelProvider.None);

await demo("JKL012", "Volvo Amazon", ModelProvider.Anthropic
);
