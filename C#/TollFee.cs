namespace TollFeeCalculator
{
    internal class TollFee
    {
        private readonly int hour;
        private readonly int minute;
        private readonly int fee;

        internal TollFee(int hour, int minute, int fee)
        {
            this.hour = hour;
            this.minute = minute;
            this.fee = fee;
        }

        public bool TryGetFee(DateTime date, out int fee)
        {
            fee = this.fee;
            return date.Hour < hour || date.Hour == hour && date.Minute < minute;
        }
    }
}