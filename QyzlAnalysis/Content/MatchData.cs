using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QyzlAnalysis.Content
{
    public class MatchData
    {
        public static List<List<double[]>> GetArr(List<Models.ZL_Match> list) 
        {
            List<List<double[]>> return_list = new List<List<double[]>>();
            foreach (Models.ZL_Match model in list) 
            {
                List<double[]> son_list = new List<double[]>();
                //处理年份
                List<string> year = model.years.Split('_').ToList();
                year.RemoveAt(year.Count-1);
                List<double> year2 = new List<double>();
                foreach (string item in year) 
                {
                    year2.Add(Convert.ToDouble(item));
                }
                double[] x = year2.ToArray();
                son_list.Add(x);
                //处理数据
                List<string> data = model.nums.Split('_').ToList();
                data.RemoveAt(data.Count - 1);
                List<double> data2 = new List<double>();
                foreach (string item in data) 
                {
                    if (item == "" || item == "/")
                    {
                        data2.Add(0);
                    }
                    else 
                    {
                        data2.Add(Convert.ToDouble(item));
                    }
                }
                List<double> data3 = new List<double>();
                double den = denominate(data2);
                foreach(double item in data2)
                {
                    double res = item / den;
                    data3.Add(res);
                }
                double[] y = data3.ToArray();
                son_list.Add(y);
                return_list.Add(son_list);
            }
            return return_list;
        }
        private static double denominate(List<double> ld) {
            for (int i = 0; i < ld.Count; i++) {
                if (ld[i] != 0)
                {
                    return ld[i];
                }
            }
            return 0;
        }
    }
}