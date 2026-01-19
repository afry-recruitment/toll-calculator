using TollFeeCalculator.Enterprise.Entities;

namespace TollFeeCalculator.Presentation.Models
{
    public class GetTollFeeRequestModel
    {
        public IReadOnlyList<DateTime> DateTimes { get; set; } = null!;
        public Vehicle Vehicle { get; set; } = null!;
    }
}
