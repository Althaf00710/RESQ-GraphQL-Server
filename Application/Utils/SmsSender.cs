using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Application.Utils
{
    public class SmsSender
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;
        private readonly string _apiToken;
        private readonly string _senderId;

        public SmsSender(HttpClient httpClinet, IConfiguration configuration)
        {
            _httpClient = httpClinet;
            _apiToken = configuration["ApiKey:SmsSender"];
            _apiUrl = configuration["SmsGateway:SenderUri"];
            _senderId = configuration["SmsGateway:SenderId"];
        }

        public async Task<bool> SendSmsAsync(string phoneNumber, string message)
        {
            var payload = new
            {
                api_token = _apiToken,
                recipient = phoneNumber,
                sender_id = _senderId,
                type = "plain",
                message
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, _apiUrl);
            request.Headers.Accept.Add(new("application/json"));
            request.Content = content;

            var response = await _httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"SMS API Response: {response.StatusCode} - {responseContent}");

            return response.IsSuccessStatusCode;
        }
    }
}
