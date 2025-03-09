using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;


//Checks external API(fake) pretending it's real vehicles. 
public class CarService
{
    private readonly HttpClient _httpClient;

    public CarService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
   
    public async Task<bool> VehicleExistsAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"https://freetestapi.com/api/v1/cars/{id}");

            // Returnerer true hvis API-et svarer med 200 OK og har innhold
            return response.IsSuccessStatusCode && response.Content.Headers.ContentLength > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error when checking for car {id}: {ex.Message}");
            return false;
        }
    }
}