using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using QyzlAnalysis.Models;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using System.IO;

namespace QyzlAnalysis.Common
{
    public static class ExcelHelper
    {
        public static string ExcelOut(string jsontxt) 
        {
            jsontxt = jsontxt.Insert(0, "{\"data\":");
            jsontxt = jsontxt.Insert(jsontxt.Length, "}");
            JObject jsobj = Newtonsoft.Json.Linq.JObject.Parse(jsontxt);
            int count = jsobj["data"].Count();
            List<alldata> alldata_list = new List<alldata>();
            for (int i = 0; i < count-1; i++)
            {
                List<datamodel> datamodel_list = new List<datamodel>();
                alldata alldatamodel = new alldata();
                string[] years = jsobj["data"][i]["yea"].ToString().Split('_');
                string[] nums = jsobj["data"][i]["num"].ToString().Split('_');
                for (int j = 0; j < nums.Length; j++)
                {
                    if (nums[j] == "")
                    {
                        nums[j] = "0";
                    }
                }

                for (int n = 0; n < years.Length; n++)
                {
                    datamodel model = new datamodel() { year = years[n], num = nums[n] };
                    datamodel_list.Add(model);
                }
                alldatamodel.name = jsobj["data"][i]["name"].ToString();
                alldatamodel.dmodel = datamodel_list;
                alldata_list.Add(alldatamodel);
            }
            List<string> firstrow = new List<string>();
            firstrow.Add("年份\\名称");
            foreach (alldata model in alldata_list)
            {
                firstrow.Add(model.name);
            }
            List<List<string>> rowdata = new List<List<string>>();
            rowdata.Add(firstrow);
            string[] myyear = jsobj["data"][0]["yea"].ToString().Split('_');
            for (int i = 0; i < myyear.Length; i++)
            {
                List<string> everyyearnum = new List<string>();
                everyyearnum.Add(myyear[i]);
                foreach (alldata model in alldata_list)
                {
                    string num = model.dmodel.Where(m => m.year == myyear[i]).ToList().FirstOrDefault().num;
                    everyyearnum.Add(num);
                }
                rowdata.Add(everyyearnum);
            }
            //将数据导入excel
            IWorkbook mybook = new HSSFWorkbook();
            ISheet mysheet = mybook.CreateSheet();
            for (int i = 0; i < rowdata.Count; i++)
            {
                IRow myrow = mysheet.CreateRow(i);
                for (int j = 0; j < rowdata[i].Count; j++)
                {
                    ICell mycell = myrow.CreateCell(j);
                    mycell.SetCellValue(rowdata[i][j]);
                }
            }
            FileStream fs = File.OpenWrite("D:\\" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm") + ".xls");
            mybook.Write(fs);
            fs.Dispose();
            string json = "导出成功";
            json = DataHelper.Obj2Json(json);
            return json;
        }
    }
}