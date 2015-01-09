using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QyzlAnalysis.Models;
using Newtonsoft.Json;

namespace QyzlAnalysis.Controllers
{
    public class AddDataTypeController : Controller
    {
        //
        // GET: /AddDataType/
        QyzlEntities qy = new QyzlEntities();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string maindatatype) {
            if (maindatatype.Trim() != "")
            {
                QY_DataType qd = new QY_DataType();
                qd.name = maindatatype;
                qy.QY_DataType.AddObject(qd);
                qy.SaveChanges();
            }
            return RedirectToAction("../AddInfo/Index");
        }
        [HttpPost]
        public ActionResult Unit(string defaultUnit)
        {
            if (defaultUnit.Trim() != "")
            {
                QY_Unit qd = new QY_Unit();
                qd.name = defaultUnit;
                qy.QY_Unit.AddObject(qd);
                qy.SaveChanges();
            }
            return RedirectToAction("../AddInfo/Index");
        }
        public string DataTypeVal() {
            List<QY_DataType> mess = (from n in qy.QY_DataType
                                      select n).ToList();
            string s = "[";
            foreach (QY_DataType qd in mess) {
                s += "{\"id\":\""+qd.id+"\",\"name\":\""+qd.name+"\"},";
            }
            if (s.EndsWith(",")) {
                s = s.Substring(0, s.Length - 1);
            }
            s += "]";
            return s;
        }
        public string UnitVal()
        {
            List<QY_Unit> mess = (from n in qy.QY_Unit
                                      select n).ToList();
            string s = "[";
            foreach (QY_Unit qd in mess)
            {
                s += "{\"id\":\"" + qd.id + "\",\"name\":\"" + qd.name + "\"},";
            }
            if (s.EndsWith(","))
            {
                s = s.Substring(0, s.Length - 1);
            }
            s += "]";
            return s;
        }
        public ActionResult SonDataType(int selSonDataType, string sonDataType, int selUnit)
        {
            if (sonDataType.Trim() != "")
            {
                QY_SonDataType qs = new QY_SonDataType();
                qs.dtid = selSonDataType;
                qs.name = sonDataType;
                qs.defaultUnit = selUnit;
                qy.QY_SonDataType.AddObject(qs);
                qy.SaveChanges();
            }
            return RedirectToAction("../AddInfo/Index");
        }
        public string yearSonType(int id) {
            string s = "";
            if (id != 0) {
                List<QY_SonDataType>  lq= qy.QY_SonDataType.Where(u => u.dtid == id).ToList();
                s = "[";
                foreach (QY_SonDataType qs in lq) {
                    s += "{\"id\":\""+qs.id+"\",\"name\":\""+qs.name+"\",\"unit\":\""+qs.defaultUnit+"\"},";
                }
                if (s.EndsWith(",")) {
                    s = s.Substring(0, s.Length - 1);
                }
                s += "]";
            }
            return s;
        }
        public string yearNum(string id) {//子数据单位
            int sid =int.Parse(str(id, 1));
            List<QY_Unit> qu = (from n1 in qy.QY_Unit
                                select n1).ToList();
            string s = "[";
            foreach (QY_Unit q in qu) {
                if (q.id == sid)
                {
                    s += "{\"id\":\"" + q.id + "\",\"name\":\"" + q.name + "\",\"unit\":\"1\"},";
                }
                else {
                    s += "{\"id\":\"" + q.id + "\",\"name\":\"" + q.name + "\",\"unit\":\"0\"},";
                }
            }
            return s;
        }
        private string str(string s,int i) {
            string[] strlist = s.Split('_');
            try
            {
                if (strlist[i] != "")
                {
                    return strlist[i];
                }
                else {
                    return strlist[0];
                }
            }
            catch {
                return strlist[0];
            }
        }
        public string addYeaNum(string sonid, string yea, decimal yeanum, int yeaUnit)
        {
            try{
                int sid = int.Parse(str(sonid, 0));
                QY_YearNum qn = new QY_YearNum();
                qn.Num = yeanum;
                qn.unit = yeaUnit;
                qn.sdtid = sid;
                qn.presentYear = yea;
                qy.QY_YearNum.AddObject(qn);
                qy.SaveChanges();
                return "1";
            }catch{
                return"2";
            }
        }
    }
}
