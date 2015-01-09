using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QyzlAnalysis.Models;
using QyzlAnalysis.Common;

namespace QyzlAnalysis.Controllers
{
    public class XaxisController : Controller
    {
        //
        // GET: /Xaxis/
        QyzlEntities axis = new QyzlEntities();
        public ActionResult Index()
        {
            return View();
        }
        public string GetXaxis(int dataTypeid)
        {
            //var newmode = xaxis.QY_YearNum.Join(xaxis.QY_SonDataType, s => s.sdtid, c => c.id, (s, c) => new { yea = s.presentYear, sdtid = c.id}).ToList();
            List<QY_SonDataType> qs = axis.QY_SonDataType.Where(s => s.dtid == dataTypeid).ToList();
            QY_DataType qd = axis.QY_DataType.First(s => s.id == dataTypeid);
            int yid = qs[0].id;
            List<QY_YearNum> ls = axis.QY_YearNum.Where(u => u.sdtid == yid).ToList();
            string str = "[";
            foreach (QY_YearNum qn in ls) {
                if (YearIsNull(dataTypeid,qn.presentYear))
                {
                    str += "{\"year\":\"" + qn.presentYear + "\",\"title\":\"" + qd.name + "\"},";
                }
            }
            if (str.EndsWith(",")) {
                str = str.Substring(0, str.Length-1);
            }
            str += "]";
            return str;
        }
        public string GetYaxis(int dataTypeid,int datatyepe) {
            List<QY_SonDataType> qs = axis.QY_SonDataType.Where(s => s.dtid == dataTypeid).ToList();
            List<int?> ls = new List<int?>();
            List<int?> ls2 = new List<int?>();
            foreach (QY_SonDataType qt in qs) {
                if (ls.Count >= 1)
                {
                    if (!ls.Contains(qt.defaultUnit))
                    {
                        ls.Add(qt.defaultUnit);
                    }
                }
                else {
                    ls.Add(qt.defaultUnit);
                }
                ls2.Add(qt.defaultUnit);
            }
            int? unit = 0;
            if (datatyepe == 1) {
                unit = wunit(ls2);
            }
            List<QY_Unit> lu = new List<QY_Unit>();
            if (unit != 0)
            {
                var qn = axis.QY_Unit.First(u => u.id == unit);
                lu.Add(qn);
            }
            else
            {
                foreach (int i in ls)
                {
                    var qn = axis.QY_Unit.First(u => u.id == i);
                    lu.Add(qn);
                }
            }
            string str = "[";
            foreach (QY_Unit q in lu) {
                if (q.id != 6)
                {
                    str += "{\"unit\":\"" + q.name + "\"},";
                }
            }
            if (str.EndsWith(",")) {
                str = str.Substring(0, str.Length - 1);
            }
            str += "]";
            return str;
        }
        public string GetSeries(int dataTypeid)
        {
            List<QY_SonDataType> qs = axis.QY_SonDataType.Where(s => s.dtid == dataTypeid).ToList();
            List<string> lsnum = new List<string>();
            List<string> lsname = new List<string>();
            List<int> lsunit = new List<int>();
            List<string> lssunit = new List<string>();
            List<string> lssfather = new List<string>();//在QY_Relation表中查询是否有父类
            int? jcompare =0;
            foreach (QY_SonDataType qd in qs) {
                int id = qd.id;
                string str = "";
                List<QY_YearNum> qy = axis.QY_YearNum.Where(s => s.sdtid == id).OrderBy(s=>s.presentYear).ToList();
                foreach (QY_YearNum yn in qy) {
                    if (YearIsNull(dataTypeid, yn.presentYear))
                    {
                        if (yn.Num != null && yn.Num.ToString() != "" && yn.unit!=6)
                        {
                            str += newnumber(yn.Num) + "_";
                        }
                        else
                        {
                            str += 0 + "_";
                        }
                    }
                }
                if (str.EndsWith("_")) {
                    str = str.Substring(0, str.Length - 1);
                }
                List<QY_Relation> rela = axis.QY_Relation.Where(u => u.sonsdtid == id).ToList();
                if (rela.Count >= 1)
                {
                    foreach (QY_Relation q in rela) { 
                        int? qint = q.sdtid;
                        QY_SonDataType qsdt = axis.QY_SonDataType.First(u => u.id == qint);
                        lssfather.Add(qsdt.name);
                    }
                }
                else {
                    lssfather.Add("%"); 
                }
                lsnum.Add(str);//用来存放连接的数据例如1_2_3_4_5
                if (qd.defaultUnit != 6)
                {
                    lsname.Add(qd.name);
                    lssunit.Add(qd.QY_Unit.name);
                }//存放对应的名称
                if (lsunit.Count >= 1)//用来判断是否为同一个单位,及所属y轴的哪一条线
                {
                    if (qd.defaultUnit == jcompare)
                    {
                        lsunit.Add(0);
                    }
                    else
                    {
                        lsunit.Add(1);
                    }
                }
                else
                {
                    jcompare = qd.defaultUnit;
                    lsunit.Add(0);
                }
            }
            string conn = "[";
            for (int i = 0; i < lsname.Count; i++) {
                conn += "{\"num\":\"" + lsnum[i] + "\",\"name\":\"" + lsname[i] + "" + lssunit[i] + "\",\"unit\":\"" + lsunit[i] + "\",\"stack\":\"" + lssfather[i] + "" + lssunit[i] + "\"},";
            }
            if (conn.EndsWith(",")) {
                conn = conn.Substring(0, conn.Length - 1);
            }
            conn += "]";
            return conn;
        }
        public string GetOtherSeries(int dataTypeid) { 
            List<QY_SonDataType> qs = axis.QY_SonDataType.Where(s => s.dtid == dataTypeid).ToList();
            List<string> lsnum = new List<string>();
            List<string> lsname = new List<string>(); 
            List<string> lsunit = new List<string>();
            List<int?> lsiunit = new List<int?>();
            foreach (QY_SonDataType qd in qs) {
                lsiunit.Add(qd.defaultUnit);
            }
            foreach (QY_SonDataType qd in qs)
            {
                int id = qd.id;
                string str = "";
                List<QY_YearNum> qy = axis.QY_YearNum.Where(s => s.sdtid == id).OrderBy(s => s.presentYear).ToList();
                foreach (QY_YearNum yn in qy)
                {
                    if (YearIsNull(dataTypeid, yn.presentYear))
                    {
                        if (yn.Num != null && yn.Num.ToString() != "" && yn.unit == wunit(lsiunit))
                        {
                            str += newnumber(yn.Num) + "_";
                        }
                        else
                        {
                            str += "";
                        }
                    }
                }
                if (str.EndsWith("_"))
                {
                    str = str.Substring(0, str.Length - 1);
                }
                lsnum.Add(str);//用来存放连接的数据例如1_2_3_4_5
                lsname.Add(qd.name);
                lsunit.Add(qd.QY_Unit.name);//存放对应的名称
            }
            string conn = "[";
            for (int i = 0; i < lsname.Count; i++) {
                if (lsnum[i] != "")
                {
                    conn += "{\"num\":\"" + lsnum[i] + "\",\"name\":\"" + lsname[i] + "" + lsunit[i] + "\"},";
                }
            }
            if (conn.EndsWith(",")) {
                conn = conn.Substring(0, conn.Length - 1);
            }
            conn += "]";
            return conn;
        }
        private bool YearIsNull(int dtid, string yea) {//DataType id和年份
            int y = int.Parse(yea);
            List<QY_YearNum> number = (from n1 in axis.QY_YearNum
                                       join n2 in axis.QY_SonDataType
                                       on n1.sdtid equals n2.id
                                     where n2.dtid == dtid && yea == n1.presentYear
                                     select n1).ToList();
            foreach (QY_YearNum q in number) {
                int num = (int)q.Num;
                if (num != 0) {
                    if (y >= 2004)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private string newnumber(decimal? num) {
            return double.Parse(num.ToString()).ToString();
        }
        private int? wunit(List<int?> l) {
            int? i = l.GroupBy(x => x).OrderByDescending(y => y.Count()).First().Key;
            return i;
        }
        [HttpGet]
        public string ExeclOutput(int? id) {
            
                ZlProTabController zl = new ZlProTabController();
                ExcelHelper.ExcelOut(zl.GetZlXaxis(id));
                return "1";
        }
    }
}
