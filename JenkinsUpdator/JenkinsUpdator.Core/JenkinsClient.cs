using IdentityModel.Client;
using JenkinsUpdator.Core.Models;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace JenkinsUpdator.Core
{
    public class JenkinsClient : HttpClient
    {
        public JenkinsClient(Uri jenkinsAddress, string username, string apiToken)
        {
            if (username == null)
            {
                throw new ArgumentNullException(nameof(username));
            }

            if (apiToken == null)
            {
                throw new ArgumentNullException(nameof(apiToken));
            }

            BaseAddress = jenkinsAddress;
            this.SetBasicAuthentication(username, apiToken);
        }

        public async Task<GetInstalledPluginListResult> GetInstalledPluginListAsync()
        {
            var json = await GetStringAsync("/pluginManager/api/json?depth=1");
            return JsonConvert.DeserializeObject<GetInstalledPluginListResult>(json);
        }

        public async Task RestartAsync()
        {
            var response = await PostAsync("/safeRestart", null);
            Debug.Assert(response.StatusCode == HttpStatusCode.ServiceUnavailable);
        }

        public async Task UploadPluginAsync(byte[] plugin, string fileName)
        {
            if (plugin == null)
            {
                throw new ArgumentNullException(nameof(plugin));
            }

            using (var content = new MultipartFormDataContent())
            {
                content.Add(new ByteArrayContent(plugin), "name", fileName);
                var response = await PostAsync("/pluginManager/uploadPlugin", content);
                response.EnsureSuccessStatusCode();
            }
        }
    }
}