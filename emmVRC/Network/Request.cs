using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using emmVRCLoader;
using Encoder = emmVRC.TinyJSON.Encoder;

namespace emmVRC.Network
{
    public class Request
    {
        private const string RoutePrefix = "/api/v2";

        public static HttpMethod Patch = new HttpMethod("PATCH");

        public static async Task<(HttpStatusCode, string)> AttemptRequest(HttpMethod httpMethod, string requestUrl,
            object @object = null, bool removePrefix = false)
        {
            try
            {
                var requestMessage = !removePrefix ? new HttpRequestMessage(httpMethod, RoutePrefix + requestUrl) 
                    : new HttpRequestMessage(httpMethod, requestUrl);

                if (@object != null)
                    requestMessage.Content = new StringContent(Encoder.Encode(@object), Encoding.UTF8,
                        "application/json");

                // TODO Remove static HttpClient
                using (var responseMessage = await NetworkClient.httpClient.SendAsync(requestMessage))
                {
                    var (statusCode, response) = (responseMessage.StatusCode, await responseMessage.Content.ReadAsStringAsync());
                    #if DEBUG
                    Logger.Log($"{statusCode} | {response}");
                    #endif
                    return (statusCode, response);
                }
            }
            catch (Exception ex)
            {
                return (HttpStatusCode.ServiceUnavailable, null);
            }
        }
    }
}