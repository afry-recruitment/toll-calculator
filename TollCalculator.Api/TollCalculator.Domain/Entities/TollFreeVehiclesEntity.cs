using System.ComponentModel.DataAnnotations.Schema;

namespace TollCalculator.Domain.Entities;

public class TollFreeVehiclesEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Vehicle { get; set; } = string.Empty; 
}
