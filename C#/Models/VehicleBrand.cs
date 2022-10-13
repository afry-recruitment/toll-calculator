namespace TollFeeCalculator.Models
{
    public class VehicleBrand : IModel
    {
        public int Id { get; init; } // Autoincrement in db.
        public string Name { get; set; }

        public VehicleBrand(string name)
        {
            this.Name = name;
        }
    }
}