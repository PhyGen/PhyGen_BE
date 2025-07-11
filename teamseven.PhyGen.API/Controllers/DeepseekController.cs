using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using teamseven.PhyGen.Services.Object.Requests;
using teamseven.PhyGen.Services.Object.Responses;

namespace teamseven.PhyGen.API.Controllers
{
    [ApiController]
    [Route("api/deepseek")]
    [Produces("application/json")]
    public class DeepSeekController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<DeepSeekController> _logger;
        private readonly string _apiKey;
        private readonly string _baseUrl;

        public DeepSeekController(IConfiguration configuration, IHttpClientFactory httpClientFactory, ILogger<DeepSeekController> logger)
        {
            _logger = logger;
            _apiKey = configuration["DeepSeek:ApiKey"] ?? throw new ArgumentNullException("DeepSeek:ApiKey is not configured.");
            _baseUrl = configuration["DeepSeek:BaseUrl"] ?? "https://api.deepseek.com";
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(600); // Increased timeout for stability
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private async Task<string> CallDeepSeekApiAsync(object requestBody)
        {
            var jsonBody = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/chat/completions", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("DeepSeek API error: {StatusCode}, {Error}", response.StatusCode, errorContent);
                throw new HttpRequestException($"Error calling DeepSeek API: {response.StatusCode}, Details: {errorContent}");
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("DeepSeek API response: {ResponseBody}", responseBody);

            using var jsonDoc = JsonDocument.Parse(responseBody);
            if (jsonDoc.RootElement.TryGetProperty("choices", out var choices) && choices.GetArrayLength() > 0)
            {
                return jsonDoc.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString() ?? string.Empty;
            }

            return string.Empty; // Return empty if no choices
        }

        [HttpPost("solve")]
        [SwaggerOperation(Summary = "Solve physics problem with DeepSeek API", Description = "Sends a physics problem to DeepSeek API for detailed step-by-step solution. Falls back to chat model if reasoner returns empty.")]
        [SwaggerResponse(200, "Physics solution retrieved successfully.", typeof(DeepSeekChatResponse))]
        [SwaggerResponse(400, "Invalid request data.", typeof(object))]
        [SwaggerResponse(500, "Error occurred while calling DeepSeek API.", typeof(object))]
        public async Task<IActionResult> Solve([FromBody] DeepSeekChatRequest request)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(request.Message))
            {
                _logger.LogWarning("Invalid request data for DeepSeek solve.");
                return BadRequest(new { Message = "Message is required." });
            }

            try
            {
                // Prompt for reasoner model
                var prompt = $@"You are an expert physicist solving a problem step by step using Chain of Thought reasoning. For the given problem, follow these steps:
1. Restate the problem clearly in English.
2. List all given data and assumptions.
3. Derive the necessary formulas (e.g., for bright fringe positions in Young's experiment).
4. Solve step by step, explaining each calculation.
5. Verify the solution against all conditions (e.g., number of coinciding fringes, total fringes).
6. Provide the final answer in LaTeX format within \boxed{{}}.
Ensure all mathematical expressions are in LaTeX (e.g., \lambda, k, m) and use clear, concise English.

Problem: {request.Message}";

                var requestBody = new
                {
                    model = "deepseek-reasoner",
                    messages = new[]
                    {
                        new { role = "user", content = prompt }
                    },
                    max_tokens = 4096, // Increased for complex problems
                    temperature = 0.5, // Lowered for more consistent output
                    stream = false // Explicitly disable streaming
                };

                _logger.LogInformation("DeepSeek API solve request body: {RequestBody}", JsonSerializer.Serialize(requestBody));

                var responseText = await CallDeepSeekApiAsync(requestBody);

                // Fallback to chat model if response is empty
                if (string.IsNullOrWhiteSpace(responseText))
                {
                    _logger.LogWarning("Reasoner model returned empty response, falling back to chat model.");

                    var fallbackPrompt = $"Solve this physics problem step by step: {request.Message}";
                    var fallbackBody = new
                    {
                        model = "deepseek-chat",
                        messages = new[]
                        {
                            new { role = "user", content = fallbackPrompt }
                        },
                        max_tokens = 2000,
                        temperature = 0.7
                    };

                    responseText = await CallDeepSeekApiAsync(fallbackBody);
                }

                var result = new DeepSeekChatResponse { Response = responseText };
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing DeepSeek API solve request.");
                return StatusCode(500, new { Message = "An error occurred while processing the request." });
            }
        }

        [HttpPost("latex")]
        [SwaggerOperation(Summary = "Get LaTeX code for physics problem solution", Description = "Sends a physics problem to DeepSeek API and returns only the LaTeX code for the solution. Falls back to chat model if reasoner returns empty.")]
        [SwaggerResponse(200, "LaTeX code retrieved successfully.", typeof(DeepSeekChatResponse))]
        [SwaggerResponse(400, "Invalid request data.", typeof(object))]
        [SwaggerResponse(500, "Error occurred while calling DeepSeek API.", typeof(object))]
        public async Task<IActionResult> Latex([FromBody] DeepSeekChatRequest request)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(request.Message))
            {
                _logger.LogWarning("Invalid request data for DeepSeek latex.");
                return BadRequest(new { Message = "Message is required." });
            }

            try
            {
                // Prompt for reasoner model
                var prompt = $@"You are an expert physicist solving a problem using mathematical derivations. For the given problem, provide ONLY the LaTeX code for the solution, including:
- All necessary formulas (e.g., bright fringe positions in Young's experiment).
- Step-by-step derivations using LaTeX math mode (e.g., \begin{{align*}} ... \end{{align*}}).
- The final answer in \boxed{{}}.
Do NOT include any explanatory text, comments, or natural language descriptions. Use standard LaTeX syntax (e.g., \lambda, k, m).

Problem: {request.Message}";

                var requestBody = new
                {
                    model = "deepseek-reasoner",
                    messages = new[]
                    {
                        new { role = "user", content = prompt }
                    },
                    max_tokens = 4096, // Increased for complex problems
                    temperature = 0.5, // Lowered for more consistent output
                    stream = false // Explicitly disable streaming
                };

                _logger.LogInformation("DeepSeek API latex request body: {RequestBody}", JsonSerializer.Serialize(requestBody));

                var responseText = await CallDeepSeekApiAsync(requestBody);

                // Fallback to chat model if response is empty
                if (string.IsNullOrWhiteSpace(responseText))
                {
                    _logger.LogWarning("Reasoner model returned empty response, falling back to chat model.");

                    var fallbackPrompt = $"Provide only the LaTeX code to solve this physics problem: {request.Message}";
                    var fallbackBody = new
                    {
                        model = "deepseek-chat",
                        messages = new[]
                        {
                            new { role = "user", content = fallbackPrompt }
                        },
                        max_tokens = 2000,
                        temperature = 0.7
                    };

                    responseText = await CallDeepSeekApiAsync(fallbackBody);
                }

                var result = new DeepSeekChatResponse { Response = responseText };
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing DeepSeek API latex request.");
                return StatusCode(500, new { Message = "An error occurred while processing the request." });
            }
        }
    }
}