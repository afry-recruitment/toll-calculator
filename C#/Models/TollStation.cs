namespace TollFeeCalculator.Models
{
    public class TollStation : IModel
    {
        public int Id { get; init; } // Autoincrement in db.
        public bool IsActive { get; set; } = true;
        public string Name { get; set; }
        public TollStationLocation Location { get; set; }

        public TollStation(string name, TollStationLocation location)
        {
            this.Name = name;
            this.Location = location;
        }

        public TollStation(string name, TollStationLocation location, bool isActive)
        {
            this.Name = name;
            this.Location = location;
            this.IsActive = isActive;
        }
    }
}