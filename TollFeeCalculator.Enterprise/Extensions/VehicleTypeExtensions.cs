using System.Collections.Concurrent;
using System.Reflection;
using TollFeeCalculator.Enterprise.Attributes;
using TollFeeCalculator.Enterprise.Enums;

namespace TollFeeCalculator.Enterprise.Extensions
{
    public static class VehicleTypeExtensions
    {
        private static readonly ConcurrentDictionary<VehicleType, bool> Cache = new();

        public static bool IsTollFree(this VehicleType type) =>
            Cache.GetOrAdd(type, static t =>
            {
                var member = typeof(VehicleType).GetField(t.ToString());
                return member?.GetCustomAttribute<TollFreeAttribute>() is not null;
            });
    }
}
