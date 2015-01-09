using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QyzlAnalysis.Models;
using QyzlAnalysis.DbHelper;
using System.Data;

namespace QyzlAnalysis.Controllers
{
    public class ZlProTabController : Controller
    {
        //
        // GET: /ZlProTab/
        QyzlEntities zpt = new QyzlEntities();
        public ActionResult Index()
        {
            //Type t = Type.GetType("QyzlAnalysis.Models.yeapacount");
            //object o = Activator.CreateInstance(t);
            return View();
        }
        public string GsJsly(int? zldtid, int? zllimit) {
            if (zllimit == 1) {
                return GetZlXaxis(zldtid);//整体专利分析
            }
            else if (zllimit == 0) {
                return GetPie(zldtid);//技术领域专利分析
            }
            else if (zllimit == 2) { 
                return GetGs(zldtid);//规上企业的前10的技术领域分析
            }
            return "";
        }
        private string GetPie(int? zvid) {
            List<string> lsnum = new List<string>();
            List<string> lsstr = new List<string>();
            List<ZL_ViewName> lz = zpt.ZL_ViewName.Where(u => u.zsid == zvid).ToList();
            ZL_ViewSonType vs = zpt.ZL_ViewSonType.First(s => s.id == zvid);
            string title = "";
            try
            {
                if (zvid < 13)
                {
                    title = vs.ZL_ViewType.ZL_DataType.name + "_" + vs.ShowName;
                }
                else
                {
                    int? pid = vs.pid;
                    ZL_ViewSonType vs1 = zpt.ZL_ViewSonType.First(s => s.id == pid);
                    title = vs1.ZL_ViewType.ZL_DataType.name + "_" + vs1.ShowName + "_" + vs.ShowName;
                }
            }
            catch
            {
                int? pid = vs.pid;
                ZL_ViewSonType vs1 = zpt.ZL_ViewSonType.First(s => s.id == pid);
                title = vs1.ZL_ViewType.ZL_DataType.name + "_" + vs1.ShowName + "_" + vs.ShowName;
            }
            foreach (ZL_ViewName zv in lz) {
                string num = "";
                string classify = "";
                DataSet ds = HandleView.Jsly(zv.ViewName);
                foreach (DataRow dr in ds.Tables[0].Rows) {
                    num += dr["cdcount"] + "_";
                    classify += dr["classifyid"] + "_";
                }
                if (num.EndsWith("_")) {
                    num = num.Substring(0, num.Length-1);
                }
                if (classify.EndsWith("_")) {
                    classify = classify.Substring(0, classify.Length - 1);
                }
                lsnum.Add(num);
                lsstr.Add(classify);
            }
            string strconn = "[";
            for (int i = 0; i < lsstr.Count; i++) {
                strconn += "{\"num\":\"" + lsnum[i] + "\",\"classify\":\"" + lsstr[i] + "\",\"title\":\"" + title + "\"},";    
            }
            if (strconn.EndsWith(","))
            {
                strconn = strconn.Substring(0, strconn.Length - 1);
            }
            strconn += "]";
            return strconn;
        }
        private string GetGs(int? zvnid) {
            List<string> lsnum = new List<string>();
            List<string> lsstr = new List<string>();
            List<string> lsclassify = new List<string>();
            List<ZL_ViewName> lz = zpt.ZL_ViewName.Where(u => u.zsid == zvnid).ToList();
            ZL_ViewSonType vs = zpt.ZL_ViewSonType.First(s => s.id == zvnid);
            string title = "";
            try
            {
                if (zvnid < 13)
                {
                    title = vs.ZL_ViewType.ZL_DataType.name + "_" + vs.ShowName;
                }
                else
                {
                    int? pid = vs.pid;
                    ZL_ViewSonType vs1 = zpt.ZL_ViewSonType.First(s => s.id == pid);
                    title = vs1.ZL_ViewType.ZL_DataType.name + "_" + vs1.ShowName + "_" + vs.ShowName;
                }
            }
            catch
            {
                int? pid = vs.pid;
                ZL_ViewSonType vs1 = zpt.ZL_ViewSonType.First(s => s.id == pid);
                title = vs1.ZL_ViewType.ZL_DataType.name + "_" + vs1.ShowName + "_" + vs.ShowName;
            }
            foreach (ZL_ViewName zv in lz) { //获取视图名称
                DataSet ds1 = HandleView.GsQyName(zv.ViewName);
                foreach (DataRow dr1 in ds1.Tables[0].Rows) {
                    DataSet ds = HandleView.GsJsly(zv.ViewName, dr1["pa"].ToString());
                    string num = "";
                    string classify = "";
                    foreach (DataRow dr in ds.Tables[0].Rows) {
                        num += dr["paxcount"] + "_";
                        classify += dr["classifyid"] + "_";
                    }
                    if (num.EndsWith("_")) {
                        num = num.Substring(0, num.Length - 1);
                    }
                    if (classify.EndsWith("_")) {
                        classify = classify.Substring(0, classify.Length - 1);
                    }
                    lsnum.Add(num);
                    lsclassify.Add(classify);
                    lsstr.Add(dr1["pa"].ToString());
                }
            }
            string strjson = "[";
            for (int i = 0; i < lsnum.Count; i++) {
                strjson += "{\"name\":\"" + lsstr[i] + "\",\"num\":\"" + lsnum[i] + "\",\"classify\":\"" + lsclassify[i] + "\",\"title\":\"" + title + "\"},";    
            }
            if (strjson.EndsWith(",")) {
                strjson = strjson.Substring(0, strjson.Length - 1);
            }
            strjson += "]";
            return strjson;
        }
        public string GetZlXaxis(int? zldtid)
        {
            List<ZL_ViewName> lz = zpt.ZL_ViewName.Where(u => u.zsid == zldtid).ToList();
            ZL_ViewSonType vs = zpt.ZL_ViewSonType.First(s => s.id == zldtid);
            string title = "";
            try
            {
                if (zldtid < 13)
                {
                    title = vs.ZL_ViewType.ZL_DataType.name + "_" + vs.ShowName;
                }
                else {
                    int? pid = vs.pid;
                    ZL_ViewSonType vs1 = zpt.ZL_ViewSonType.First(s => s.id == pid);
                    title = vs1.ZL_ViewType.ZL_DataType.name + "_" + vs1.ShowName + "_" + vs.ShowName;  
                }
            }
            catch {
                int? pid= vs.pid;
                ZL_ViewSonType vs1 = zpt.ZL_ViewSonType.First(s => s.id == pid);
                title = vs1.ZL_ViewType.ZL_DataType.name + "_" + vs1.ShowName + "_" + vs.ShowName;
            }
            List<string> lsnum = new List<string>();
            List<string> lsstr = new List<string>();
            List<string> lsyear = new List<string>();
            Dictionary<string, string> dicAll = new Dictionary<string, string>();
            Dictionary<string, string> dicPart = new Dictionary<string, string>();
            int fmzl = 0;
            int syxx = 0;
            int wgzl = 0;
            foreach (ZL_ViewName zv in lz) {
                string name = "";
                DataSet ds_1 = HandleView.ZllxType(zv.ViewName);//获取视图的gross的列数
                foreach (DataRow dr_1 in ds_1.Tables[0].Rows)
                {
                    DataSet ds = HandleView.ViewData(zv.ViewName, dr_1["gross"].ToString());
                    name = dr_1["gross"].ToString();
                    string num = "";
                    string year = "";
                    bool flag1 = false;
                    bool flag2 = false;
                    bool flag3 = false;
                    bool flag4 = false;
                    bool flag5 = false;
                    switch(dr_1["gross"].ToString()){
                        case "申请总量":flag1 = true;break;
                        case "发明授权量":flag2 = true;break;
                        case "发明专利": flag3 = true; break;
                        case "实用新型": flag4 = true; break;
                        case "外观专利": flag5 = true; break;
                        default: break;
                    }
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        num += dr["paxcount"]+"_";
                        year += dr["ad"]+ "_";
                        if (flag1)
                        {
                            try
                            {
                                dicAll.Add(dr["ad"].ToString(), dr["paxcount"].ToString());
                            }
                            catch {
                                dicAll.Add(dr["ad"].ToString(), "0");
                            }
                        }
                        if (flag2) {
                            try
                            {
                                dicPart.Add(dr["ad"].ToString(), dr["paxcount"].ToString());
                            }
                            catch {
                                dicPart.Add(dr["ad"].ToString(), "0");
                            }
                        }
                        if (flag3) {
                            try
                            {
                                fmzl += int.Parse(dr["paxcount"].ToString());
                            }
                            catch {
                                fmzl += 0;
                            }
                        };
                        if (flag4)
                        {
                            try
                            {
                                syxx += int.Parse(dr["paxcount"].ToString());
                            }
                            catch
                            {
                                syxx += 0;
                            }
                        };
                        if (flag5)
                        {
                            try
                            {
                                wgzl += int.Parse(dr["paxcount"].ToString());
                            }
                            catch
                            {
                                wgzl += 0;
                            }
                        };
                    }
                    if (num.EndsWith("_"))
                    {
                        num = num.Substring(0, num.Length - 1);
                    }
                    if (year.EndsWith("_"))
                    {
                        year = year.Substring(0, year.Length - 1);
                    }
                    lsstr.Add(name);
                    lsnum.Add(num);
                    lsyear.Add(year);
                }
            }
            string strjson = "[{\"name\":\"发明授权率\",\"num\":\"" + InventRate(dicAll, dicPart) + "\",\"yea\":\"" + yearstr() + "\",\"unit\":\"1\",\"title\":\"" + title + "\"},";
            
            for (int i = 0; i < lsstr.Count; i++) {
                strjson += "{\"name\":\"" + lsstr[i] + "\",\"num\":\"" + lsnum[i] + "\",\"yea\":\"" + yearstr() + "\",\"unit\":\"0\",\"title\":\"" + title + "\"},";
            }
            strjson += "{\"name\":\"发明专利_实用新型_外观专利\",\"num\":\"" + fmzl + "_" + syxx + "_" + wgzl + "\",\"yea\":\"" + yearstr() + "\",\"unit\":\"2\",\"title\":\"" + title + "\"}";
            strjson += "]";
            return strjson;
        }
        public string yearstr()
        {
            return "2004_2005_2006_2007_2008_2009_2010_2011_2012_2013";
        }
        public string InventRate(Dictionary<string, string> dicpa,Dictionary<string, string> dicpart) {
            string num = "";
            foreach (KeyValuePair<string, string> kvp in dicpa) {
                foreach (KeyValuePair<string, string> sonkvp in dicpart) {
                    if (sonkvp.Key == kvp.Key) {
                        decimal d1 = 0;
                        decimal d2 = 1;
                        try
                        {
                            d1 = decimal.Parse(sonkvp.Value);
                        }
                        catch {
                            d1 = 0;
                        } 
                        try
                        {
                            d2 = decimal.Parse(kvp.Value);
                        }
                        catch {
                            d2 = 1;
                        }
                        num += ((d1 / d2)*100).ToString("#0.0") + "_";
                    }
                }
            }
            if (num.EndsWith("_"))
            {
                num = num.Substring(0, num.Length - 1);
            }
            return num;
        }
    }
}
