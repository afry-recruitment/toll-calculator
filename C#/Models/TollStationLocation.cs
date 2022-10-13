namespace TollFeeCalculator.Models
{
    public class TollStationLocation : IModel
    {
        public int Id { get; init; } // Autoincrement in db.
        public string? StreetAddress { get; set; }
        public string? StreetNumber { get; set; }
        public double[] GeoCoordinates { get; set; }

        public TollStationLocation(double[] geoCoordinates, string streetAddress = null, string streetNumber = null)
        {
            this.GeoCoordinates = geoCoordinates;
            this.StreetAddress = streetAddress;
            this.StreetNumber = streetNumber;
        }
    }
}