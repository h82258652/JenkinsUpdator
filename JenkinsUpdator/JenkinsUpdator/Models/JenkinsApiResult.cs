using System.Text.Json.Serialization;

namespace JenkinsUpdator.Models
{
    public class JenkinsApiResult
    {
        [JsonPropertyName("plugins")]
        public JenkinsPlugin[] Plugins { get; set; }
    }
}