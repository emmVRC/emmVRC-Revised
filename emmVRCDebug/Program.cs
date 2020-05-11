using System;
using System.Collections.Generic;
using System.Net.Http;
using emmVRC.Network;

namespace emmVRCDebug
{
    class Program
    {
        public static Dictionary<string, string> dic = new Dictionary<string, string>();
        static void Main(string[] args)
        {
            dic["name"] = "zail";
            dic["username"] = "qweqeqeqeq-fdf6-ewr4b63-a2b1-64awdaw19aweqqweqwee634f240";
            NetworkClient.InitializeClient();
            resolveGET("http://localhost:3000/api/donor");
            resolvePOST("http://localhost:3000/api/authentication/login", dic);
        }

        private static async void resolveGET(string url)
        {
            try
            {
                TinyJSON.ProxyArray data = (TinyJSON.ProxyArray)TinyJSON.Decoder.Decode(await HTTPRequest.get(url));
                for (int i = 0; i < data.Count; i++)
                {
                    Console.WriteLine((string)data[i]["donor_user_id"]);
                }
                Console.WriteLine(await HTTPRequest.get(url));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static async void resolvePOST(string url, object obj)
        {
            try
            {
                TinyJSON.ProxyObject data = (TinyJSON.ProxyObject)TinyJSON.Decoder.Decode(await HTTPRequest.post(url, obj));
                Console.WriteLine(data.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
    }
}
