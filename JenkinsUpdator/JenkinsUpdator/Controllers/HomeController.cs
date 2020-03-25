using JenkinsUpdator.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace JenkinsUpdator.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<HomeController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _logger = logger;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Update()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var bytes = await httpClient.GetByteArrayAsync("https://mirrors.tuna.tsinghua.edu.cn/jenkins/war/latest/jenkins.war");
            var jenkinsFolderPath = _configuration["JenkinsFolder"];
            var jenkinsFilePath = Path.Combine(jenkinsFolderPath, "jenkins.war");
            await System.IO.File.WriteAllBytesAsync(jenkinsFilePath, bytes);

            ViewData["Message"] = "更新完成，请重启 Jenkins";
            return View("Index");
        }
    }
}