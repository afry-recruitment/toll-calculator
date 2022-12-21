using System.Text.Json;
using System.Reflection;
using TrafficToll.Internals.Models;
using TrafficToll.Internals.Exceptions;

namespace TrafficToll.Internals.DataAccess;

internal static class TrafficTollSpecificationRepository
{
    private static TrafficTollSpecification? __trafficTollSpecification;
    private static TrafficTollSpecification TrafficTollSpecification => __trafficTollSpecification ??= GetTollSpecificationFromAssemblyEmbeddedJsonFile();
    public static TrafficTollSpecification GetDailyTrafficTollSpecification()
    {
        return TrafficTollSpecification;
    }

    internal static TrafficTollSpecification? GetTrafficTollSpecification()
    {
        throw new NotImplementedException();
    }

    private static TrafficTollSpecification GetTollSpecificationFromAssemblyEmbeddedJsonFile()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = $"{nameof(TrafficToll)}.{nameof(Internals)}.{nameof(DataAccess)}.traffic_toll_2013.json";

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