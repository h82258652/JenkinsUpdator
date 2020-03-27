using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace JenkinsUpdator.Controllers
{
    public class PluginController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public PluginController(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            var pluginFolder = Path.Combine(_configuration["JenkinsFolder"], "plugins");
            var installedPlugins = Directory.GetFiles(pluginFolder, "*.jpi").Select(Path.GetFileNameWithoutExtension).ToList();

            ViewData["InstalledPlugins"] = installedPlugins;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Update(string plugin)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var bytes = await httpClient.GetByteArrayAsync($"https://mirrors.tuna.tsinghua.edu.cn/jenkins/plugins/{plugin}/latest/{plugin}.hpi");
            var pluginFolder = Path.Combine(_configuration["JenkinsFolder"], "plugins");
            var filePath = Path.Combine(pluginFolder, $"{plugin}.jpi");
            await System.IO.File.WriteAllBytesAsync(filePath, bytes);

            ViewData["Message"] = "更新插件完成，请重启 Jenkins";
            return RedirectToAction("Index");
        }
    }
}