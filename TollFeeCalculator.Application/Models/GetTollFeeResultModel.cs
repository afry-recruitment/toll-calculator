namespace TollFeeCalculator.Application.Models
{
    public class GetTollFeeResultModel
    {
        public int? Fee { get; set; }
        public Exception? Exception { get; set; }

        public GetTollFeeResultModel(int fee)
        {
            Fee = fee;
            Exception = default;
        }

        public GetTollFeeResultModel(Exception exception)
        {
            Fee = default;
            Exception = exception;
        }
    }
}
