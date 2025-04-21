using AfryToll.Model.Dtos;
using AfryTollApp.Services.Contracts;
using System.Net.Http.Json;

namespace AfryTollApp.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient httpClient;

        public UserService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<UserDto> GetUserItem(string UserName, string Password)
        {
            try
            {
                var response = await this.httpClient.GetAsync($"api/User/{UserName}/{Password}");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(UserDto);
                    }

                    return await response.Content.ReadFromJsonAsync<UserDto>();
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




        public async Task<UserAddDto> AddItem(UserAddDto UserAddDto)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync<UserAddDto>("api/User", UserAddDto);

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(UserAddDto);
                    }

                    return await response.Content.ReadFromJsonAsync<UserAddDto>();

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




    }
}
