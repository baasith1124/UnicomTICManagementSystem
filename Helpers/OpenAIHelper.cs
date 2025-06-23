using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UnicomTICManagementSystem.Helpers
{
    public static class OpenAIHelper
    {
        private static readonly string apiKey;
        private static readonly string endpoint = "https://openrouter.ai/api/v1/chat/completions";

        static OpenAIHelper()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            // IMPORTANT:
            // This API key is for academic testing only.
            apiKey = config["OpenRouter:ApiKey"];  // Note: changed to match new section
        }

        public static async Task<string> AskChatGPTAsync(string question)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
                httpClient.DefaultRequestHeaders.Add("HTTP-Referer", "https://unicomtic.local");
                httpClient.DefaultRequestHeaders.Add("X-Title", "UnicomTICBot");

                var requestBody = new
                {
                    // or "openai/gpt-3.5-turbo" (limited free)
                    model = "mistralai/mistral-7b-instruct", // or try "mistralai/mistral-7b-instruct"  (faster + free)
                    messages = new[]
                    {
                        new { role = "system", content = "You are a helpful assistant for students at Unicom TIC." },
                        new { role = "user", content = question }
                    }
                };

                var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(endpoint, content);
                var json = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw new Exception("❌ OpenRouter API failed: " + json);

                dynamic result = JsonConvert.DeserializeObject(json);
                return result.choices[0].message.content.ToString().Trim();
            }
        }
    }
}
