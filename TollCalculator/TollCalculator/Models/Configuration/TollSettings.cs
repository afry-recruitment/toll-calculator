using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("TollFeeCalculator.Test")]

namespace TollFeeCalculator.Models.Configuration
{
    internal class TollSettings
    {
        public int? MaxFee { get; set; }
        public TollFreeSettings? TollFree { get; set; }

        public Dictionary<int, string>? FeeAmounts { get; set; }

        public Dictionary<int, IEnumerable<TollHour>>? Fees
        {
            get
            {
                if(FeeAmounts != null)
                {
                    var fees = new Dictionary<int, IEnumerable<TollHour>>();
                    foreach (var feeAmount in FeeAmounts)
                    {
                        if(!string.IsNullOrWhiteSpace(feeAmount.Value))
                        {
                            var feeTimespans = new List<TollHour>();
                            var allFeeSpans = feeAmount.Value.Split(',');
                            foreach(var span in allFeeSpans)
                            {
                                if(!string.IsNullOrWhiteSpace(span))
                                {
                                    var startAndEnd = span.Split('-');
                                    var startHourAndMinute = startAndEnd.FirstOrDefault();
                                    var endHourAndMinute = startAndEnd.LastOrDefault();

                                    if (!string.IsNullOrWhiteSpace(startHourAndMinute) &&
                                        !string.IsNullOrWhiteSpace(endHourAndMinute))
                                    {
                                        var startHourAndMinuteValues = startHourAndMinute.Split(':');
                                        var endHourAndMinuteValues = endHourAndMinute.Split(':');

                                        if (int.TryParse(startHourAndMinuteValues.First(), out int startHour) &&
                                            int.TryParse(startHourAndMinuteValues.Last(), out int startMinutes) &&
                                            int.TryParse(endHourAndMinuteValues.First(), out int endHour) &&
                                             int.TryParse(endHourAndMinuteValues.Last(), out int endMinutes))
                                        {
                                            feeTimespans.Add(new TollHour(startHour, startMinutes, endHour, endMinutes, feeAmount.Key));
                                        }
                                    }
                                }
                            }
                            
                            if(!fees.ContainsKey(feeAmount.Key))
                            {
                                fees.Add(feeAmount.Key, feeTimespans);
                            }
                        }
                    }

                    return fees;
                }

                return null;
            }
        }
    }
}