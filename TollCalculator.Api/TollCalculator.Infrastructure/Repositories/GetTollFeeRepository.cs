using Application.Interfaces.GetTollFee;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TollCalculator.Domain.DTOs;
using TollCalculator.Domain.Entities;
using TollCalculator.Infrastructure.Context;

namespace TollCalculator.Infrastructure.Repositories;

public class GetTollFeeRepository : IGetTollFeeRepository
{
    private readonly InMemoryDbContext _inMemoryDbContext;
    public GetTollFeeRepository(InMemoryDbContext inMemoryDbContext)
    {
        _inMemoryDbContext = inMemoryDbContext; 
    }
    public async Task<string> GetTollFee(GetFeeByVehicleDTO getFeeDTO, CancellationToken cancellationToken)
    {
        var tollFreeDates = await _inMemoryDbContext.TollFreeDays.ToListAsync();
        if(tollFreeDates.Any(date => date.Date.Contains(DateTime.Now.ToString("yyyy/MM/dd"))) ||
           (DateTime.Today.DayOfWeek == DayOfWeek.Saturday || DateTime.Today.DayOfWeek == DayOfWeek.Sunday))
        {
            return "Vehicle got charged 0 kr in toll";
        }

        var isTollFreeVehicle = await _inMemoryDbContext.TollFreeVehicles.ToListAsync();
        if(isTollFreeVehicle.Any(vehicle => vehicle.Vehicle.Contains(getFeeDTO.VehicleType)))
            return "Vehicle got charged 0 kr in toll";

        var isTollFreeTime = await _inMemoryDbContext.TollRates.ToListAsync();
        var currentTime = ConvertTimeStringToInt(DateTime.Now.ToString("HH:mm:ss"));
        var setFee = isTollFreeTime.Where(time => currentTime >= time.StartTime && currentTime <= time.EndTime).Select(x => x.Fee).FirstOrDefault();

        if(setFee == 0)
            return "Vechile got charged 0 kr due to there being no toll rate";


        if(!_inMemoryDbContext.VehicleInformation.Any(licensPlate => licensPlate.LicensPlate.Contains(getFeeDTO.LicensPlate)))
        {
            _inMemoryDbContext.Add(new VehicleInformationEntity { LicensPlate = getFeeDTO.LicensPlate, TotalCost = setFee, LastTollCost = setFee, LastTollPassage = DateTime.Now });
            await _inMemoryDbContext.SaveChangesAsync();
            return $"Vechile's current toll charge for the day is {setFee} kr";
        }
        else
        {
            var currentVehicle = _inMemoryDbContext.VehicleInformation.Where(licensPlate => licensPlate.LicensPlate.Equals(getFeeDTO.LicensPlate)).First();
            var currentTimeStamp = DateTime.Now;
            
            if(currentVehicle.LastTollPassage < DateTime.Today)
            {
                currentVehicle.TotalCost = 0;
                currentVehicle.LastTollCost = 0;
                await _inMemoryDbContext.SaveChangesAsync();
            }

            if(currentVehicle.TotalCost == 60)
                return "Vehicle got charged 0 kr in toll";

            if(currentTimeStamp < currentVehicle.LastTollPassage.AddHours(1) && setFee <= currentVehicle.LastTollCost)
            {
                return "Vehicle got charged 0 kr in toll";
            }
            else if(currentTimeStamp < currentVehicle.LastTollPassage.AddHours(1) && setFee > currentVehicle.LastTollCost)
            {
                if(currentVehicle.TotalCost + setFee - currentVehicle.LastTollCost >= 60)
                {
                    currentVehicle.LastTollCost = setFee;
                    currentVehicle.TotalCost = 60;
                }
                else
                {
                    currentVehicle.TotalCost += setFee - currentVehicle.LastTollCost;
                    currentVehicle.LastTollCost = setFee;
                }
                currentVehicle.LastTollPassage = currentTimeStamp;
            }
            else
            {
                currentVehicle.LastTollPassage = currentTimeStamp;
                currentVehicle.LastTollCost = setFee;
                currentVehicle.TotalCost = setFee + currentVehicle.TotalCost >= 60 ? currentVehicle.TotalCost = 60 : setFee + currentVehicle.TotalCost;
            }
            await _inMemoryDbContext.SaveChangesAsync();
            return $"Vechile's current toll charge for the day is {currentVehicle.TotalCost} kr";
        }
    }

    //Storing times for toll fees in secound since midnight for ease of use and ease of database storage
    //This metods help change the current time from HH:MM:SS to seconds
    public static int ConvertTimeStringToInt(string timeString)
    {
        String[] timeParts = timeString.Split(":");

        int hrs = Int32.Parse(timeParts[0]);
        int min = Int32.Parse(timeParts[1]);
        int sec = Int32.Parse(timeParts[2]);

        int secondsFromMidnight = (hrs * 3600) + (min * 60) + sec;

        return secondsFromMidnight;
    }
}
