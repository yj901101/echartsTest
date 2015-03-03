using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QyzlAnalysis.Common;
using QyzlAnalysis.Models;
using QyzlAnalysis.Content;
using MoneyRetard.Content;

namespace QyzlAnalysis.Controllers
{
    public class MatchingController : Controller
    {
        //
        // GET: /Matching/
        QyzlEntities dbmatch = new QyzlEntities();
        public ActionResult Index()
        {
            int pageIndex = 1;
            try
            {
                pageIndex = int.Parse(Request.QueryString["pager"]);
            }
            catch
            {
                pageIndex = 1;
            }
            ViewData["pagerIndex"] = pageIndex;
            const int pageSize = 10;
            List<string> lyear = new List<string>();
            for (int i = 2004; i <= DateTime.Now.AddYears(-1).Year; i++)
            {
                lyear.Add(i.ToString());
            }
            ViewData["year_list"] = lyear;
            var mat = from n1 in dbmatch.ZL_Match
                      orderby n1.id
                      select n1;
            YJPagedList<ZL_Match> pagerList;
            try
            {
                pagerList = new YJPagedList<ZL_Match>(mat, pageIndex - 1, pageSize);
            }
            catch
            {
                pagerList = new YJPagedList<ZL_Match>(mat, 0, pageSize);
            }
            ViewData["Total"] = mat.Count();
            ViewData["TotalPager"] = pagerList.TotalPages;
            ViewData["rpeo"] = geiSeeVal(pagerList);
            return View();
        }
        private Dictionary<string, List<string>> geiSeeVal(YJPagedList<ZL_Match> pagerList)
        {
            List<string> lsnus = new List<string>();
            Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
            foreach(ZL_Match zm in pagerList){
                lsnus = new List<string>();
                string[] strList = zm.nums.Split('_');
                foreach (string s in strList) {
                    if (s != "")
                    {
                        lsnus.Add(s);
                    }
                }
                dic.Add(zm.id+"_"+zm.names, lsnus);
            }
            return dic;
        }
        [HttpPost]
        public ActionResult insert(string nums, string timequene,string names)
        {
            JsonClass jc = new JsonClass();
            string years = "";
            try
            {
                for (int i = 2004; i <= DateTime.Now.AddYears(-1).Year; i++)
                {
                    years += i + "_";
                }
                string[] numList = nums.Split(',');
                string[] nameList = names.Split(',');
                for (int i = 0; i < numList.Length; i++)
                {
                    ZL_Match m = new ZL_Match();
                    m.nums = numList[i];
                    m.names = nameList[i].Substring(2, nameList[i].Length-2);
                    m.years = years;
                    m.pihao = timequene;
                    dbmatch.ZL_Match.AddObject(m);
                    dbmatch.SaveChanges();
                }
                jc.msg = "加入成功";
                jc.status = "1";
            }
            catch {
                jc.msg = "加入失败";
                jc.status = "2";
            }
            return Json(jc);
        }
        public string rtnKr(string matchid) { //获取kr的值
            return computerMatch(matchid);
        }
        public string computerMatch(string matchList)
        {
            string[] str = matchList.Split(',');
            List<ZL_Match> lzm = new List<ZL_Match>();
            List<List<double[]>> ldb = new List<List<double[]>>();
            List<double> lbS = new List<double>();
            List<string> lk = new List<string>();
            List<string> lr = new List<string>();
            List<List<string>> lls = new List<List<string>>();//存储 
            List<string> ls = new List<string>();
            List<int> li = new List<int>();
            foreach (string s in str) {
                if (s != "") {
                    int id = Convert.ToInt32(s);
                    ZL_Match zm = dbmatch.ZL_Match.Where(u => u.id == id).FirstOrDefault();
                    lzm.Add(zm);
                    ls.Add(zm.names);
                    li.Add(zm.id);
                }
            }
            ldb = MatchData.GetArr(lzm);
            foreach (List<double[]> lb in ldb) 
            {
                double k=Calculate.MultiLine(lb[0], lb[1], lb[0].Length, 1)[1];
                lk.Add(k.ToString());
                double r = Calculate.GetR2(lb[0], lb[1]);
                lr.Add(r.ToString());
            }
            string sjson = "[";
            for (int i = 0; i < lk.Count; i++) {
                sjson += "{\"id\":\"" + li[i] + "\",\"name\":\"" + ls[i] + "\",\"k\":\"" + lk[i] + "\",\"r\":\"" + lr[i] + "\"},";
            }
            if (sjson.EndsWith(",")) {
                sjson = sjson.Substring(0, sjson.Length - 1);
            }
            sjson += "]";
            return sjson;
            //for (int i = 0; i < lk.Count; i+=2) {
            //    int aaf = matchNum.comAaf(lk[i], lk[i+1]);
            //    double baf = matchNum.GetB(lr, i);
            //    lbS.Add(aaf * baf);
            //}
        }
        public string computerS(string krval)
        {
            List<double> lk = new List<double>();
            List<double> lr = new List<double>();
            List<string> lname = new List<string>();
            string[] strList = krval.Split(',');
            foreach (string s in strList) {
                string[] kr = s.Split('_');
                int nid = Convert.ToInt32(kr[0]);
                ZL_Match zm = dbmatch.ZL_Match.Where(u => u.id == nid).FirstOrDefault();
                lk.Add(Convert.ToDouble(kr[1]));
                lr.Add(Convert.ToDouble(kr[2]));
                lname.Add(zm.names);
            }
            int aaf = 0;
            double baf = 0;
            for (int i = 0; i < lk.Count; i+=2) {
                aaf = matchNum.comAaf(lk[i], lk[i + 1]);
                baf = matchNum.GetB(lr, i);
            }
            return lname[0]+"-"+lname[1]+"_"+(aaf * baf).ToString();
        }
        public ActionResult DelFiles(string Vid)
        {
            JsonClass jc = new JsonClass();
            int id = 0;
            if (Vid != "") {
                id = Convert.ToInt32(Vid);
            }
            if (id != 0)
            {

                ZL_Match zm = dbmatch.ZL_Match.Where(u => u.id == id).FirstOrDefault();
                dbmatch.DeleteObject(zm);
                dbmatch.SaveChanges();
                jc.msg = "删除成功";
            }
            else {
                jc.msg = "删除失败";
            }
            return Json(jc);
        }
    }
}
