using System.ComponentModel.DataAnnotations.Schema;

namespace TollCalculator.Domain.Entities;

public class TollFreeDaysEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Date { get; set; } = string.Empty;

}
