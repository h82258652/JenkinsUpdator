using Newtonsoft.Json;

namespace JenkinsUpdator.Core.Models
{
    public class GetInstalledPluginListResult
    {
        [JsonProperty("plugins")]
        public JenkinsPlugin[] Plugins { get; set; }
    }
}