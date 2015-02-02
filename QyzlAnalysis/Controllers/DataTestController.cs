using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QyzlAnalysis.Models;

namespace QyzlAnalysis.Controllers
{
    public class DataTestController : Controller
    {
        //
        // GET: /DataTest/
        Models.QyzlEntities db1 = new Models.QyzlEntities();
        public ActionResult Index()
        {
            return View();
        }
        public string retJson(string strconn) {
            string[] sLsit = strconn.Split(',');
            List<string> ls = new List<string>();
            List<string> lsyear = new List<string>() { "2004", "2005", "2006", "2007", "2008", "2009", "2010", "2011", "2012", "2013"};
            foreach (string s in sLsit) {
                List<DataTest> ldt= db1.DataTest.Where(u => u.name == s).ToList();
                string str = "";
                foreach(DataTest d in ldt){
                    str += d.Ydata+"_";
                }
                if (str.EndsWith("_"))
                {
                    str = str.Substring(0, str.Length - 1);
                }
                ls.Add(str);
            }
            string conn = "[";
            for (int i = 0; i < ls.Count; i++) {
                conn += "{\"num\":\"" + ls[i] + "\",\"name\":\"" + lsyear[i]+"\"},";
            }
            if (conn.EndsWith(","))
            {
                conn = conn.Substring(0, conn.Length - 1);
            }
            conn += "]";
            return conn;
        }
    }
}
