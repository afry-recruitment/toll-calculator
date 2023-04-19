using System.ComponentModel.DataAnnotations.Schema;

namespace TollCalculator.Domain.Entities;

public class TollRatesEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public double StartTime { get; set; }
    public double EndTime { get; set; }
    public int Fee { get; set; } 
}
