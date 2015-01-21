using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QyzlAnalysis.Content;
using QyzlAnalysis.DbHelper;

namespace QyzlAnalysis.Controllers
{
    public class EvaluatingAjaxController : Controller
    {
        //
        // GET: /EvaluatingAjax/

        public ActionResult Index()
        {
            return View();
        }
        private static List<List<string>> ldic = new List<List<string>>();
        List<string> dic_fl = new List<string>();
        List<string> dic_js = new List<string>();
        List<string> dic_jj = new List<string>();
        private static int counterIndex = 0;
        private static int[] counter = { 0, 0, 0 }; //用作重置Index 
        public string ComResult(double flscore, double jsscore, double jjscore, double score)
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
            List<List<string>> lsstr = StyleName.ComputerScore(ls, flscore, jsscore, jjscore, score);
            return newnumber(Evaluating.PreForResult(lsstr) * 100) + "%";
        }
        private void setList()
        {
            dic_fl.Add("有效专利_8"); dic_fl.Add("实质审查_6"); dic_fl.Add("公开发明_4"); dic_fl.Add("失效专利_2");
            dic_js.Add("铁基新材料专利_8"); dic_js.Add("专用汽车产业专利_6"); dic_js.Add("电子信息产业专利_6");
            dic_jj.Add("第二产业_8"); dic_jj.Add("第三产业_6");
        }
        private void handle()
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
        private string newnumber(decimal num)
        {
            return Math.Round(decimal.Parse(num.ToString()), 2).ToString();
        }
    }
}
