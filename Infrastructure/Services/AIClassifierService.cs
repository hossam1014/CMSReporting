using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Abstractions;
using Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    public class AIClassifierService : IAIClassifierService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<AIClassifierService> _logger;
        private readonly string _apiUrl = "https://citioai-webapp.azurewebsites.net/classifier";

        public AIClassifierService(IHttpClientFactory httpClientFactory, ILogger<AIClassifierService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<Result<ClassificationResult>> ClassifyTextAsync(string text)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                
                var requestData = new { text = text };
                var content = new StringContent(
                    JsonSerializer.Serialize(requestData),
                    Encoding.UTF8,
                    "application/json");

                var response = await client.PostAsync(_apiUrl, content);
                
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("AI Classifier API returned status code: {StatusCode}", response.StatusCode);
                    return Result.Failure<ClassificationResult>(new Error("ClassificationFailed", $"Classification failed with status code: {response.StatusCode}"));
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                
                try
                {
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var classificationResult = JsonSerializer.Deserialize<ClassificationResult>(responseContent, options);
                    
                    if (classificationResult == null)
                    {
                        return Result.Failure<ClassificationResult>(new Error("DeserializationFailed", "Failed to deserialize classification result"));
                    }
                    
                    return Result.Success(classificationResult);
                }
                catch (JsonException ex)
                {
                    _logger.LogError(ex, "Error deserializing classification response: {Response}", responseContent);
                    return Result.Failure<ClassificationResult>(new Error("JsonParsingError", $"Error parsing classification response: {ex.Message}"));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while calling AI Classifier API");
                return Result.Failure<ClassificationResult>(new Error("ClassifierApiError", $"Classification error: {ex.Message}"));
            }
        }
    }
} 