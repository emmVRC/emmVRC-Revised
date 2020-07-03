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
        public static string get_sync(string url)
        {
            try
            {
                return request(HttpMethod.Get, url).Result;
            }
            catch(Exception except)
            {
                throw except;
            }
        }

        public static string post_sync(string url, object obj)
        {
            try
            {
                return request(HttpMethod.Post, url, obj).Result;
            }
            catch(Exception except)
            {
                throw except;
            }
        }

        public static string put_sync(string url, object obj)
        {
            try
            {
                return request(HttpMethod.Put, url, obj).Result;
            }
            catch(Exception except)
            {
                throw except;
            }
        }

        public static string patch_sync(string url, object obj)
        {
            try { 
                return request(new HttpMethod("PATCH"), url, obj).Result;
            }
            catch (Exception except)
            {
                throw except;
            }
        }

        public static string delete_sync(string url, object obj)
        {
            try
            {
                return request(HttpMethod.Delete, url, obj).Result;
            }
            catch (Exception except)
            {
                throw except;
            }
        }

        public static async Task<string> get(string url)
        {
            try
            {
                return await request(HttpMethod.Get, url);
            }
            catch(Exception except)
            {
                throw except;
            }
        }

        public static async Task<string> post(string url, object obj)
        {
            try { 
                return await request(HttpMethod.Post, url, obj);
            }
            catch (Exception except)
            {
                throw except;
            }
        }

        public static async Task<string> put(string url, object obj)
        {
            try { 
                return await request(HttpMethod.Put, url, obj);
            }
            catch (Exception except)
            {
                throw except;
            }
        }

        public static async Task<string> patch(string url, object obj)
        {
            try { 
                return await request(new HttpMethod("PATCH"), url, obj);
            }
            catch (Exception except)
            {
                throw except;
            }
        }

        public static async Task<string> delete(string url, object obj)
        {
            try
            {
                return await request(HttpMethod.Delete, url, obj);
            }catch(Exception except)
            {
                throw except;
            }
        }

        private static async Task<string> request(HttpMethod method, string url, object obj = null)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(method, url);

            if (obj != null)
            {
                string json = TinyJSON.Encoder.Encode(obj);
                requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            return await Task.Run(() => {
                using (HttpResponseMessage responseMessage = NetworkClient.httpClient.SendAsync(requestMessage).Result)
                {
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        return responseMessage.Content.ReadAsStringAsync();
                    }
                    else if (responseMessage.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        //emmVRCLoader.Logger.Log(responseMessage.Content.ToString());
                        //emmVRCLoader.Logger.Log(requestMessage.Content.ReadAsStringAsync().Result);
                        //TODO: change to request token again
                        //throw new Exception(responseMessage.ReasonPhrase);
                        throw new Exception("unauthorized");
                    }
                    else if (responseMessage.StatusCode == HttpStatusCode.Forbidden)
                    {
                        throw new Exception("forbidden");
                    }
                    else
                    {
                        emmVRCLoader.Logger.LogDebug("{0}{1}", responseMessage.StatusCode, responseMessage.Content);
                        throw new Exception(responseMessage.ReasonPhrase);
                    }
                }
            });
            
        }

    }
}
