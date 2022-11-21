namespace TollFeeCalculator.TollFees
{
    internal class TollFeeRepositoryFactory
    {
        public static ITollFeeRepository GetNewTollFeeRepository()
        {
            return new TollFeeRepository();
        }
    }
}
