using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emmVRC.Network.Objects
{
    public static class SerializedObjectExtensions
    {
        public static string Encode(this SerializedObject @object)
        {
            return TinyJSON.Encoder.Encode(@object);
        }

        public static TinyJSON.Variant Decode(string str)
        {
            return TinyJSON.Decoder.Decode(str);
        }
    }
}
