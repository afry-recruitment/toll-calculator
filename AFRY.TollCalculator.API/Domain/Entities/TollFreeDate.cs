namespace AFRY.TollCalculator.API.Domain.Entities;

public class TollFreeDate
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public string Description { get; set; } = string.Empty;

}
