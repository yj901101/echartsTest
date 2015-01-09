using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QyzlAnalysis.Models.ZlJson
{
    public class ZlModel2
    {
        public string TotalRow { get; set; }
        public string MaxPage { get; set; }
        public string Descript { get; set; }
        public ZlModel3 data { get; set; }
    }
}