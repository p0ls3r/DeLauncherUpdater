using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace DeLauncher
{
    public static class ConnectivityChecker
    {
        public enum ConnectionStatus
        {
            NotConnected,
            LimitedAccess,
            Connected
        }

        public static ConnectionStatus CheckInternet()
        {
            try
            {
                IPHostEntry entry = Dns.GetHostEntry("github.com");
                IPHostEntry entry2 = Dns.GetHostEntry("dns.msftncsi.com");
                if (entry.AddressList.Length == 0 || entry2.AddressList.Length == 0)
                {
                    return ConnectionStatus.NotConnected;
                }
            }
            catch
            {
                return ConnectionStatus.NotConnected;
            }

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://www.msftncsi.com/ncsi.txt");
            try
            {
                HttpWebResponse responce = (HttpWebResponse)request.GetResponse();

                if (responce.StatusCode != HttpStatusCode.OK)
                {
                    return ConnectionStatus.LimitedAccess;
                }
                using (StreamReader sr = new StreamReader(responce.GetResponseStream()))
                {
                    if (sr.ReadToEnd().Equals("Microsoft NCSI"))
                    {
                        return ConnectionStatus.Connected;
                    }
                    else
                    {
                        return ConnectionStatus.LimitedAccess;
                    }
                }
            }
            catch
            {
                return ConnectionStatus.NotConnected;
            }
        }
    }
}
