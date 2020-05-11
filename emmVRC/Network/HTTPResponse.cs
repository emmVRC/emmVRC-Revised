using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace emmVRC.Network
{
    public class HTTPResponse
    {
        public Object Results { get; set; }

        public static TinyJSON.Variant Serialize(string httpContent)
        {
            return TinyJSON.Decoder.Decode(httpContent);
        }

        public static string Deserialize(TinyJSON.Variant obj)
        {
            return TinyJSON.Encoder.Encode(obj);
        }
    }
}
