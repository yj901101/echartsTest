using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QyzlAnalysis.Models;
using QyzlAnalysis.DbHelper;
using System.Data;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
namespace QyzlAnalysis.Controllers
{
    public class CustomDataController : Controller
    {
        //
        // GET: /CustomData/
        Models.QyzlEntities db = new Models.QyzlEntities();
        public ActionResult Index()
        {
            List<string> lyear = new List<string>();
            for (int i = 2004; i <= DateTime.Now.AddYears(-1).Year; i++) {
                lyear.Add(i.ToString());
            }
            ViewData["year_list"] = lyear;
            List<QY_DataType> datatype_list = db.QY_DataType.ToList();
            SelectList datatype_sellist = new SelectList(datatype_list, "id", "name");
            ViewData["datatype_sellist"] = datatype_sellist.AsEnumerable();
            var sondatatype_list = db.QY_SonDataType.Where(s => s.dtid == 1).ToList().Select(s => new { id = s.id, name = s.name }).ToList();
            SelectList sondatatype_sellist = new SelectList(sondatatype_list, "id", "name");
            ViewData["sondatatype_sellist"] = sondatatype_sellist.AsEnumerable();
            List<Models.QY_Unit> unit_list = db.QY_Unit.ToList();
            SelectList unit_sellist = new SelectList(unit_list, "id", "name");
            ViewData["unit_sellist"] = unit_sellist.AsEnumerable();
            List<ZL_DataType> lzd = db.ZL_DataType.ToList();
            SelectList zldatatype_sellist = new SelectList(lzd, "id", "name");
            ViewData["zldatatype_sellist"] = zldatatype_sellist.AsEnumerable();
            List<ZL_ViewType> lzv = (from u1 in db.ZL_ViewType
                                     where u1.ZDID == 1 && u1.id == 1
                                     select u1).ToList();
            SelectList zlviewtype_sellist = new SelectList(lzv, "id", "View_name");
            ViewData["zlviewtype_sellist"] = zlviewtype_sellist.AsEnumerable();
            List<ZL_ViewSonType> lzsv = (from n1 in db.ZL_ViewSonType
                                        where n1.zvid==1 && n1.pid==0
                                             select n1).ToList();
            SelectList zlviewsontype_sellist = new SelectList(lzsv, "id", "Showname");
            ViewData["zlviewsontype_sellist"] = zlviewsontype_sellist.AsEnumerable();
            System.Text.StringBuilder strhtml=new System.Text.StringBuilder();
            List<ZL_ViewName> ViewName_list = db.ZL_ViewName.Where(z => z.zsid ==1).ToList();
                 foreach (ZL_ViewName viewmodel in ViewName_list)
                 {
                     DataSet ds = HandleView.ZlGross(viewmodel.ViewName, null);
                     foreach (DataRow dr in ds.Tables[0].Rows)
                     {
                         string gross = dr[0].ToString();
                         strhtml.AppendLine("<option  value=\"" + viewmodel.id + "_" + gross + "\">" + gross + "</option>");
                     }
                 }
                 ViewData["sel_view"] = strhtml.ToString();
            return View();
        }
        public ActionResult QY_SonDataType()
        {
            List<Models.QY_DataType> datatype_list = db.QY_DataType.ToList();
            SelectList datatype_sellist = new SelectList(datatype_list, "id", "name");
            ViewData["datatype_sellist"] = datatype_sellist.AsEnumerable();
            List<Models.QY_Unit> unit_list = db.QY_Unit.ToList();
            var sondatatype_list = db.QY_SonDataType.Where(s => s.dtid == 1).ToList().Select(s => new { id = s.id, name = s.name }).ToList();
            SelectList sondatatype_sellist = new SelectList(sondatatype_list, "id", "name");
            ViewData["sondatatype_sellist"] = sondatatype_sellist.AsEnumerable();
            SelectList unit_sellist = new SelectList(unit_list, "id", "name");
            ViewData["unit_sellist"] = unit_sellist.AsEnumerable();
            return View();
        }
        public ActionResult QY_YearNum()
        {
            List<Models.YearModel> yearlist = new List<Models.YearModel>();
            for (int i = 2004; i <= DateTime.Now.Year; i++)
            {
                Models.YearModel yearmodel = new Models.YearModel() { TextField = i.ToString(), ValueField = i.ToString() };
                yearlist.Add(yearmodel);
            }
            SelectList year_sellist = new SelectList(yearlist, "ValueField", "TextField");
            ViewData["year_sellist"] = year_sellist.AsEnumerable();
            List<Models.QY_DataType> datatype_list = db.QY_DataType.ToList();
            SelectList datatype_sellist = new SelectList(datatype_list, "id", "name");
            ViewData["datatype_sellist"] = datatype_sellist.AsEnumerable();
            var sondatatype_list = db.QY_SonDataType.Where(s => s.dtid == 1).ToList().Select(s => new { id = s.id, name = s.name }).ToList();
            SelectList sondatatype_sellist = new SelectList(sondatatype_list, "id", "name");
            ViewData["sondatatype_sellist"] = sondatatype_sellist.AsEnumerable();
            List<Models.QY_Unit> unit_list = db.QY_Unit.ToList();
            SelectList unit_sellist = new SelectList(unit_list, "id", "name");
            ViewData["unit_sellist"] = unit_sellist.AsEnumerable();
            return View();
        }
        public ActionResult sonshow_change(int id) 
        {
            int pid =id;
            JsonModel jsmodel = new JsonModel();
            System.Text.StringBuilder strhtml = new System.Text.StringBuilder();
             List<ZL_ViewSonType> sonlist= db.ZL_ViewSonType.Where(z => z.pid == pid).ToList();
             if (sonlist.Count!=0)
             {
                 foreach (Models.ZL_ViewSonType sonmodel in sonlist)
                 {
                     if (sonmodel.id == 14||sonmodel.id == 17||sonmodel.id == 20)
                     {
                         List<ZL_ViewName> ViewName_list = db.ZL_ViewName.Where(z => z.zsid == sonmodel.id).ToList();
                         foreach (ZL_ViewName viewmodel in ViewName_list)
                         {
                             DataSet ds = HandleView.ZlGross(viewmodel.ViewName, null);
                             foreach (DataRow dr in ds.Tables[0].Rows)
                             {
                                 string gross = dr[0].ToString();
                                 strhtml.AppendLine("<option  value=\"" + viewmodel.id + "_" + gross + "\">" + gross + "</option>");
                             }
                         }
                     }

                 }
                 jsmodel.statu = "ok";
                 jsmodel.data = strhtml.ToString();
             }
             else 
             {
                 List<ZL_ViewName> ViewName_list = db.ZL_ViewName.Where(z => z.zsid ==id).ToList();
                 foreach (ZL_ViewName viewmodel in ViewName_list)
                 {
                     DataSet ds = HandleView.ZlGross(viewmodel.ViewName, null);
                     foreach (DataRow dr in ds.Tables[0].Rows)
                     {
                         string gross = dr[0].ToString();
                         strhtml.AppendLine("<option  value=\"" + viewmodel.id + "_" + gross + "\">" + gross + "</option>");
                     }
                 }
                 jsmodel.statu = "ok";
                 jsmodel.data = strhtml.ToString();
             }
            return Json(jsmodel,JsonRequestBehavior.AllowGet);
        }
        public ActionResult zlfather_change(int id)
        {
            sellistmodel jsmodel = new sellistmodel();
            System.Text.StringBuilder strhtml1 = new System.Text.StringBuilder();
            System.Text.StringBuilder strhtml2 = new System.Text.StringBuilder();
            System.Text.StringBuilder strhtml3 = new System.Text.StringBuilder();
            Models.ZL_ViewType viewtype_model=db.ZL_ViewType.Where(z => z.ZDID == id).ToList().OrderBy(z=>z.id).ToList().FirstOrDefault();
            strhtml1.AppendLine("<option  value=\"" +viewtype_model.id+ "\">" + viewtype_model.View_name + "</option>");//电子信息，铁基，专业汽车
            List<Models.ZL_ViewSonType> viewsontype_list=db.ZL_ViewSonType.Where(z => z.zvid == viewtype_model.id).ToList();
            Models.ZL_ViewSonType viewsontype_model=db.ZL_ViewSonType.Where(z => z.zvid == viewtype_model.id).ToList().OrderBy(z => z.id).ToList().FirstOrDefault();
            foreach(Models.ZL_ViewSonType smodel in viewsontype_list)
            {
                strhtml2.AppendLine("<option  value=\"" +smodel.id+ "\">" + smodel.ShowName+ "</option>");//马鞍山
            }
            List<ZL_ViewSonType> sonlist = db.ZL_ViewSonType.Where(z => z.pid ==viewsontype_model.id).ToList();
            if (sonlist.Count != 0)
            {
                foreach (Models.ZL_ViewSonType sonmodel in sonlist)
                {
                    if (sonmodel.id == 14 || sonmodel.id == 17 || sonmodel.id == 20)//整体专利分析，规上专利分析，龙头企业
                    {
                        List<ZL_ViewName> ViewName_list = db.ZL_ViewName.Where(z => z.zsid == sonmodel.id).ToList();
                        foreach (ZL_ViewName viewmodel in ViewName_list)
                        {
                            DataSet ds = HandleView.ZlGross(viewmodel.ViewName, null);
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                string gross = dr[0].ToString();
                                strhtml3.AppendLine("<option  value=\"" + viewmodel.id + "_" + gross + "\">" + gross + "</option>");
                            }
                        }
                    }

                }
            }
            else
            {
                List<ZL_ViewName> ViewName_list = db.ZL_ViewName.Where(z => z.zsid ==viewsontype_model.id).ToList();
                foreach (ZL_ViewName viewmodel in ViewName_list)
                {
                    DataSet ds = HandleView.ZlGross(viewmodel.ViewName, null);
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        string gross = dr[0].ToString();
                        strhtml3.AppendLine("<option  value=\"" + viewmodel.id + "_" + gross + "\">" + gross + "</option>");
                    }
                }
               
            }
            jsmodel.sel1 = strhtml1.ToString();
            jsmodel.sel2 = strhtml2.ToString();
            jsmodel.sel3 = strhtml3.ToString();
            return Json(jsmodel,JsonRequestBehavior.AllowGet);
        }
        public ActionResult ExcelOutput() 
        {
            string[] rows = Request.QueryString["data"].ToString().Split(',');
            List<List<string>> allrows=new List<List<string>>();
            for (int i = 0; i < rows.Length; i++) 
            {  
                List<string> rowdetail=new List<string>();
                string[] details=rows[i].Split(';');
                for (int j = 0; j < details.Length; j++) 
                {
                    rowdetail.Add(details[j]);
                }
                allrows.Add(rowdetail);
            }
            IWorkbook mybook = new HSSFWorkbook();
            ISheet mysheet=mybook.CreateSheet();
            for (int i = 0; i < allrows.Count; i++) 
            {
                IRow myrow=mysheet.CreateRow(i);
                for (int j = 0; j < allrows[i].Count; j++) 
                {
                   ICell mycell=myrow.CreateCell(j);
                   mycell.SetCellValue(allrows[i][j]);
                }
            }
            string dir = "/Excel导出/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            Directory.CreateDirectory(Path.GetDirectoryName(HttpContext.Server.MapPath(dir))) ;
            string filename = DateTime.Now.ToString("yyyy-MM-dd-HH-mm") + ".xls";
            string filepath = dir + filename;
            string path = Request.MapPath(filepath);
            FileStream fs = System.IO.File.OpenWrite(path);
            mybook.Write(fs);
            fs.Dispose();
            string json = filepath;
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        public  class sellistmodel
        {
            public string sel1 { get; set; }
            public string sel2 { get; set; }
            public string sel3 { get; set; }
        }
    }
}
