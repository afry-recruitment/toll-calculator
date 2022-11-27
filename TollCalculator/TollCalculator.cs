using TollFeeCalculator.Interfaces;

namespace TollFeeCalculator
{
    public class TollCalculator
    {
        private readonly Repository repository = new Repository();
        private const int MAX_FEE_ONE_DAY = 60;

        /**
         * Calculate the total toll fee for one day
         *
         * @param vehicle - the vehicle
         * @param passageTimes   - date and time of all passes on one day
         * @return - the total toll fee for that day
         */

        public int GetTollFee(IVehicle vehicle, DateTime[] passageTimes)
        {
            int totalFee = 0;
            int highestFeeThisHour = 0;

            if (passageTimes != null && passageTimes.Length > 0)
            {
                Array.Sort(passageTimes);
                DateTime firstPassage = passageTimes[0];

                for (int i = 0; i < passageTimes.Length; i++)
                {
                    DateTime timeframeMultiplePassages = firstPassage.AddMinutes(60);
                    DateTime currentPassage = passageTimes[i];

                    if (currentPassage <= timeframeMultiplePassages)
                    {
                        var fee = GetTollFeeForPassage(currentPassage, vehicle);
                        if (fee > highestFeeThisHour) highestFeeThisHour = fee;
                    }
                    else
                    {
                        totalFee = totalFee + highestFeeThisHour;
                        firstPassage = currentPassage;
                        highestFeeThisHour = GetTollFeeForPassage(currentPassage, vehicle);
                    }
                }

                totalFee = totalFee + highestFeeThisHour;
                if (totalFee > MAX_FEE_ONE_DAY) totalFee = MAX_FEE_ONE_DAY;
            }

            return totalFee;

        }

        public int GetTollFeeForPassage(DateTime passageTime, IVehicle vehicle)
        {
            if (IsTollFreeDate(passageTime) || IsTollFreeVehicle(vehicle)) return 0;

            return repository.GetTollFeeForTime(TimeOnly.FromDateTime(passageTime));

        }

        public bool IsTollFreeVehicle(IVehicle vehicle)
        {
            if (vehicle == null) return false;
            return repository.IsTollFreeVehicle(vehicle.GetVehicleType());
        }

        public bool IsTollFreeDate(DateTime passageTime)
        {
            if (passageTime.DayOfWeek == DayOfWeek.Saturday || passageTime.DayOfWeek == DayOfWeek.Sunday) return true;

            if (repository.IsTollFreeDate(passageTime)) return true;

            if (passageTime.Month == 7) return true;

            return false;
        }
    }
}