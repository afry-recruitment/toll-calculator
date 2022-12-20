using System.Text.Json;
using toll_calculator.models;
using System.Reflection;
using toll_calculator.exceptions;

namespace toll_calculator.repository;

internal static class TrafficTollSpecificationRepository
{
    public static TrafficTollSpecification GetTrafficTollSpecification()
    {
        return GetTollSpecificationFromAssemblyEmbeddedJsonFile();
    }

    private static TrafficTollSpecification GetTollSpecificationFromAssemblyEmbeddedJsonFile()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = $"{nameof(toll_calculator)}.{nameof(repository)}.traffic_toll_2022.json";

        Stream stream = null!;
        StreamReader reader = null!;
        try
        {
            stream = assembly.GetManifestResourceStream(resourceName)!;
            reader = new StreamReader(stream);
            var trafficTollJson = reader.ReadToEnd();
            return JsonSerializer.Deserialize<TrafficTollSpecification>(trafficTollJson) ?? throw new ArgumentNullException();
        }
        catch (Exception ex)
        {
            throw new TrafficTollDataRetrievalException(nameof(TrafficTollSpecification), ex);
        }
        finally
        {
            reader?.Dispose();
            stream?.Dispose();
        }       
    }
}