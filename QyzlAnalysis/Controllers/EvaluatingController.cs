using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QyzlAnalysis.Content;
using QyzlAnalysis.DbHelper;

namespace QyzlAnalysis.Controllers
{
    public class EvaluatingController : Controller
    {
        //
        // GET: /Evaluating/
        private static List<List<string>> ldic = new List<List<string>>();
        List<string> dic_fl = new List<string>();
        List<string> dic_js = new List<string>();
        List<string> dic_jj = new List<string>();
        private static int counterIndex = 0;
        private static int[] counter = { 0, 0, 0 }; //用作重置Index 
        public ActionResult Index()
        {
            ldic = new List<List<string>>();
            dic_fl = new List<string>();
            dic_js = new List<string>();
            dic_jj = new List<string>();
            List<string> ls = new List<string>();
            setList();
            ldic.Add(dic_fl); ldic.Add(dic_js); ldic.Add(dic_jj);
            counterIndex = ldic.Count - 1;
            for (int i = 0; i < dic_fl.Count * dic_js.Count * dic_jj.Count; i++)
            {
                ls.Add(dic_fl[counter[0]] + "|" + dic_js[counter[1]] + "|" + dic_jj[counter[2]]);
                handle();
            }
            /**
             *得到返回的二维数组，index=0的是达到及格分的，index=1的是不达标的
             */
            List<List<string>> lsstr = ComputerScore(ls, 0.2, 0.2, 0.6, 7.0);
            ViewData["count"] = Evaluating.PreForResult(lsstr)*100+"%";
            return View();
        }
        private void setList() {
            dic_fl.Add("有效专利_8"); dic_fl.Add("实质审查_6"); dic_fl.Add("公开发明_4"); dic_fl.Add("失效专利_2");
            dic_js.Add("铁基新材料专利_8"); dic_js.Add("专用汽车产业专利_6"); dic_js.Add("电子信息产业专利_6");
            dic_jj.Add("第二产业_8"); dic_jj.Add("第三产业_6");
        }
        public void handle()
        {
            counter[counterIndex]++;//将数组counter的值自增
            if (counter[counterIndex] >= ldic[counterIndex].Count)//当增加的值大于或等于当前数组的索引的时候,重置索引数组counter的值
            {
                counter[counterIndex] = 0;//重置当前的索引位
                counterIndex--;//往前走一位
                if (counterIndex >= 0)
                {
                    handle();
                }
                counterIndex = ldic.Count - 1;//修改最后一位内容
            }
        }
        /*
         *p1法律价值参数，p2技术价值参数，p3经济价值参数,result及格分（专利好坏的区别分数）
         */
        private List<List<string>> ComputerScore(List<string> lstr,double p1,double p2 , double p3,double result)
        {
            List<List<string>> llstr = new List<List<string>>();
            List<string> lsstr = new List<string>();
            List<string> lsstr_1 = new List<string>();
            foreach (string s in lstr) {
                string[] ls = s.Split('|');
                int s1 = int.Parse(ls[0].Split('_')[1]);
                int s2 = int.Parse(ls[1].Split('_')[1]);
                int s3 = int.Parse(ls[2].Split('_')[1]);
                if (p1 * s1 + p2 * s2 + p3 * s3 >= result)
                {
                    lsstr.Add(ls[0].Split('_')[0] + "_" + ls[1].Split('_')[0] + "_" + ls[2].Split('_')[0]);
                }
                else {
                    lsstr_1.Add(ls[0].Split('_')[0] + "_" + ls[1].Split('_')[0] + "_" + ls[2].Split('_')[0]);
                }

            }
            llstr.Add(lsstr); llstr.Add(lsstr_1);
            return llstr;
        }
    }
}
