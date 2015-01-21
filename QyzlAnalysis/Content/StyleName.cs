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
        /*
         *p1法律价值参数，p2技术价值参数，p3经济价值参数,result及格分（专利好坏的区别分数）
         */
        public static List<List<string>> ComputerScore(List<string> lstr, double p1, double p2, double p3, double result)
        {
            List<List<string>> llstr = new List<List<string>>();
            List<string> lsstr = new List<string>();
            List<string> lsstr_1 = new List<string>();
            foreach (string s in lstr)
            {
                string[] ls = s.Split('|');
                int s1 = int.Parse(ls[0].Split('_')[1]);
                int s2 = int.Parse(ls[1].Split('_')[1]);
                int s3 = int.Parse(ls[2].Split('_')[1]);
                if (p1 * s1 + p2 * s2 + p3 * s3 >= result)
                {
                    lsstr.Add(ls[0].Split('_')[0] + "_" + ls[1].Split('_')[0] + "_" + ls[2].Split('_')[0]);
                }
                else
                {
                    lsstr_1.Add(ls[0].Split('_')[0] + "_" + ls[1].Split('_')[0] + "_" + ls[2].Split('_')[0]);
                }

            }
            llstr.Add(lsstr); llstr.Add(lsstr_1);
            return llstr;
        }
    }
}