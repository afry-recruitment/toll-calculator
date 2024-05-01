using Tollculator.Enums;

namespace Tollculator.VehicleRegister;

public static class Register
{
    /// <summary>
    /// Vehicle register.
    /// </summary>
    public static readonly IDictionary<string, VehicleType> Vehicles = new Dictionary<string, VehicleType>
    {
        { "AAA111", VehicleType.Car },
        { "BBB111", VehicleType.Car },
        { "CCC111", VehicleType.Truck },
        { "DDD111", VehicleType.Car },
        { "EEE111", VehicleType.Emergency },
        { "FFF111", VehicleType.Car },
        { "GGG111", VehicleType.Foreign },
        { "HHH111", VehicleType.Military },
        { "III111", VehicleType.Car },
        { "JJJ111", VehicleType.Motorbike },
        { "KKK111", VehicleType.Car },
        { "LLL111", VehicleType.Tractor },
        { "MMM111", VehicleType.Emergency },
        { "NNN111", VehicleType.Diplomat },
        { "OOO111", VehicleType.Car },
        { "PPP111", VehicleType.Military },
        { "QQQ111", VehicleType.Car },
        { "RRR111", VehicleType.Motorbike },
        { "SSS111", VehicleType.Truck },
        { "TTT111", VehicleType.Car },
        { "UUU111", VehicleType.Emergency },
        { "VVV111", VehicleType.Truck },
        { "WWW111", VehicleType.Truck },
        { "XXX111", VehicleType.Truck },
        { "YYY111", VehicleType.Car },
        { "ZZZ111", VehicleType.Motorbike },
        { "AAA222", VehicleType.Truck },
        { "BBB222", VehicleType.Car },
        { "CCC222", VehicleType.Car },
        { "DDD222", VehicleType.Car },
        { "EEE222", VehicleType.Foreign },
        { "FFF222", VehicleType.Military },
        { "GGG222", VehicleType.Car },
        { "HHH222", VehicleType.Motorbike },
        { "III222", VehicleType.Truck },
        { "JJJ222", VehicleType.Truck },
        { "KKK222", VehicleType.Truck },
        { "LLL222", VehicleType.Truck },
        { "MMM222", VehicleType.Truck },
        { "NNN222", VehicleType.Truck },
        { "OOO222", VehicleType.Car },
        { "PPP222", VehicleType.Motorbike },
        { "QQQ222", VehicleType.Truck },
        { "RRR222", VehicleType.Tractor },
        { "SSS222", VehicleType.Car },
        { "TTT222", VehicleType.Car },
        { "UUU222", VehicleType.Car },
        { "VVV222", VehicleType.Car },
        { "WWW222", VehicleType.Car },
        { "XXX222", VehicleType.Motorbike },
        { "YYY222", VehicleType.Truck },
        { "ZZZ222", VehicleType.Tractor },
        { "AAA333", VehicleType.Truck },
        { "BBB333", VehicleType.Diplomat },
        { "CCC333", VehicleType.Foreign },
        { "DDD333", VehicleType.Military },
        { "EEE333", VehicleType.Car },
        { "FFF333", VehicleType.Truck },
        { "GGG333", VehicleType.Truck },
        { "HHH333", VehicleType.Tractor },
        { "III333", VehicleType.Emergency },
        { "JJJ333", VehicleType.Diplomat },
        { "KKK333", VehicleType.Foreign },
        { "LLL333", VehicleType.Military },
        { "MMM333", VehicleType.Car },
        { "NNN333", VehicleType.Motorbike },
        { "OOO333", VehicleType.Car },
        { "PPP333", VehicleType.Car },
        { "QQQ333", VehicleType.Emergency },
        { "RRR333", VehicleType.Diplomat },
        { "SSS333", VehicleType.Foreign },
        { "TTT333", VehicleType.Military },
        { "UUU333", VehicleType.Car },
        { "VVV333", VehicleType.Car },
        { "WWW333", VehicleType.Truck },
        { "XXX333", VehicleType.Truck },
        { "YYY333", VehicleType.Truck },
        { "ZZZ333", VehicleType.Diplomat }
    };

    /// <summary>
    /// All the toll free vehicles.
    /// </summary>
    public static readonly IReadOnlyList<VehicleType> TollFreeVechicleTypes =
    [
        VehicleType.Motorbike,
        VehicleType.Tractor,
        VehicleType.Emergency,
        VehicleType.Diplomat,
        VehicleType.Foreign,
        VehicleType.Military,
    ];
}
