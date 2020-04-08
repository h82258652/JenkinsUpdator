using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using JenkinsUpdator.Models;

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

        public async Task<IActionResult> Index()
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(_configuration["Jenkins:UrlBase"]);

            var username = _configuration["Jenkins:Username"];
            var apiToken = _configuration["Jenkins:ApiToken"];
            var authorization = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{apiToken}"));
            httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Basic {authorization}");

            var json = await httpClient.GetStringAsync("/pluginManager/api/json?depth=1");
            var result = JsonSerializer.Deserialize<JenkinsApiResult>(json);
            var hasUpdatePlugins = result.Plugins.Where(temp => temp.HasUpdate).Select(temp => temp.ShortName).ToList();

            ViewData["InstalledPlugins"] = hasUpdatePlugins;

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