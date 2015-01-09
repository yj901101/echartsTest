using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace QyzlAnalysis.DbHelper
{
    public class HandleView
    {
        public static DataSet ViewData(string tab,string gross) {
            string sql = "select parentyear as ad,paxcount,tab.gross from parentyear p left join (select * from " + tab + " where gross = '" + gross + "') tab on p.parentyear = tab.ad order by p.parentyear";
            DataSet ds = DbHelper.Query(sql);
            return ds;
        }
        public static DataSet ZllxType(string tab)
        {
            string sql = "select distinct gross from " + tab;
            DataSet ds = DbHelper.Query(sql);
            return ds;
        }
        public static DataSet Jsly(string tab) {
            string sql = "select top 10 cdcount,c.classifyid from "+tab+" j inner join ZL_Classifydata c on j.cdid = c.id order by cdcount desc";
            DataSet ds = DbHelper.Query(sql);
            return ds;
        }
        public static DataSet GsQyName(string tab) {
            string sql = "select distinct pa from "+tab;
            DataSet ds = DbHelper.Query(sql);
            return ds;
        }
        public static DataSet GsJsly(string tab, string param) {
            string sql = "select * from " + tab+" where pa = '"+param+"'";
            DataSet ds = DbHelper.Query(sql);
            return ds;
        }
        public static DataSet ZlYearData(string tab, string param)
        {
            string sql = "select * from " + tab + " where gross = '" + param + "'";
            DataSet ds = DbHelper.Query(sql);
            return ds;
        }
        public static DataSet ZlGross(string tab, string param)
        {
            string sql = "select distinct gross from " + tab;
            DataSet ds = DbHelper.Query(sql);
            return ds;
        }
    }
}