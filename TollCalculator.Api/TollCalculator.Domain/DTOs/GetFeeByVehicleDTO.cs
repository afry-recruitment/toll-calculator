using System.ComponentModel.DataAnnotations;

namespace TollCalculator.Domain.DTOs;

public class GetFeeByVehicleDTO
{
    [Required]
    public string LicensPlate { get; set; } = string.Empty;
    [Required]
    public string VehicleType { get; set; } = "Car";
}
