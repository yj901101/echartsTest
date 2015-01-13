using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QyzlAnalysis.Models;

namespace QyzlAnalysis.Controllers
{
    public class GradeController : Controller
    {
        //
        // GET: /Grade/
        QyzlEntities qe = new QyzlEntities();
        public ActionResult Index()
        {
            return View();
        }
        public string Denominato()
        {
            int id = 2;
            List<QY_SonDataType> lson = qe.QY_SonDataType.Where(u => u.dtid == id).ToList();
            Dictionary<int?, int?> unit = removeDiffUnit(lson);//获取的id对应的unit.
            List<Dictionary<string, decimal?>> ldic = new List<Dictionary<string, decimal?>>();
            List<string> ls = new List<string>();
            List<string> lsname = new List<string>();
            string x = countDenominato(unit);
            if (x.IndexOf("|") != -1)
            {
                string[] str = x.Substring(0, x.Length - 1).Split('_');
                foreach (string s in str)
                {
                    if (s != "")
                    {
                        int? xid = int.Parse(s);
                        List<QY_YearNum> lsy = (from n1 in qe.QY_YearNum
                                                where n1.sdtid == xid
                                                select n1)
                                               .ToList();
                        Dictionary<string, decimal?> dic = new Dictionary<string, decimal?>();
                        foreach (QY_YearNum yn in lsy)
                        {
                            if (int.Parse(yn.presentYear) > 2003)
                            {
                                dic.Add(yn.presentYear, yn.Num);
                            }
                        }
                        QY_SonDataType son = qe.QY_SonDataType.First(u => u.id == xid);
                        lsname.Add(son.name);
                        ldic.Add(dic);
                    }
                }
                for (int i = 1; i < ldic.Count; i++)
                {
                    ls.Add("[{\"name\",\"" + lsname[i] + "\"," + resultCount(ldic[0], ldic[i]) + "}]");
                }
                string s1 = "";
                foreach (string strjosn in ls)
                {
                    s1 += strjosn + "|";
                }
                s1 = s1 + "";
            }
            else
            {

            }
            return "";
        }
        public Dictionary<int?, int?> removeDiffUnit(List<QY_SonDataType> lson)//取适合的单位
        {
            List<Dictionary<int?, int?>> lli = new List<Dictionary<int?, int?>>();
            Dictionary<int?, int?> li = new Dictionary<int?, int?>();
            foreach (QY_SonDataType qs in lson)
            {
                if (lli.Count <= 0)
                {
                    li.Add(qs.id, qs.defaultUnit);
                }
                else
                {
                    if (contains(lli, qs.defaultUnit))
                    {
                        li.Add(qs.id, qs.defaultUnit);//在二维数组中则添加
                    }
                    else
                    {
                        li = new Dictionary<int?, int?>();
                        li.Add(qs.id, qs.defaultUnit);
                    }
                }
                lli.Add(li);
            }
            return rUnitCondition(lli);
        }
        private bool contains(List<Dictionary<int?, int?>> lli, int? x)//判断元素是否在二维数组中
        {
            foreach (Dictionary<int?, int?> li in lli)
            {
                foreach (KeyValuePair<int?, int?> kv in li)
                {
                    if (kv.Value == x)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private Dictionary<int?, int?> rUnitCondition(List<Dictionary<int?, int?>> lli)//返回需要的单位
        {
            int coun = rLc(lli);
            List<Dictionary<int?, int?>> newLi = new List<Dictionary<int?, int?>>();
            foreach (Dictionary<int?, int?> li in lli)
            {
                if (li.Count == coun)
                {
                    newLi.Add(li);
                }
            }
            if (newLi.Count > 1)
            {
                return newLi[0];//返回了第一个（后续功能添加）
            }
            else
            {
                return newLi[0];//当newLi中只有一个元素
            }
        }
        private int rLc(List<Dictionary<int?, int?>> lli)//返回最长的数值的数量
        {
            int len = 0;
            foreach (Dictionary<int?, int?> li in lli)
            {
                if (li.Count >= len)
                {
                    len = li.Count;
                }
            }
            return len;
        }
        private string countDenominato(Dictionary<int?, int?> den)
        {//总额在数组列表中
            int? x = 0;
            int i = 0;
            string str = "";
            foreach (KeyValuePair<int?, int?> kv in den)
            {
                if (i == 0)
                {
                    x = kv.Key;
                    i++;
                }
                str += kv.Key + "_";
            }
            List<QY_Relation> lq = qe.QY_Relation.Where(u => u.sdtid == x).ToList();
            if (lq.Count >= 1)
            {
                return str + "|";
            }
            else
            {
                return str;
            }
        }
        private string resultCount(Dictionary<string, decimal?> den, Dictionary<string, decimal?> ele)
        {
            string strconn = "";
            string stryear = "";
            foreach (KeyValuePair<string, decimal?> kv in den)
            {
                decimal? dec = 0;
                foreach (KeyValuePair<string, decimal?> kvele in ele)
                {
                    if (kv.Key == kvele.Key)
                    {
                        try
                        {
                            dec = kvele.Value / kv.Value;
                        }
                        catch
                        {
                            dec = 0;
                        }
                    }
                }
                strconn += newnumber(dec) + "_";
                stryear += kv.Key + "_";
            }
            if (strconn.EndsWith("_"))
            {
                strconn = strconn.Substring(0, strconn.Length - 1);
            }
            if (stryear.EndsWith("_"))
            {
                stryear = stryear.Substring(0, stryear.Length - 1);
            }
            return "\"str\":\"" + strconn + "\",\"year\":\"" + stryear + "\"";
        }
        private string newnumber(decimal? num)
        {
            return Math.Round(decimal.Parse(num.ToString()), 2).ToString();
        }

    }
}
