using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TollFeeCalculator.models
{
    class Fee 
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong Id { get; set; }
        public Vehicle Vehicle { get; set; }
        public int FeeAmount { get; set; }
        public FeeDay FeeDay { get; set; }
        public FeeHour FeeHour { get; set; }
    }

    class FeeDay
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong id { get; set; }
        public DateTime Day { get; set; }
        public int FeeAmount { get; set; }
    }
    class FeeHour
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong id { get; set; }
        public DateTime Time { get; set; }
        public int FeeAmount { get; set; }
    }
}
