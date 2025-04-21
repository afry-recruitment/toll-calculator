using AfryToll.Model.Dtos;
using AfryTollApp.Services.Contracts;
using System.Net.Http.Json;

namespace AfryTollApp.Services
{
    public class Categoryservice : ICategoryService
    {
        private readonly HttpClient httpClient;

        public Categoryservice(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<List<CategoryDto>> GetItems()
        {
            try
            {
                var response = await this.httpClient.GetAsync("api/Category");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<CategoryDto>().ToList();
                    }
                    return await response.Content.ReadFromJsonAsync<List<CategoryDto>>();
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

        public async Task<CategoryDto> AddItem(CategoryDto CategoryDto)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync<CategoryDto>("api/Category", CategoryDto);

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(CategoryDto);
                    }

                    return await response.Content.ReadFromJsonAsync<CategoryDto>();

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

        public async Task<List<CategoryDto>> GetstatusItems(bool Status)
        {
            try
            {
                var response = await this.httpClient.GetAsync($"api/Category/CategoryStatus/{Status}");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<CategoryDto>().ToList();
                    }
                    return await response.Content.ReadFromJsonAsync<List<CategoryDto>>();
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
        public async Task<CategoryDto> GetItem(int id)
        {
            try
            {
                var response = await this.httpClient.GetAsync($"api/Category/{id}");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return null;
                    }
                    return await response.Content.ReadFromJsonAsync<CategoryDto>();
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


    }
}
