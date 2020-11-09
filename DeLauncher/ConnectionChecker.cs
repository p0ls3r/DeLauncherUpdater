using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Net.Http;

namespace DeLauncher
{
    public static class ConnectivityChecker
    {
        public enum ConnectionStatus
        {
            NotConnected,
            Connected
        }

        public static async Task<ConnectionStatus> CheckInternet()
        {
            HttpClient client = new HttpClient();

            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpResponseMessage response =  await client.GetAsync("https://github.com/"); 
                response.EnsureSuccessStatusCode();                

                return ConnectionStatus.Connected;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.Message); 
                Console.WriteLine(e.InnerException.Message);
                return ConnectionStatus.NotConnected;
            }
        }            
    }
}
