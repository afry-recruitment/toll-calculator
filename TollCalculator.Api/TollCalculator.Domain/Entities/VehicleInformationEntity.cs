using System.ComponentModel.DataAnnotations.Schema;

namespace TollCalculator.Domain.Entities;

public class VehicleInformationEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string LicensPlate { get; set; } = string.Empty;
    public int TotalCost { get; set; }
    public DateTime LastTollPassage { get; set; }
    public int LastTollCost { get; set; }
}
