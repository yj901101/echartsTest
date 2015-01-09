using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QyzlAnalysis.Models.ZlJson
{
    public class ZlModel1
    {
        public string Success { get; set; }
        public string Message { get; set; }
        public ZlModel2 Rusults { get; set; }
    }
}