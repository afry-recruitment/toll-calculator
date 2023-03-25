
namespace ConsoleClient.Repo
{
    public class ClassOne
    {
        private static readonly string apiUri = "https://THEURLUWANTTOCALL/api/v1/SomeValue";
        HttpClient HttpClient { get; }
        /*public async Task<List<Model>> GetData(string request) =>
            await DeserializeData(await HttpClient.GetAsync(apiUri + request));
        private async Task<List<Model>> DeserializeData(HttpResponseMessage responseMessage) =>
            responseMessage.IsSuccessStatusCode ?
            JsonConvert.DeserializeObject<List<Model>>(
                await responseMessage.Content.ReadAsStringAsync()) : null;
        public ApiRepo()
        {
            HttpClient = new HttpClient();
        }*/
    }
}