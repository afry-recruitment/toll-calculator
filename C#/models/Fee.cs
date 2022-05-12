using System;
using System.Collections.Generic;
using System.Text;

namespace TollFeeCalculator.models
{
    class Fee
    {
        public IVehicle Vehicle { get; set; }
        public int FeeAmount { get; set; }
        public FeeDay FeeDay { get; set; }
        public FeeHour FeeHour { get; set; }
    }

    class FeeDay
    {
        public DateTime Day { get; set; }
        public int FeeAmount { get; set; }
    }
    class FeeHour
    {
        public DateTime Time { get; set; }
        public int FeeAmount { get; set; }
    }
}
