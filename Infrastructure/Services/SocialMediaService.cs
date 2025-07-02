using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Interfaces.SocialMedia;

namespace Infrastructure.Services.SocialMedia
{
    public class SocialMediaService : ISocialMediaService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SocialMediaService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> ShareToPlatform(string endpoint, string token, string caption, string tag, string mediaUrl)
        {
            var client = _httpClientFactory.CreateClient();

            var requestData = new
            {
                postCaption = caption,
                tag = tag,
                media = mediaUrl
            };

            var content = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsync(endpoint, content);

            return response.IsSuccessStatusCode;
        }
    }
}
