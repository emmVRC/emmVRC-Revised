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
        public static Task<string> get(string url) =>
             request(HttpMethod.Get, url);

        public static Task<string> post(string url, object obj) =>
             request(HttpMethod.Post, url, obj);

        public static Task<string> put(string url, object obj) =>
             request(HttpMethod.Put, url, obj);

        public static Task<string> patch(string url, object obj) =>
             request(new HttpMethod("PATCH"), url, obj);

        public static Task<string> delete(string url, object obj) =>
             request(HttpMethod.Delete, url, obj);

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
                
                using (HttpResponseMessage responseMessage = await NetworkClient.httpClient.SendAsync(requestMessage))
                {
                    if (responseMessage.IsSuccessStatusCode)
                        return await responseMessage.Content.ReadAsStringAsync();

                    emmVRCLoader.Logger.LogDebug(responseMessage.ReasonPhrase);
                    return responseMessage.ReasonPhrase;
                }
            }
            catch (Exception conception)
            {
                return (conception.ToString());
            }

        }

    }
}