using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Application.Utils
{
    public class SnakePredict
    {
        public string Label { get; set; } = string.Empty;
        public double Prob { get; set; }
    }

    public class SnakePredictor
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        private static readonly JsonSerializerOptions JsonOpts = new()
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public SnakePredictor(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _apiUrl = configuration["SnakePredictor:Uri"];
        }

        public async Task<SnakePredict> PredictAsync(string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
                throw new ArgumentException("Image URL must be provided.", nameof(imageUrl));

            // Build payload
            var payload = new PredictRequest { Url = imageUrl, TopK = 1 };
            var json = JsonSerializer.Serialize(payload, JsonOpts);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Build request
            var request = new HttpRequestMessage(HttpMethod.Post, _apiUrl);
            request.Headers.Accept.Add(new("application/json"));
            request.Content = content;

            // Send
            var response = await _httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"SnakePredict API Response: {response.StatusCode} - {responseContent}");

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"Predict API failed ({(int)response.StatusCode}): {responseContent}");

            var dto = JsonSerializer.Deserialize<PredictResponse>(responseContent, JsonOpts)
                      ?? throw new InvalidOperationException("Empty or invalid JSON from Predict API.");

            var raw = dto.Top1 ?? dto.Top?.FirstOrDefault()?.Label;
            var prob = dto.Top1Prob != 0 ? dto.Top1Prob : dto.Top?.FirstOrDefault()?.Prob ?? 0;

            if (string.IsNullOrWhiteSpace(raw))
                throw new InvalidOperationException("Predict API did not include a top-1 label.");

            return new SnakePredict
            {
                Label = FormatLabel(raw), 
                Prob = prob
            };
        }
        private static string FormatLabel(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return string.Empty;

            // Replace underscores with spaces, ensure Genus is Capitalized and species epithets are lowercase.
            var parts = raw.Split('_', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) return raw.Replace('_', ' ');

            parts[0] = Capitalize(parts[0]);
            for (int i = 1; i < parts.Length; i++)
                parts[i] = parts[i].ToLowerInvariant();

            return string.Join(' ', parts);
        }

        private static string Capitalize(string s)
            => string.IsNullOrEmpty(s) ? s : char.ToUpperInvariant(s[0]) + s.Substring(1).ToLowerInvariant();

        private sealed class PredictRequest
        {
            [JsonPropertyName("url")]
            public string Url { get; set; } = string.Empty;

            [JsonPropertyName("top_k")]
            public int TopK { get; set; } = 1;
        }

        private sealed class PredictResponse
        {
            [JsonPropertyName("top1")] public string? Top1 { get; set; }
            [JsonPropertyName("top1_prob")] public double Top1Prob { get; set; }
            [JsonPropertyName("top")] public TopItem[]? Top { get; set; }

            public sealed class TopItem
            {
                [JsonPropertyName("label")] public string Label { get; set; } = string.Empty;
                [JsonPropertyName("prob")] public double Prob { get; set; }
            }
        }
    }
}
