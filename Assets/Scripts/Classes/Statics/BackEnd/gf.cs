using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace DeltaCoreBE
{

    public static class gf
    {
        // LOGGING 
        public static bool ENABLE_LOGGING = true;

        public static string nvStr(string name, object value)
        {
            return String.Format("{0}[{1}]", name, value.ToString());
        }

        public static void nameValue(string name, object value)
        {
            if (ENABLE_LOGGING)
            {
                Console.WriteLine(String.Format("{0}[{1}]", name, value.ToString()));
            }
        }
        public static void gLog(string s)
        {
            if (ENABLE_LOGGING)
            {
                Console.WriteLine(s);
                //Debug.Log(s);
            }
        }
    }
}