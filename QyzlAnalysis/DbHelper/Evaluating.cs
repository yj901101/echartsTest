using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace QyzlAnalysis.DbHelper
{
    public class Evaluating
    {
        public static decimal PreForResult(List<List<string>> lls) {
            decimal count_1 = 0;
            decimal count_2 = 0;
            foreach (string s_1 in lls[0]) { 
                string[] strList = s_1.Split('_');
                string sql = "select count(1) from EvaluatingView where flzt='" + strList[0] + "' and js = '" + strList[1] + "' and jj='" + strList[2] + "'";
                DataSet ds = DbHelper.Query(sql);
                count_1 += int.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            foreach (string s_1 in lls[1])
            {
                string[] strList = s_1.Split('_');
                string sql = "select count(1) from EvaluatingView where flzt='" + strList[0] + "' and js = '" + strList[1] + "' and jj='" + strList[2] + "'";
                DataSet ds = DbHelper.Query(sql);
                count_2 += int.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            return count_1 / (count_1 + count_2);
        }
    }
}