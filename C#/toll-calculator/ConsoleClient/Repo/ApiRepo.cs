
using DataLib.Enum;
using DataLib.Model;
using Newtonsoft.Json;

namespace ConsoleClient.Repo
{
    public class ApiRepo
    {
        private static readonly string apiUri = "https://localhost:7246/";
        HttpClient HttpClient { get; }
        public async Task<VehicleModel> GetData(Vehicles request) =>
            await DeserializeData(await HttpClient.GetAsync(apiUri + request));
      
        private async Task<VehicleModel> DeserializeData(HttpResponseMessage responseMessage) =>
            responseMessage.IsSuccessStatusCode ?
            JsonConvert.DeserializeObject<VehicleModel>(
                await responseMessage.Content.ReadAsStringAsync()) : null;

        public ApiRepo()
        {
            HttpClient = new HttpClient();
        }
    }
}