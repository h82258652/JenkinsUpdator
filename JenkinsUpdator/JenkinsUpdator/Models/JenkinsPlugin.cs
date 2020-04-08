using System.Text.Json.Serialization;

namespace JenkinsUpdator.Models
{
    public class JenkinsPlugin
    {
        [JsonPropertyName("hasUpdate")]
        public bool HasUpdate { get; set; }

        [JsonPropertyName("shortName")]
        public string ShortName { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }
    }
}