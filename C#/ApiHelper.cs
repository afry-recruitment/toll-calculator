using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace afryCodeTest.toll_calculator.C_
{
    public static class ApiHelper
    {
        public static HttpClient APIClient { get; set; } = new HttpClient();

        public static void InitializeClient()
        {
            APIClient = new HttpClient
            {
                BaseAddress = new Uri("https://svenskahelgdagar.info/v2")
            };
            APIClient.DefaultRequestHeaders.Accept.Clear();
            APIClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static async Task<DataResponse> getToday()
        {
            using (var response = await APIClient.GetAsync("/today"))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<DataResponse>();
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
    public class DataResponse {
        public bool public_sunday { get; set; }
    }
}
