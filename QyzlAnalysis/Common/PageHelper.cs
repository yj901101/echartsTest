using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QyzlAnalysis.Models;
namespace QyzlAnalysis.Common
{
    public static class PageHelper
    {
        public static QyzlEntities db = new QyzlEntities();
        public static string IsAble() 
        {
            string html = string.Empty;
            int num=db.ZL_ClassifyData.Where(c => c.IsToDb == true).ToList().Count();
            if (num > 0)
            {
               // html = "<input type=\"button\" value=\"批量导入\" id=\"btn3\"  disabled=\"true\"/>";
                html = "<img type=\"button\" value=\"批量导入\" id=\"btn4\" src=\"../../img/btn-import-bulk-gray.png\"  />";
            }
            else 
            {
                html = "<img type=\"button\" value=\"批量导入\" id=\"btn3\" src=\"../../img/btn-import-bulk.png\"  />";
            }
            return html;
        }
        public static string IS(bool b) 
        {
            if (b == true)
            {
                return "是";
            }
            else 
            {
                return "否";
            }
        }
        public static bool IsGs(string companyname) 
        {
            List<ZL_GsCompany> list=db.ZL_GsCompany.ToList();
            List<string> names = new List<string>();
            int i = 0;
            foreach (ZL_GsCompany model in list)
            {
                if (model.name == companyname) 
                {
                    i++;
                }
            }
            if (i>0)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }
        public static string GetZllxByDbname(string dbname)
        {
            //fmzl_ab,syxx_ab,wgzl_ab
            switch (dbname)
            {
                case "fmzl_ab":
                    return "发明专利";
                case "fmsq_ab":
                    return "发明授权";
                case "syxx_ab":
                    return "实用新型";
                case "wgzl_ab":
                    return "外观专利";
                default:
                    return "";
            }
        }
    }
}