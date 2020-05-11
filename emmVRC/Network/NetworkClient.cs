using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Network;
using System.Net.Http;
using System.Net.Http.Headers;

namespace emmVRC.Network
{
    public static class NetworkClient
    {
        //TODO add caching
        private static string BaseAddress = "127.0.0.1";
        private static int Port = 3000;
        private static string _authToken;
        public static string authToken { 
            get { return _authToken; } 
            set
            {
                _authToken = value;
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authToken);
            } 
        }

        public static HttpClient httpClient { get; set; }

        public static void InitializeClient()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("emmVRC/1.0 (Client; emmVRCClient/)");
        }
    }
}
