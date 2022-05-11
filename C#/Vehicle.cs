using System;

namespace TollFeeCalculator
{
    public interface Vehicle
    {
        public string LicensePlate { get; set; }
        String GetVehicleType();
        String GetLicensePlate() { return LicensePlate; }
    }
}