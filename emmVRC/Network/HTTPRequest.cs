using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Network;
using TinyJSON;

namespace emmVRC.Network
{
    public class HTTPRequest
    {
        public static string get_sync(string url)
        {
            return request(HttpMethod.Get, url).Result;
        }

        public static string post_sync(string url, object obj)
        {
            return request(HttpMethod.Post, url, obj).Result;
        }

        public static string put_sync(string url, object obj)
        {
            return request(HttpMethod.Put, url, obj).Result;
        }

        public static string delete_sync(string url, object obj)
        {
            return request(HttpMethod.Delete, url, obj).Result;
        }

        public static async Task<string> get(string url)
        {
            return await request(HttpMethod.Get, url);
        }

        public static async Task<string> post(string url, object obj) 
        {
            return await request(HttpMethod.Post, url, obj);
        }

        public static async Task<string> put(string url, object obj)
        {
            return await request(HttpMethod.Put, url, obj);
        }

        public static async Task<string> delete(string url, object obj)
        {
            return await request(HttpMethod.Delete, url, obj);
        }

        private static async Task<string> request(HttpMethod method, string url, object obj = null)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(method, url);

            if (obj != null)
            {
                string json = TinyJSON.Encoder.Encode(obj);
                requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            using (HttpResponseMessage responseMessage = NetworkClient.httpClient.SendAsync(requestMessage).Result)
            {
                if (responseMessage.IsSuccessStatusCode)
                {
                    return await responseMessage.Content.ReadAsStringAsync();
                }
                else if (responseMessage.StatusCode == HttpStatusCode.Unauthorized)
                {
                    emmVRCLoader.Logger.Log(responseMessage.Content.ToString());
                    emmVRCLoader.Logger.Log(await requestMessage.Content.ReadAsStringAsync());
                    //TODO: change to request token again
                    throw new Exception(responseMessage.ReasonPhrase);
                }
                else
                {
                    emmVRCLoader.Logger.Log("{0}{1}", responseMessage.StatusCode, responseMessage.Content);
                    throw new Exception(responseMessage.ReasonPhrase);
                }
            }
        }

    }
}
