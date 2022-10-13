namespace TollFeeCalculator.Models
{
    public class Address : IModel
    {
        public int Id { get; init; }
        public string StreetAddress { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public Address(string streetAddress, string postalCode, string city)
        {
            StreetAddress = streetAddress;
            PostalCode = postalCode;
            City = city;
        }
    }
}