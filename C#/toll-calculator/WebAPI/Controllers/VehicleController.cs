using interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using WebAPI.DataStrings;
using WebAPI.Enum;

namespace WebAPI.Controllers
{
    public class VehicleController : IVehicle
    {
        public VehicleController()
        {
        }

        [Route("/GetVehicleType/{id}")]
        [HttpGet]
        public IResult GetVehicleType([FromRoute] string id)
        {
            Regex regex = new Regex(RegexStrings.RegexDoesStringContainNumbers);
            if (!regex.IsMatch(id))           //fix issue, if id doesnt excist in
                return Results.BadRequest(); //vehicles it will be set to car. Causes problems

            var currentVehicle = System.Enum.GetName(typeof(Vehicles), int.Parse(id));

            if (currentVehicle is null)
                return Results.NotFound();

            return Results.Ok(currentVehicle);
        }
    }
}