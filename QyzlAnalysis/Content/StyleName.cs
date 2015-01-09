using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QyzlAnalysis.Models;

namespace QyzlAnalysis.Content
{
    public class StyleName : Controller
    {
        public static string classon(int i) {
            if (i == 1 || i == 7 || i == 10)
            {
                return "class = on";
            }
            else {
                return "";
            }
        }
        public static List<ZL_ViewSonType> sondt(int? sonpid) {
            QyzlEntities sty = new QyzlEntities();
            List<ZL_ViewSonType> lz = null;
            if(sonpid!=0 && sonpid.ToString()!=""){
                lz = sty.ZL_ViewSonType.Where(u => u.pid == sonpid).ToList();
            }
            return lz;
        }
    }
}