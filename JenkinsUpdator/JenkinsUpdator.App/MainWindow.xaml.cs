using JenkinsUpdator.Core;
using JenkinsUpdator.Core.Models;
using System;
using System.Configuration;
using System.Windows;

namespace JenkinsUpdator.App
{
    public partial class MainWindow
    {
        private readonly JenkinsClient _client;

        public MainWindow()
        {
            InitializeComponent();

            var jenkinsAddress = new Uri(ConfigurationManager.AppSettings["JenkinsAddress"]);
            var username = ConfigurationManager.AppSettings["Username"];
            var apiToken = ConfigurationManager.AppSettings["ApiToken"];
            _client = new JenkinsClient(jenkinsAddress, username, apiToken);
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var result = await _client.GetInstalledPluginListAsync();
            PluginsDataGrid.ItemsSource = result.Plugins;
        }

        private async void RestartJenkinsButton_Click(object sender, RoutedEventArgs e)
        {
            await _client.RestartAsync();
            MessageBox.Show("重启指令发送成功");
        }

        private async void UpdatePluginButton_Click(object sender, RoutedEventArgs e)
        {
            var plugin = (JenkinsPlugin)((FrameworkElement)sender).DataContext;
            var bytes = await new TsinghuaClient().DownloadPluginAsync(plugin.ShortName);
            var fileName = $"{plugin.ShortName}.hpi";
            await _client.UploadPluginAsync(bytes, fileName);
            MessageBox.Show("更新插件成功，请重启 Jenkins");
        }
    }
}