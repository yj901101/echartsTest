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
    public class CreateTabController : Controller
    {
        //
        // GET: /CreateTab/
        QyzlEntities db = new QyzlEntities();
        public ActionResult Index()
        {
            return View();
        }
        public string Rjson(string id, string valtype) {
            if (valtype == "jj") {
                return JjRjson(int.Parse(id));
            }
            else if (valtype == "zl")
            {
                return ZlRjson(id);
            }
            else {
                return "";
            }
        }
        private string ZlRjson(string str) {
            string[] strlist = str.Split('_');
            int zdtid = int.Parse(strlist[0]);//zl_dataType的 id
            int zvtid = int.Parse(strlist[1]);//zl_viewtype 的id
            int zsvtid = int.Parse(strlist[2]);//zl_sonviewtype的id
            ZL_DataType zdt = db.ZL_DataType.First(u => u.id == zdtid);
            ZL_ViewType zvt = db.ZL_ViewType.First(v => v.id == zvtid);
            ZL_ViewSonType zvst = db.ZL_ViewSonType.First(s => s.id == zsvtid);
            int id = int.Parse(strlist[3]);
            string condition = "";
            try{
                condition = strlist[4];
                if (condition =="") 
                {
                    return "err";
                }
            }
            catch {
                return "err";
            }
            var viewname = db.ZL_ViewName.First(u => u.id == id);
            DataSet ds = HandleView.ViewData(viewname.ViewName, condition);
            string strconn = "[";
            foreach (DataRow dr in ds.Tables[0].Rows) {
                strconn += "{\"name\":\"" + sub(zdt.name, 4) + "" + sub(zvst.ShowName, 4) + "" + condition + "（个）\",\"year\":\"" + dr["ad"] + "\",\"num\":\"" + dr["paxcount"] + "\",\"typ\":\"zl" + sub(zdt.name, 4) + "" + sub(zvst.ShowName, 4) + "" + condition + "\",\"unit\":\"0\"},";
            }
            if (strconn.EndsWith(",")) {
                strconn = strconn.Substring(0, strconn.Length - 1);
            }
            strconn += "]";
            return strconn;
        }
        private string JjRjson(int id)
        {
            string strconn = "[";
           
            QY_SonDataType st = db.QY_SonDataType.First(u => u.id == id);
            Dictionary<string,string> dic = YearDic();
            foreach (KeyValuePair<string, string> kvp in dic){
                string syear = kvp.Key;
                List<QY_YearNum> yearNum =(from n1 in db.QY_YearNum
                                           where n1.sdtid == id && n1.presentYear==syear
                                           select n1).ToList();
                if (yearNum.Count > 0)
                {
                    foreach (QY_YearNum y in yearNum)
                    {
                        strconn += "{\"name\":\"" + st.name + "" + st.QY_Unit.name + "\",\"year\":\"" + syear + "\",\"num\":\"" + newnumber(y.Num) + "\",\"typ\":\"jl" + st.name + "" + st.QY_Unit.name + "\",\"unit\":\"" + st.QY_Unit.name + "\"},";
                    }
                }
                else {
                    strconn += "{\"name\":\"" + st.name + "" + st.QY_Unit.name + "\",\"year\":\"" + syear + "\",\"num\":\"0\",\"typ\":\"jl" + st.name + "" + st.QY_Unit.name + "\",\"unit\":\"" + st.QY_Unit.name + "\"},";
                }
                
            }
            if (strconn.EndsWith(",")) {
                strconn = strconn.Substring(0, strconn.Length - 1);
            }
            strconn += "]";
            return strconn;
        }
        private Dictionary<string, string> YearDic() {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            for (int i = 2004; i <= DateTime.Now.AddYears(-1).Year; i++) {
                dic.Add(i.ToString(), "-");
            }
            return dic;
        }
        private string newnumber(decimal? num)
        {
            return double.Parse(num.ToString()).ToString();
        }
        private string sub(string s, int n) {
            return s.Substring(0, n);
        }
    }
}
