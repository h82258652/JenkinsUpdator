using Newtonsoft.Json;

namespace JenkinsUpdator.Core.Models
{
    public class JenkinsPlugin
    {
        [JsonProperty("hasUpdate")]
        public bool HasUpdate { get; set; }

        [JsonProperty("shortName")]
        public string ShortName { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }
    }
}