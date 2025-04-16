using FrontEnd.Helpers.Interfaces;

namespace FrontEnd.Helpers.Implementations
{
    public class ServiceRepository : IServiceRepository
    {
        public HttpClient Client { get; set; }

        public ServiceRepository(HttpClient _client, IConfiguration configuration)
        {
            Client = _client;
            string baseUrl = configuration.GetValue<string>("BackEnd:Url") ?? "";
            string apiKey = configuration.GetValue<string>("BackEnd:ApiKey") ?? "";
            Client.BaseAddress = new Uri(baseUrl);
            Client.DefaultRequestHeaders.Add("ApiKey", apiKey);
        }
        public HttpResponseMessage DeleteResponse(string url)
        {
            return Client.DeleteAsync(url).Result;
        }

        public HttpResponseMessage GetResponse(string url)
        {
            return Client.GetAsync(url).Result;
        }

        public HttpResponseMessage PostResponse(string url, object model)
        {
            return Client.PostAsJsonAsync(url, model).Result;
        }

        public HttpResponseMessage PutResponse(string url, object model)
        {
            return Client.PutAsJsonAsync(url, model).Result;
        }
    }
}