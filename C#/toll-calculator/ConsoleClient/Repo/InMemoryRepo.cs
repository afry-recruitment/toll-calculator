using ConsoleClient.Interfaces;
using TollFeeCalculator;

namespace ConsoleClient.Repo
{
    public class InMemoryRepo
    {
        public static DateTime[] LoadNewDateAndTimes(int caseValue)
        {
            switch (caseValue)
            {
                case 1:
                    return new DateTime[]
                    {
                           new DateTime(2023, 3, 22, 6, 0, 0),
                           new DateTime(2023, 3, 22, 6, 29, 0),
                           new DateTime(2023, 3, 22, 6, 32, 0),
                           new DateTime(2023, 3, 22, 6, 45, 0),

                           new DateTime(2023, 3, 22, 7, 15, 0),
                           new DateTime(2023, 3, 22, 6, 29, 0),
                           new DateTime(2023, 3, 22, 7, 30, 0),
                           new DateTime(2023, 3, 22, 7, 59, 0),

                    };
                case 2:
                    return new DateTime[]
                    {
                           new DateTime(2023, 3, 22, 8, 0, 0),
                           new DateTime(2023, 3, 22, 8, 45, 0),
                           new DateTime(2023, 3, 22, 10, 15, 0),
                           new DateTime(2023, 3, 22, 13, 54, 0),
                           new DateTime(2023, 3, 22, 14, 54, 0)
                    };
                case 3:
                    return new DateTime[]
                    {
                           new DateTime(2023, 3, 22, 15, 0, 0),
                           new DateTime(2023, 3, 22, 15, 15, 0),
                           new DateTime(2023, 3, 22, 15, 59, 0),
                    };
                case 4:
                    return new DateTime[]
                    {
                           new DateTime(2023, 3, 22, 16, 30, 0),
                           new DateTime(2023, 3, 22, 16, 45, 0),
                           new DateTime(2023, 3, 22, 16, 55, 0),
                           new DateTime(2023, 3, 22, 16, 59, 0),

                           new DateTime(2023, 3, 22, 17, 30, 0),
                           new DateTime(2023, 3, 22, 17, 45, 0),
                           new DateTime(2023, 3, 22, 17, 55, 0),
                           new DateTime(2023, 3, 22, 17, 59, 0),

                           new DateTime(2023, 3, 22, 18, 15, 0),
                           new DateTime(2023, 3, 22, 18, 29, 0),
                           new DateTime(2023, 3, 22, 18, 55, 0),
                           new DateTime(2023, 3, 22, 18, 59, 0),
                    };
                case 5:
                    return new DateTime[]
                    {
                           new DateTime(2023, 3, 25, 9, 30, 0),
                           new DateTime(2023, 3, 25, 14, 0, 0),
                           new DateTime(2023, 3, 25, 16, 0, 0),

                           new DateTime(2023, 3, 26, 9, 30, 0),
                           new DateTime(2023, 3, 26, 12, 30, 0),
                           new DateTime(2023, 3, 26, 14, 10, 0),

                           new DateTime(2023, 3, 26, 16, 45, 0),
                           new DateTime(2023, 3, 26, 17, 30, 0)
                    };
                case 6:
                    return new DateTime[]
                    {
                           new DateTime(2023, 4, 7, 15, 0, 0),
                           new DateTime(2023, 4, 7, 16, 0, 0),
                           new DateTime(2023, 4, 7, 17, 0, 0),

                           new DateTime(2013, 3, 29, 15, 30, 0),
                           new DateTime(2013, 3, 29, 16, 30, 0),
                           new DateTime(2013, 3, 29, 17, 30, 0),

                           new DateTime(1989, 3, 24, 15, 30, 0),
                           new DateTime(1989, 3, 24, 16, 30, 0),
                           new DateTime(1989, 3, 24, 17, 30, 0),
                    };
                case 7:
                    return new DateTime[]
                    {
                           new DateTime(2023, 5, 1, 9, 30, 0),
                           new DateTime(2023, 5, 1, 11, 30, 0),
                           new DateTime(2023, 5, 1, 14, 30, 0),

                           new DateTime(2013, 5, 1, 9, 30, 0),
                           new DateTime(2013, 5, 1, 11, 30, 0),
                           new DateTime(2013, 5, 1, 14, 30, 0),

                           new DateTime(1989, 5, 1, 9, 30, 0),
                           new DateTime(1989, 5, 1, 11, 30, 0),
                           new DateTime(1989, 5, 1, 14, 30, 0)
                    };
                case 8:
                    return new DateTime[]
                    {
                           new DateTime(2023, 5, 18, 15, 0, 0),
                           new DateTime(2023, 5, 18, 16, 0, 0),
                           new DateTime(2023, 5, 18, 17, 0, 0),

                           new DateTime(2013, 5, 9, 15, 0, 0),
                           new DateTime(2013, 5, 9, 16, 0, 0),
                           new DateTime(2013, 5, 9, 17, 0, 0),

                           new DateTime(1989, 5, 4, 15, 0, 0),
                           new DateTime(1989, 5, 4, 16, 0, 0),
                           new DateTime(1989, 5, 4, 17, 0, 0),
                    };
                case 9:
                    return new DateTime[]
                    {
                           new DateTime(2023, 4, 10, 15, 0, 0),
                           new DateTime(2023, 4, 10, 16, 0, 0),
                           new DateTime(2023, 4, 10, 17, 0, 0),

                           new DateTime(2013, 4, 1, 15, 0, 0),
                           new DateTime(2013, 4, 1, 16, 0, 0),
                           new DateTime(2013, 4, 1, 17, 8, 0),

                           new DateTime(1989, 3, 27, 15, 0, 0),
                           new DateTime(1989, 3, 27, 16, 0, 0),
                           new DateTime(1989, 3, 27, 17, 0, 0),
                    };
                case 10:
                    return new DateTime[]
                    {
                           new DateTime(2023, 6, 6, 15, 0, 0),
                           new DateTime(2023, 6, 6, 16, 0, 0),
                           new DateTime(2023, 6, 6, 1, 0, 0)
                    };
                case 11:
                    return new DateTime[]
                    {
                           new DateTime(2023, 12, 25, 9, 30, 0),
                           new DateTime(2023, 12, 26, 14, 0, 0),
                           new DateTime(2013, 12, 25, 9, 0, 0),

                           new DateTime(2013, 12, 26, 14, 0, 0),
                           new DateTime(1989, 12, 25, 9, 0, 0),
                           new DateTime(1989, 12, 26, 14, 0, 0),
                    };
            }
            return null;
        }

        public static List<IVehicle> LoadVehicles() =>
            new List<IVehicle>
            {
                new Diplomat("ITR542"),
                new Car("AFE342"),
                new Foreign("VC34523"),
                new Car("GSO334"),
                new Emergency("CCE345"),
                new Military("TTY554"),
                new Car("GHF542"),
                new Motorbike("FFR564"),
                new Car("TTR428"),
                new Tractor("HKG586")
            };
    }
}
