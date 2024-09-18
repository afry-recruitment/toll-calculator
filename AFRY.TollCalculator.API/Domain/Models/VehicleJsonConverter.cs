using AFRY.TollCalculator.API.Domain.Models;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public class VehicleJsonConverter : JsonConverter<Vehicle>
{
    public override Vehicle? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
        {
            var vehicleType = doc.RootElement.GetProperty("vehicleType").GetString();

            return vehicleType switch
            {
                "Car" => JsonSerializer.Deserialize<Car>(doc.RootElement.GetRawText(), options),
                "Motorbike" => JsonSerializer.Deserialize<Motorbike>(doc.RootElement.GetRawText(), options),
                _ => throw new NotSupportedException($"Vehicle type {vehicleType} is not supported")
            };
        }
    }

    public override void Write(Utf8JsonWriter writer, Vehicle value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, (object)value, options);
    }
}
