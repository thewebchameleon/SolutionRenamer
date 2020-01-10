using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SolutionRenamer.Blazor
{
    public static class HttpHelper
    {
        public static async Task<HttpResponseMessage> Get(IHttpClientFactory _httpClientFactory, string url)
        {
            var httpClient = _httpClientFactory.CreateClient(url);

            var httpRequest = new HttpRequestMessage()
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Get,
            };
            return await httpClient.SendAsync(httpRequest);
        }
    }
}
