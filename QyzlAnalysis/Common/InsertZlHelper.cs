using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using QyzlAnalysis;
using Newtonsoft.Json;

namespace QyzlAnalysis.Common
{
    public static class InsertZlHelper
    {
        public static List<Models.ZL_Data> GetZlJsonData(params object[] pars)
        {
            string pic_or_sic = pars[0].ToString();
            int curpage =int.Parse(pars[1].ToString());
            string url = "http://www.masipo.org.cn/somas/DataInterface/dataImpl.aspx?action=MasLucene&GUID=dee7c54b-d9b7-408c-b4b2-a4e0f27d544a&par=(pic:"+pic_or_sic+" or sic:"+pic_or_sic+")&typex=fmzl_ab,fmsq_ab,syxx_ab,wgzl_ab&Country=&PageSize=10&CurPage="+curpage+"&done=";
            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
            string str = reader.ReadLine();
            //Models.ZlJson.ZlModel1 model = (Models.ZlJson.ZlModel1)JsonConvert.DeserializeObject(str, typeof(Models.ZlJson.ZlModel1));
            JObject jsobj = Newtonsoft.Json.Linq.JObject.Parse(str);
            List<Models.ZL_Data> zldata_list = new List<Models.ZL_Data>();
            for (int i = 0; i < jsobj["Rusults"]["data"].Count(); i++) 
            {
                Models.ZL_Data zl_model = new Models.ZL_Data();
                zl_model.AN = jsobj["Rusults"]["data"][i]["AN"].ToString();
                zl_model.Ad = Convert.ToDateTime(jsobj["Rusults"]["data"][i]["AD"].ToString());
                zl_model.Pd = Convert.ToDateTime(jsobj["Rusults"]["data"][i]["PD"].ToString());
                zl_model.Pa = jsobj["Rusults"]["data"][i]["PA"].ToString();
                zl_model.Inn = jsobj["Rusults"]["data"][i]["INN"].ToString();
                zl_model.Pic = jsobj["Rusults"]["data"][i]["PIC"].ToString();
                zl_model.Sic = jsobj["Rusults"]["data"][i]["SIC"].ToString();
                zl_model.Zllx = Common.PageHelper.GetZllxByDbname(jsobj["Rusults"]["data"][i]["StrDb"].ToString());
                zl_model.IsHand = false;
                zl_model.IsGs = Common.PageHelper.IsGs(jsobj["Rusults"]["data"][i]["PA"].ToString());
                zl_model.Flzt = jsobj["Rusults"]["data"][i]["Flzt"].ToString();
                string[] IsSqres=IsSq(jsobj["Rusults"]["data"][i]["AN"].ToString());
                if (IsSqres[0].ToLower() == "false")
                {
                    zl_model.IsSq = false;
                    zl_model.SqDate = null;
                }
                else 
                {
                    zl_model.IsSq = true;
                    zl_model.SqDate = Convert.ToDateTime(IsSqres[1]);
                }
                zl_model.CDID = int.Parse(pars[2].ToString());
                zldata_list.Add(zl_model);
            }
            return zldata_list;
        }
        public static int GetPageCount(params object[] pars) 
        {
            string pic_or_sic = pars[0].ToString();
            int curpage = int.Parse(pars[1].ToString());
            string url = "http://www.masipo.org.cn/somas/DataInterface/dataImpl.aspx?action=MasLucene&GUID=dee7c54b-d9b7-408c-b4b2-a4e0f27d544a&par=(pic:" + pic_or_sic + " or sic:" + pic_or_sic + ")&typex=fmzl_ab,fmsq_ab,syxx_ab,wgzl_ab&Country=&PageSize=10&CurPage=" + curpage + "&done=";
            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
            string str = reader.ReadLine();
            JObject jsobj = Newtonsoft.Json.Linq.JObject.Parse(str);
            int PageCount = int.Parse(jsobj["Rusults"]["TotalRow"].ToString());
            return PageCount;
        }
        public static string[] IsSq(string AN) 
        {
            string[] IsSqres = new string[2];
            string url = "http://www.masipo.org.cn/somas/DataInterface/dataImpl.aspx?action=IsSq&GUID=dee7c54b-d9b7-408c-b4b2-a4e0f27d544a&par=AN:"+AN+"&typex=fmzl_ab,fmsq_ab,syxx_ab,wgzl_ab&Country=&PageSize=10&CurPage=1&done=";
            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
            string str = reader.ReadLine();
            JObject jsobj = Newtonsoft.Json.Linq.JObject.Parse(str);
            IsSqres[0] = jsobj["IsSq"].ToString();
            IsSqres[1]=jsobj["PD"].ToString();
            return IsSqres;
        }
        public static void InsertToDB(List<Models.ZL_Data> zllist) 
        {
            Models.QyzlEntities db = new Models.QyzlEntities();
            foreach (Models.ZL_Data model in zllist) 
            {
                db.ZL_Data.AddObject(model);
                db.SaveChanges();
            }
        }
    }
}