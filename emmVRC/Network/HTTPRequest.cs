using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Network;

namespace emmVRC.Network
{
    public class HTTPRequest
    {
        public static string get_sync(string url) =>
            request(HttpMethod.Get, url).Result;

        public static string post_sync(string url, object obj) =>
            request(HttpMethod.Post, url, obj).Result;

        public static string put_sync(string url, object obj) =>
            request(HttpMethod.Put, url, obj).Result;

        public static string patch_sync(string url, object obj) =>
             request(new HttpMethod("PATCH"), url, obj).Result;

        public static string delete_sync(string url, object obj) =>
             request(HttpMethod.Delete, url, obj).Result;

        public static async Task<string> get(string url) =>
             await request(HttpMethod.Get, url);

        public static async Task<string> post(string url, object obj) =>
             await request(HttpMethod.Post, url, obj);

        public static async Task<string> put(string url, object obj) =>
             await request(HttpMethod.Put, url, obj);

        public static async Task<string> patch(string url, object obj) =>
             await request(new HttpMethod("PATCH"), url, obj);

        public static async Task<string> delete(string url, object obj) =>
             await request(HttpMethod.Delete, url, obj);

        private static async Task<string> request(HttpMethod method, string url, object obj = null)
        {
            try
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(method, url);

                if (obj != null)
                {
                    string json = TinyJSON.Encoder.Encode(obj);
                    requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");
                }
                
                return await Task.Run(() =>
                {
                    using (HttpResponseMessage responseMessage = NetworkClient.httpClient.SendAsync(requestMessage).Result)
                    {
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            return responseMessage.Content.ReadAsStringAsync();
                        }
                        else
                        {
                            emmVRCLoader.Logger.LogDebug(responseMessage.ReasonPhrase);
                            return Task.FromResult(responseMessage.ReasonPhrase);
                        }
                    }
                });
            }
            catch (Exception conception)
            {
                return (conception.ToString());
            }

        }

    }
}