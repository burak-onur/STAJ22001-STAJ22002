using AiEntegrasyonProjesi.Data;
using AiEntegrasyonProjesi.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq; 
using System.Net.Http;
using System.Threading.Tasks;

namespace AiEntegrasyonProjesi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AiController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly DbBaglanti _context;

        public AiController(IHttpClientFactory httpClientFactory, DbBaglanti context)
        {
            _httpClient = httpClientFactory.CreateClient("GoogleGenerativeAi");
            _context = context;
        }

        [HttpPost("ask")]
        public async Task<IActionResult> AskAi([FromBody] AiRequest request)
        {
            try
            {
                var requestBody = new
                {
                    contents = new[]
                    {
                        new
                        {
                            role = "user",
                            parts = new[]
                            {
                                new { text = request.Question }
                            }
                        }
                    }
                };

                var response = await _httpClient.PostAsJsonAsync("https://generativelanguage.googleapis.com/v1/models/gemini-pro:generateContent?key=AIzaSyD50u_uKwA_NDXwyDGoiNUZ6QCPRIVXanc", requestBody);

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();

                    // JSON verisini ayrıştırma
                    var jsonResponse = JObject.Parse(responseData);

                    // Yanıt içindeki "text" değerini bulma
                    var textValue = jsonResponse["candidates"]?[0]?["content"]?["parts"]?[0]?["text"]?.ToString();

                    // Yanıtı veritabanına kaydetme
                    var aiResponse = new AiResponse
                    {
                        Question = request.Question,
                        ResponseText = textValue, // Yalnızca "text" değerini kaydediyoruz
                        CreatedAt = DateTime.UtcNow
                    };

                    _context.AiResponse.Add(aiResponse);
                    await _context.SaveChangesAsync();

                    return Ok(textValue); // Yalnızca "text" değerini döndürüyoruz
                }

                return StatusCode((int)response.StatusCode, "Error while calling Gemini API");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }

    public class AiRequest
    {
        public string Question { get; set; }
    }
}
