using System.Net.Http;
using System.Threading.Tasks;

namespace JenkinsUpdator.Core
{
    public class TsinghuaClient
    {
        private static readonly HttpClient Client = new HttpClient();

        public async Task<byte[]> DownloadPluginAsync(string pluginName)
        {
            var url = $"https://mirrors.tuna.tsinghua.edu.cn/jenkins/plugins/{pluginName}/latest/{pluginName}.hpi";
            var bytes = await Client.GetByteArrayAsync(url);
            return bytes;
        }
    }
}