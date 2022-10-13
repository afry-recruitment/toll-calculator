namespace TollFeeCalculator.Models
{
    public class VehicleModel : IModel
    {
       public int Id { get; init; } // Autoincrement in db.
        public string Name { get; set; }

        public VehicleModel(string name)
        {
            this.Name = name;
        }
    }
}