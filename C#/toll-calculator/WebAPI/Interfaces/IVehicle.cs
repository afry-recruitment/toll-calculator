using WebAPI.Enum;

namespace interfaces
{
    public interface IVehicle
    {
        IResult GetVehicleType(string id);
    }
}