using System.Text;

namespace AiEntegrasyonProjesi.Controllers
{
    public class GoogleGenerativeAiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public GoogleGenerativeAiClient(string apiKey)
        {
            _httpClient = new HttpClient();
            _apiKey = apiKey;
        }

        public async Task<string> GenerateText(string prompt)
        {
            var requestUrl = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent?key={_apiKey}";

            var requestBody = new
            {
                prompt = new { text = prompt }
            };

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(requestUrl, content);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
    }
}
