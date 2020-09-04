using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Collections.Generic;

namespace DeLauncher
{
    static class UpdatesLoader
    {
        const string repo = "p0ls3r/DeLauncher";
        const string tempPrefix = "temp";

        public static async Task<int> GetLatestVersionNumber()
        {
            Console.WriteLine("Checking version...");

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.UserAgent.Add(
                new ProductInfoHeaderValue("DeLauncher", typeof(Updater).Assembly.GetName().Version.ToString()));

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                        
            var contentsUrl = $"https://api.github.com/repos/{repo}/contents";

            var contentsJson = await httpClient.GetStringAsync(contentsUrl);

            dynamic contents = new JavaScriptSerializer().DeserializeObject(contentsJson);
            
            foreach (var data in contents)
            {
                var parsedData = (Dictionary<string, object>)data;

                var fileName = (string)data["name"];

                if (fileName == Updater.VersionFileName)
                {
                    var downloadUrl = (string)data["download_url"];
                    var myWebClient = new WebClient();
                    var name = tempPrefix + fileName;

                    using (WebClient client = new WebClient())
                        client.DownloadFile(downloadUrl, name);

                    var newVersionNumber = XmlVersionReader.GetVersionFromXml(name);

                    File.Delete(name);

                    return newVersionNumber;
                }
            }
            throw new ApplicationException("version file not found!");
        }

        public static async Task DownloadUpdate()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.UserAgent.Add(
                new ProductInfoHeaderValue("DeLauncher", typeof(Updater).Assembly.GetName().Version.ToString()));

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            var contentsUrl = $"https://api.github.com/repos/{repo}/contents";

            var contentsJson = await httpClient.GetStringAsync(contentsUrl);

            dynamic contents = new JavaScriptSerializer().DeserializeObject(contentsJson);

            using (WebClient client = new WebClient())
            {
                foreach (var data in contents)
                {
                    var parsedData = (Dictionary<string, object>)data;

                    if ((string)parsedData["type"] == "file")
                    {
                        var downloadUrl = (string)parsedData["download_url"];
                        Console.WriteLine($"Download: {downloadUrl}");
                        var fileName = (string)parsedData["name"];

                        if (fileName == Updater.VersionFileName)
                            client.DownloadFile(new Uri(downloadUrl), Updater.LauncherFolder + fileName);
                        else
                            client.DownloadFile(new Uri(downloadUrl), fileName);
                    }
                }
            }
        }
    }
}
