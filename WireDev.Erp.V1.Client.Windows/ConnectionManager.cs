using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace WireDev.Erp.V1.Client.Windows
{
    public class ConnectionManager
    {
        public HttpClient Client { get; set; }

        public ConnectionManager(Uri url, MediaTypeWithQualityHeaderValue mediaType)
        {
            Client = new()
            {
                BaseAddress = url
            };
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(mediaType);
        }

        public void SetToken(string token)
        {
            Client.DefaultRequestHeaders.Authorization = new("Bearer", token);
        }

        public async Task<bool> IsOnline()
        {
            HttpResponseMessage response = await Client.GetAsync("ping");
            return response.IsSuccessStatusCode;
        }
    }
}