
using DataLib.Enum;

namespace interfaces
{
    public interface IVehicle
    {
        IResult GetVehicleType(Vehicles? vehicle);
    }
}