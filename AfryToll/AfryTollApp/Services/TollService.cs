using System.Net.Http.Json;

using System.Net.Http.Json;
using System.Text;
using AfryToll.Model.Dtos;
using AfryTollApp.Services.Contracts;

namespace AfryTollApp.Services
{
    public class TollService : ITollService
    {
        private readonly HttpClient httpClient;

        public TollService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<TollDto> GetItem(int id)
        {
            try
            {
                var response = await httpClient.GetAsync($"api/Toll/{id}");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(TollDto);
                    }

                    return await response.Content.ReadFromJsonAsync<TollDto>();
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

        public async Task<List<TollDto>> GetItems()
        {
            try
            {
                var response = await this.httpClient.GetAsync("api/Toll");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<TollDto>().ToList();
                    }

                    return await response.Content.ReadFromJsonAsync<List<TollDto>>();
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

        public async Task<TollToAddDto> AddItem(TollToAddDto TollToAddDto)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync<TollToAddDto>("api/Toll", TollToAddDto);

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(TollToAddDto);
                    }

                    return await response.Content.ReadFromJsonAsync<TollToAddDto>();

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

        public async Task<IEnumerable<TollCostDto>> GetTollCosts()
        {
            try
            {
                var response = await httpClient.GetAsync("api/Toll/GetTollCosts");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<TollCostDto>();
                    }
                    return await response.Content.ReadFromJsonAsync<IEnumerable<TollCostDto>>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http Status Code - {response.StatusCode} Message - {message}");
                }
            }
            catch (Exception)
            {
                //Log exception
                throw;
            }
        }

        public async Task<TollCostDto> GetTollCost(int CategoryId, string Time)
        {
            try
            {
                var response = await httpClient.GetAsync($"api/Toll/GetTollCost/{CategoryId}/{Time}");

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
        public async Task<List<TollDto>> GetUser(int userid)
        {
            try
            {
                var response = await this.httpClient.GetAsync($"api/Toll/{userid}");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<TollDto>().ToList();
                    }

                    return await response.Content.ReadFromJsonAsync<List<TollDto>>();
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
        public async Task<List<TollDto>> GetUserItem(int userid, string Date)
        {
            try
            {
                var response = await this.httpClient.GetAsync($"api/Toll/{userid}/{Date}");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<TollDto>().ToList();
                    }

                    return await response.Content.ReadFromJsonAsync<List<TollDto>>();
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
        public async Task<bool> UpdateUserCost(int TollId, int UserCost)
        {
            try
            {
                var content = JsonContent.Create(new UpdateUserCostDto { UserCost = UserCost });

                var response = await httpClient.PutAsync($"api/Toll/UpdateUserCost/{TollId}", content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http status code: {response.StatusCode}, message: {message}");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<TollDto> GetTolltime(int userid, string time)
        {
            var response = await httpClient.GetAsync($"api/Toll/GetTolltime/{userid}/{time}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return default(TollDto);
                }

                return await response.Content.ReadFromJsonAsync<TollDto>();
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http status code: {response.StatusCode} message: {message}");
            }
        }

        public async Task<int> GetTotalCostForDay(int userid, string date)
        {
            try
            {
                var response = await httpClient.GetAsync($"api/Toll/GetTotalCostForDay/{userid}/{date}");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return 0;
                    }

                    return await response.Content.ReadFromJsonAsync<int>();
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
    }
}
