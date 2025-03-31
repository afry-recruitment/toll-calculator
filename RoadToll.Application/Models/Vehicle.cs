namespace RoadToll.Application.Models
{
    public class Vehicle
    {
        public static Vehicle Car = new("Car", "Car", false);
        public static Vehicle Motorbike = new("Motorbike", "Motorbike", true);
        public static Vehicle Tractor = new("Tractor", "Tractor", true);
        public static Vehicle Emergency = new("Emergency", "Emergency vehicle", true);
        public static Vehicle Diplomat = new("Diplomat", "Diplomatic vehicle", true);
        public static Vehicle Foreign = new("Foreign", "Foreign vehicle", true);

        protected Vehicle(string key, string displayName, bool isTollFreeVehicle)
        {
            Key = key;
            DisplayName = displayName;
            IsTollFreeVehicle = isTollFreeVehicle;
        }

        public string Key { get; }
        public string DisplayName { get; }
        public bool IsTollFreeVehicle { get; }
    }
}
