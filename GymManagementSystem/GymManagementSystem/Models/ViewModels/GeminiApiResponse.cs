using Newtonsoft.Json;
using System.Collections.Generic;

namespace GymManagementSystem.Models.ViewModels
{
    public class GeminiResponse
    {
        [JsonProperty("candidates")]
        public List<GeminiCandidate> Candidates { get; set; }
    }

    public class GeminiCandidate
    {
        [JsonProperty("content")]
        public GeminiContent Content { get; set; }
    }

    public class GeminiContent
    {
        [JsonProperty("parts")]
        public List<GeminiPart> Parts { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }
    }

    public class GeminiPart
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}