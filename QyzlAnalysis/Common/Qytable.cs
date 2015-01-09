using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QyzlAnalysis.Common
{
    public class Qytable
    {
        public static string sub(string str){
            if (str.Length > 10) {
                return str.Substring(0, 10) + "...";
            }else{
                return str;
            }
        }
    }
}