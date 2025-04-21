using System.Net.Http.Json;

using System.Net.Http.Json;
using System.Text;
using AfryToll.Model.Dtos;
using AfryTollApp.Services.Contracts;

namespace AfryTollApp.Services
{
    public class TollCostService : ITollCostService
    {
        private readonly HttpClient httpClient;

        public TollCostService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<List<TollCostDto>> GetItems()
        {
            try
            {
                var response = await this.httpClient.GetAsync("api/TollCost");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<TollCostDto>().ToList();
                    }
                    return await response.Content.ReadFromJsonAsync<List<TollCostDto>>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http status code: {response.StatusCode} message: {message}");
                }

            }
            catch (Exception)
            {
                //Log exception
                throw;
            }
        }

        public async Task<TollCostAddDto> AddItem(TollCostAddDto TollCostAddDto)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync<TollCostAddDto>("api/TollCost", TollCostAddDto);

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(TollCostAddDto);
                    }

                    return await response.Content.ReadFromJsonAsync<TollCostAddDto>();

                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http status:{response.StatusCode} Message -{message}");
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<TollCostDto>> GetCategorItem(int CategoryId)
        {
            try
            {
                var response = await this.httpClient.GetAsync($"api/TollCost/Category/{CategoryId}");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<TollCostDto>().ToList();
                    }

                    return await response.Content.ReadFromJsonAsync<List<TollCostDto>>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http status code: {response.StatusCode} message: {message}");
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<TollCostDto> GetItem(int id)
        {
            try
            {
                var response = await httpClient.GetAsync($"api/TollCost/{id}");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(TollCostDto);
                    }

                    return await response.Content.ReadFromJsonAsync<TollCostDto>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http status code: {response.StatusCode} message: {message}");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<TollCostDto> DeleteItem(int id)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"api/TollCost/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<TollCostDto>();
                }
                return default(TollCostDto);
            }
            catch (Exception)
            {
                //Log exception
                throw;
            }
        }

    }
}