using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emmVRC.Libraries
{
    public class UnixTime
    {
        public static DateTime ToDateTime(double unixTime)
        {
            DateTime temp = new DateTime(1970, 1, 1).AddSeconds(unixTime);
            return temp;
        }
        public static DateTime ToDateTime(string unixTimeString)
        {
            return ToDateTime(double.Parse(unixTimeString));
        }
        public static double FromDateTime(DateTime dateTime)
        {
            return dateTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }
    }
}
