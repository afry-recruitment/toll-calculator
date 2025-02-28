using System.ComponentModel.DataAnnotations;

namespace TollFeeCalculator.Models.Request
{
    /// <summary>
    /// Current toll free request 
    /// </summary>
    public class TollRequest
    {
        /// <summary>
        /// Current vehicle related to toll request
        /// </summary>
        [Required]
        public Vehicle Vehicle { get; set; } = null!;

        /// <summary>
        /// All current passage/entry times for current vehicle
        /// </summary>
        [Required]
        public DateTime[] PassageTimes { get; set; } = null!;
    }
}
