using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using eScratchLottery.Server.WebApi.Messages;

namespace eScratchLottery.Server.Test
{
    public static class HttpExtension
    {
        private static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public static async Task<T> ReadAsync<T>(this HttpContent content)
        {
            var data = await content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(data, SerializerOptions);
        }

        public static Task<HttpResponseMessage> PostAsync<T>(this HttpClient client, string requestUri, T data)
        {
            var json = JsonSerializer.Serialize(data, SerializerOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            return client.PostAsync(requestUri, content);
        }

        public static async Task<HttpResponseMessage> RevealCard(this HttpClient client, int cardId, string playerName)
        {
            return await PostAsync(client, "api/cards", new RevealCardRequest
            {
                CardId = cardId,
                PlayerName = playerName
            });
        }
    }
}
