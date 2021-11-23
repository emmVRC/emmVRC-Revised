using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Encoder = emmVRC.TinyJSON.Encoder;

namespace emmVRC.Network
{
    public class Request
    {
        public static async Task<(HttpStatusCode, string)> AttemptRequest(HttpMethod httpMethod, string requestUrl,
            object @object = null)
        {
            try
            {
                var requestMessage = new HttpRequestMessage(httpMethod, requestUrl);

                if (@object != null)
                    requestMessage.Content = new StringContent(Encoder.Encode(@object), Encoding.UTF8,
                        "application/json");
                
                // TODO Remove static HttpClient
                using (var responseMessage = await NetworkClient.httpClient.SendAsync(requestMessage))
                {
                    return (responseMessage.StatusCode, await responseMessage.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                return (HttpStatusCode.ServiceUnavailable, null);
            }
        }
    }
}