using System;

namespace TollFeeCalculator
{
    public interface IVehicle
    {
        public string LicensePlate { get; set; }
        String GetVehicleType();
        String GetLicensePlate() { return LicensePlate; }
    }
}