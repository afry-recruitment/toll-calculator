public class TollFeeRequest
{
    public required IVehicle Vehicle { get; set; }

    public DateTime[] Passages { get; set; } = [];
}