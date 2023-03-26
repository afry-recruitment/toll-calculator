using interfaces;
using Microsoft.AspNetCore.Mvc;
using DataLib.Enum;
using System.Net.Mime;
using DataLib.Model;

namespace WebAPI.Controllers
{
    public class VehicleController// : IVehicle
    {
        public VehicleController()
        {
        }

        [HttpGet("{vehicle}")]
        public VehicleModel? GetVehicleType(Vehicles? vehicle)
        {
            if (vehicle == null) return null;

            var model = new VehicleModel
            {
                Id = Convert.ToInt32(vehicle),
                Type = Enum.GetName(typeof(Vehicles), vehicle)
            };
            return model;
            //return Results.Ok(model);
        }

    }
}