using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QyzlAnalysis.Controllers
{
    public class Add_QYDataController : Controller
    {
        //
        // GET: /Add_QY_DataType/
        Models.QyzlEntities db = new Models.QyzlEntities();
        #region 总数据类别显示页
        public ActionResult QY_DataType()
        {
            return View();
        } 
        #endregion
        #region 总数据类别的数据加载 
        public ActionResult LoadQY_DataType(int id)
        {
            int pageSize = 10;
            int pageIndex = id;
            int rowCount = db.QY_DataType.Count();
            int pageCount = Convert.ToInt32(Math.Ceiling((rowCount * 1.0) / pageSize));
            var list = db.QY_DataType.OrderBy(d => d.id).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList().Select(d => new { ID = d.id, Name = d.name }).ToList();
            Models.PagedDataModel PageModel = new Models.PagedDataModel()
            {
                PageCount = pageCount,
                PageIndex = pageIndex,
                PageSize = pageSize,
                PagedData = list,
                RowCount = rowCount
            };
            return Json(PageModel, JsonRequestBehavior.AllowGet);
        } 
        #endregion
        #region 总数据类别新增
        public ActionResult AddQY_DataType()
        {
            Models.JsonModel jsmodel = new Models.JsonModel();
            if (!string.IsNullOrEmpty(Request.Form["datatype"].ToString()))
            {
                string datatypename = Request.Form["datatype"];
                Models.QY_DataType datatypemodel = new Models.QY_DataType() { name = datatypename };
                try
                {
                    db.QY_DataType.AddObject(datatypemodel);
                    db.SaveChanges();
                    jsmodel.statu = "ok";
                    jsmodel.msg = "新增成功";
                }
                catch
                {
                    jsmodel.statu = "err";
                    jsmodel.msg = "新增异常请联系网站管理员";
                }
            }
            else
            {
                jsmodel.statu = "fail";
                jsmodel.msg = "新增内容不能为空";
            }
            return Json(jsmodel);
        } 
        #endregion
        #region 总数据类别删除
        public ActionResult DelQY_DataType()
        {
            Models.JsonModel jsmodel = new Models.JsonModel();
            try
            {
                int id = int.Parse(Request.Form["ID"].ToString());
                var model = db.QY_DataType.Where(d => d.id == id).ToList().FirstOrDefault();
                db.QY_DataType.Attach(model);
                db.ObjectStateManager.ChangeObjectState(model, System.Data.EntityState.Deleted);
                db.SaveChanges();
                jsmodel.msg = "删除成功";
            }
            catch 
            {
                jsmodel.msg = "删除异常,请联系管理员";
            }
            return Json(jsmodel);
        } 
        #endregion
        #region 单位显示页
        public ActionResult QY_Unit()
        {
            return View();
        } 
        #endregion
        #region 单位加载
        public ActionResult LoadQY_Unit(int id)
        {
            int pageSize = 10;
            int pageIndex = id;
            int rowCount = db.QY_Unit.Count();
            int pageCount = Convert.ToInt32(Math.Ceiling((rowCount * 1.0) / pageSize));
            var list = db.QY_Unit.OrderBy(u => u.id).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList().Select(u => new { ID = u.id, Name = u.name }).ToList();
            Models.PagedDataModel PageModel = new Models.PagedDataModel()
            {
                PageCount = pageCount,
                PageIndex = pageIndex,
                PageSize = pageSize,
                PagedData = list,
                RowCount = rowCount
            };
            return Json(PageModel, JsonRequestBehavior.AllowGet);
        } 
        #endregion
        #region 单位新增
        public ActionResult AddQY_Unit()
        {
            Models.JsonModel jsmodel = new Models.JsonModel();
            if (!string.IsNullOrEmpty(Request.Form["unit"].ToString()))
            {
                string unitname = Request.Form["unit"];
                Models.QY_Unit unitmodel = new Models.QY_Unit() { name = unitname };
                try
                {
                    db.QY_Unit.AddObject(unitmodel);
                    db.SaveChanges();
                    jsmodel.statu = "ok";
                    jsmodel.msg = "新增成功";
                }
                catch 
                {
                    jsmodel.statu = "err";
                    jsmodel.msg = "新增异常请联系网站管理员";
                }
            }
            else 
            {
                jsmodel.statu="fail";
                jsmodel.msg="新增内容不能为空";
            }
            return Json(jsmodel);
        }  
        #endregion
        #region 单位删除
        public ActionResult DelQY_Unit()
        {
             Models.JsonModel jsmodel = new Models.JsonModel();
            try
            {
                int id = int.Parse(Request.Form["ID"].ToString());
                var model = db.QY_Unit.Where(d => d.id == id).ToList().FirstOrDefault();
                db.QY_Unit.Attach(model);
                db.ObjectStateManager.ChangeObjectState(model, System.Data.EntityState.Deleted);
                db.SaveChanges();
                jsmodel.msg = "删除成功";
            }
            catch 
            {
                jsmodel.msg = "删除异常,请联系管理员";
            }
            return Json(jsmodel);
        } 
        #endregion
        #region 子数据类别显示页
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
        #endregion
        #region 子类数据类别加载
        public ActionResult LoadQY_SonDataType(int id)
        {
            int pageSize = 10;
            int pageIndex = id;
            int rowCount = db.QY_SonDataType.Count();
            int pageCount = Convert.ToInt32(Math.Ceiling((rowCount * 1.0) / pageSize));
            var list = db.QY_SonDataType.OrderBy(s => s.id).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList().Select(s => new { ID = s.id, Name = s.name, Dtid = s.QY_DataType.name, DeafultUnit = s.QY_Unit.name }).ToList();
            Models.PagedDataModel PageModel = new Models.PagedDataModel()
            {
                PageCount = pageCount,
                PageIndex = pageIndex,
                PageSize = pageSize,
                PagedData = list,
                RowCount = rowCount
            };
            return Json(PageModel, JsonRequestBehavior.AllowGet);
        } 
        #endregion
        #region 子类数据新增
        public ActionResult AddQY_SonDataType()
        {
            Models.JsonModel jsmodel = new Models.JsonModel();
            if ((!string.IsNullOrEmpty(Request.Form["sonname"])) && (!string.IsNullOrEmpty(Request.Form["fname"])) && (!string.IsNullOrEmpty(Request.Form["unit"]))) 
            {
                string soname = Request.Form["sonname"];
                int fnameid = int.Parse(Request.Form["fname"].ToString());
                int unitid = int.Parse(Request.Form["unit"].ToString());
                Models.QY_SonDataType sonmodel = new Models.QY_SonDataType() {  name=soname,dtid=fnameid,defaultUnit=unitid};
                try
                {
                    db.QY_SonDataType.AddObject(sonmodel);
                    db.SaveChanges();
                    jsmodel.statu = "ok";
                    jsmodel.msg = "新增成功";
                }
                catch
                {
                    jsmodel.statu = "err";
                    jsmodel.msg = "新增异常请联系网站管理员";
                }
            }
            else
            {
                jsmodel.statu = "fail";
                jsmodel.msg = "新增内容不能为空";
            }
            return Json(jsmodel);
        } 
        #endregion
        #region 子数据删除
        public ActionResult DelQY_SonDataType()
        {
             Models.JsonModel jsmodel = new Models.JsonModel();
            try
            {
                int id = int.Parse(Request.Form["ID"].ToString());
                var model = db.QY_SonDataType.Where(d => d.id == id).ToList().FirstOrDefault();
                db.QY_SonDataType.Attach(model);
                db.ObjectStateManager.ChangeObjectState(model, System.Data.EntityState.Deleted);
                db.SaveChanges();
                jsmodel.msg = "删除成功";
            }
            catch 
            {
                jsmodel.msg = "删除异常,请联系管理员";
            }
            return Json(jsmodel);
        } 
        #endregion
        #region 子类数据关系录入 
        public ActionResult AddRelation()
        {
            Models.JsonModel jsmodel = new Models.JsonModel();
            int sdtid = int.Parse(Request.Form["son"].ToString());
            int sondtid = int.Parse(Request.Form["son1"].ToString());
            string tbname = Request.Form["tbname"].ToString();
            Models.QY_Relation relmodel = new Models.QY_Relation() { sdtid = sdtid, sonsdtid = sondtid, tabName = tbname };
            try
            {
                db.QY_Relation.AddObject(relmodel);
                db.SaveChanges();
                jsmodel.statu = "ok";
                jsmodel.msg = "新增成功";
            }
            catch
            {
                jsmodel.statu = "err";
                jsmodel.msg = "新增异常请联系网站管理员";
            }
            return Json(jsmodel);
        } 
        #endregion
        #region 年份数据显示页
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
        #endregion
        #region 年份数据总数据类别选项更改
        public ActionResult ChangeQY_YearNum(int id)
        {
            int dtid = id;
            var sellist = db.QY_SonDataType.Where(s => s.dtid == dtid).ToList().Select(s => new { id = s.id, name = s.name, unit = s.defaultUnit }).ToList();
            return Json(sellist, JsonRequestBehavior.AllowGet);
        } 
        #endregion
        #region 年份数据子数据类别更改
        public ActionResult ChangeQY_YearNum2(int id)
        {
            int sid = id;
            int unitid = Convert.ToInt32(db.QY_SonDataType.Where(s => s.id == sid).ToList().FirstOrDefault().defaultUnit);
            return Json(unitid, JsonRequestBehavior.AllowGet);
        } 
        #endregion
        #region 年份数据加载
        public ActionResult LoadQY_YearNum(int id)
        {
            int pageSize = 10;
            int pageIndex = id;
            int rowCount = db.QY_YearNum.Count();
            int pageCount = Convert.ToInt32(Math.Ceiling((rowCount * 1.0) / pageSize));
            var list = db.QY_YearNum.OrderBy(y => y.id).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList().Select(y => new { ID = y.id, Year = y.presentYear, Sdtid =y.QY_SonDataType.name,Num=y.Num ,DeafultUnit = y.QY_Unit.name }).ToList();
            Models.PagedDataModel PageModel = new Models.PagedDataModel()
            {
                PageCount = pageCount,
                PageIndex = pageIndex,
                PageSize = pageSize,
                PagedData = list,
                RowCount = rowCount
            };
            return Json(PageModel, JsonRequestBehavior.AllowGet);
        } 
        #endregion
        #region 年份数据新增
        public ActionResult AddQY_YearNum()
        {
            Models.JsonModel jsmodel = new Models.JsonModel();
            if(!string.IsNullOrEmpty(Request.Form["num"]))
            {
                try
                {
                    string year = Request.Form["year"].ToString();
                    int sdtid = int.Parse(Request.Form["son"].ToString());
                    int unitid = int.Parse(Request.Form["unit"].ToString());
                    decimal? num =(decimal?)(Convert.ToDecimal(Request.Form["num"].ToString()));
                    Models.QY_YearNum yearmodel = new Models.QY_YearNum() {presentYear=year, sdtid=sdtid, Num=num,unit=unitid};
                    db.QY_YearNum.AddObject(yearmodel);
                    db.SaveChanges();
                    jsmodel.statu = "ok";
                    jsmodel.msg = "新增成功";
                }
                catch
                {
                    jsmodel.statu = "err";
                    jsmodel.msg = "新增异常请确认年份数据为数字不是字符串或请联系网站管理员";
                }
            }
            else
            {
                jsmodel.statu = "fail";
                jsmodel.msg = "新增内容不能为空";
            }
            return Json(jsmodel);
        } 
        #endregion
        #region 年份数据删除
        public ActionResult DelQY_YearNum()
        {
            Models.JsonModel jsmodel = new Models.JsonModel();
            try
            {
                int id = int.Parse(Request.Form["ID"].ToString());
                var model = db.QY_YearNum.Where(d => d.id == id).ToList().FirstOrDefault();
                db.QY_YearNum.Attach(model);
                db.ObjectStateManager.ChangeObjectState(model, System.Data.EntityState.Deleted);
                db.SaveChanges();
                jsmodel.msg = "删除成功";
            }
            catch 
            {
                jsmodel.msg = "删除异常,请联系管理员";
            }
            return Json(jsmodel);
        } 
        #endregion
    }
}
