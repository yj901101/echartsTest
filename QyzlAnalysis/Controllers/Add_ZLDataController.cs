using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;

namespace QyzlAnalysis.Controllers
{
    public class Add_ZLDataController : Controller
    {   
        //
        // GET: /Add_ZLData/
        Models.QyzlEntities db = new Models.QyzlEntities();
        #region 专利主类型显示页
        public ActionResult ZL_DataType()
        {
            return View();
        } 
        #endregion
        #region 专利主类型数据加载
        public ActionResult LoadZL_DataType(int id)
        {
            
            int pageSize = 10;
            int pageIndex = id;
            int rowCount = db.ZL_DataType.Count();
            int pageCount = Convert.ToInt32(Math.Ceiling((rowCount * 1.0) / pageSize));
            var list = db.ZL_DataType.OrderBy(d => d.id).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList().Select(d => new { ID = d.id, Name = d.name }).ToList();
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
        #region 专利主类型新增
        public ActionResult AddZL_DataType()
        {
            Models.JsonModel jsmodel = new Models.JsonModel();
            if (!string.IsNullOrEmpty(Request.Form["name"].ToString()))
            {
                string datatypename = Request.Form["name"].ToString().Trim();
                Models.ZL_DataType datatypemodel = new Models.ZL_DataType() { name = datatypename };
                try
                {
                    db.ZL_DataType.AddObject(datatypemodel);
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
        #region 专利主类型删除
        public ActionResult DelZL_DataType()
        {
            Models.JsonModel jsmodel = new Models.JsonModel();
            try
            {
                int id = int.Parse(Request.Form["ID"].ToString());
                var model = db.ZL_DataType.Where(d => d.id == id).ToList().FirstOrDefault();
                db.ZL_DataType.Attach(model);
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
        #region 专利分类显示页
        public ActionResult ZL_Classify()
        {
            List<Models.ZL_DataType> datatype_list = db.ZL_DataType.ToList();
            SelectList datatype_sellist = new SelectList(datatype_list, "id", "name");
            ViewData["datatype_sellist"] = datatype_sellist.AsEnumerable();
            return View();
        } 
        #endregion
        #region 专利分类加载
        public ActionResult LoadZL_Classify(int id)
        {
            int pageSize = 10;
            int pageIndex = id;
            int rowCount = db.ZL_Classify.Count();
            int pageCount = Convert.ToInt32(Math.Ceiling((rowCount * 1.0) / pageSize));
           // var list = db.ZL_Classify.Join(db.ZL_DataType, c => c.Pid, d => d.id, (c, d) => new { ID = c.id, Name = c.name, Pid = d.name, TableName = c.TableName }).ToList();
            List<Models.ZL_Classify> clist = db.ZL_Classify.OrderBy(c=>c.id).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            List<Models.zlclassifymodel> newlist = new List<Models.zlclassifymodel>();
            foreach(Models.ZL_Classify model in clist)
            {
                Models.zlclassifymodel newmodel = new Models.zlclassifymodel();
                int pid = Convert.ToInt32(model.Pid);
                Models.ZL_DataType fmodel = db.ZL_DataType.Where(d => d.id == pid).ToList().FirstOrDefault();
                newmodel.ID = model.id;
                newmodel.Name = model.name;
                newmodel.Pid = fmodel.name;
                if (model.TableName != "wgzl_ab")
                {
                    newmodel.TableName = "发明专利;发明授权;实用新型";
                }
                else
                {
                    newmodel.TableName = Common.PageHelper.GetZllxByDbname(model.TableName);
                }
                newlist.Add(newmodel);
            }
            Models.PagedDataModel PageModel = new Models.PagedDataModel()
            {
                PageCount = pageCount,
                PageIndex = pageIndex,
                PageSize = pageSize,
                PagedData = newlist,
                RowCount = rowCount
            };
            return Json(PageModel, JsonRequestBehavior.AllowGet);
        } 
        #endregion
        #region 专利分类新增
        public ActionResult AddZL_Classify()
        {
            Models.JsonModel jsmodel = new Models.JsonModel();
            if (!string.IsNullOrEmpty(Request.Form["name"].ToString()))
            {
                string name = Request.Form["name"].ToString().Trim();
                int pid = int.Parse(Request.Form["fname"].ToString());
                string dbname = Request.Form["dbname"];
                if (name.Contains("洛迦诺") && dbname != "wgzl_ab")
                {
                    jsmodel.statu = "newerr";
                    jsmodel.msg = "洛迦诺分类号只能用于外观专利";
                }
                else
                {
                    Models.ZL_Classify classifymodel = new Models.ZL_Classify() { name = name, Pid = pid,TableName = dbname};
                    try
                    {
                        db.ZL_Classify.AddObject(classifymodel);
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
            }
            else
            {
                jsmodel.statu = "fail";
                jsmodel.msg = "新增内容不能为空";
            }
            return Json(jsmodel);
        } 
        #endregion
        #region 专利分类删除
        public ActionResult DelZL_Classify()
        {
            Models.JsonModel jsmodel = new Models.JsonModel();
            try
            {
                int id = int.Parse(Request.Form["ID"].ToString());
                var model = db.ZL_Classify.Where(d => d.id == id).ToList().FirstOrDefault();
                db.ZL_Classify.Attach(model);
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
        #region 专利分类数据显示
        public ActionResult ZL_ClassifyData()
        {
            List<Models.ZL_Classify> classify_list = db.ZL_Classify.ToList();
            List<Models.ZL_ClassifyNewModel> new_classify_list = new List<Models.ZL_ClassifyNewModel>();
            foreach (Models.ZL_Classify model in classify_list)
            {
                Models.ZL_ClassifyNewModel newmodel = new Models.ZL_ClassifyNewModel();
                newmodel.id = model.id;
                string fname = db.ZL_DataType.Where(d => d.id == model.Pid).ToList().FirstOrDefault().name;
                newmodel.name = fname + "_" + model.name;
                new_classify_list.Add(newmodel);
            }
            SelectList classify_sellist = new SelectList(new_classify_list, "id", "name");
            ViewData["classify_sellist"] = classify_sellist.AsEnumerable();
            return View();
        } 
        #endregion
        #region 专利分类数据加载
        public ActionResult LoadZL_ClassifyData(int id)
        {
            int pageSize = 10;
            int pageIndex = id;
            int rowCount = db.ZL_ClassifyData.Count();
            int pageCount = Convert.ToInt32(Math.Ceiling((rowCount * 1.0) / pageSize));
            List<Models.ZL_ClassifyData> list = db.ZL_ClassifyData.OrderBy(z=>z.id).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            List<Models.ZL_ClassifyNewModel> newlist = new List<Models.ZL_ClassifyNewModel>();
            foreach (Models.ZL_ClassifyData model in list)
            {
                Models.ZL_ClassifyNewModel nmodel = new Models.ZL_ClassifyNewModel();
                int cid = Convert.ToInt32(model.Cid);
                int pid = Convert.ToInt32(db.ZL_Classify.Where(c => c.id == cid).ToList().FirstOrDefault().Pid);
                nmodel.name = db.ZL_DataType.Where(d => d.id == pid).ToList().FirstOrDefault().name + "_" + db.ZL_Classify.Where(c => c.id == cid).ToList().FirstOrDefault().name;
                nmodel.id = model.id;
                nmodel.num = model.ClassifyID;
                newlist.Add(nmodel);
            }
            Models.PagedDataModel PageModel = new Models.PagedDataModel()
            {
                PageCount = pageCount,
                PageIndex = pageIndex,
                PageSize = pageSize,
                PagedData = newlist,
                RowCount = rowCount
            };
            return Json(PageModel, JsonRequestBehavior.AllowGet);
        } 
        #endregion
        #region 专利分类数据新增
        public ActionResult AddZL_ClassifyData()
        {
            Models.JsonModel jsmodel = new Models.JsonModel();
            if (!string.IsNullOrEmpty(Request.Form["num"].ToString()))
            {
                string num = Request.Form["num"].ToString().Trim();
                int cid = int.Parse(Request.Form["fid"].ToString());
                Models.ZL_ClassifyData cdatamodel = new Models.ZL_ClassifyData() { ClassifyID = num, Cid = cid , IsToDb=false};
                try
                {
                    db.ZL_ClassifyData.AddObject(cdatamodel);
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
        #region 专利分类数据删除
        public ActionResult DelZL_ClassifyData()
        {
            Models.JsonModel jsmodel = new Models.JsonModel();
            try
            {
                int id = int.Parse(Request.Form["ID"].ToString());
                var model = db.ZL_ClassifyData.Where(d => d.id == id).ToList().FirstOrDefault();
                db.ZL_ClassifyData.Attach(model);
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
        #region 专利数据显示页
        public ActionResult ZL_Data()
        {
            List<Models.ZL_ClassifyData> list = db.ZL_ClassifyData.ToList();
            List<Models.ZL_ClassifyNewModel> new_classify_list = new List<Models.ZL_ClassifyNewModel>();
            foreach (Models.ZL_ClassifyData model in list)
            {
                Models.ZL_ClassifyNewModel newmodel = new Models.ZL_ClassifyNewModel();
                int cid = Convert.ToInt32(model.Cid);
                Models.ZL_Classify fmodel2 = db.ZL_Classify.Where(c => c.id == cid).ToList().FirstOrDefault();
                Models.ZL_DataType fmodel3 = db.ZL_DataType.Where(d => d.id == fmodel2.Pid).ToList().FirstOrDefault();
                newmodel.id = model.id;
                newmodel.name = fmodel3.name + "_" + fmodel2.name + "_" + model.ClassifyID;
                new_classify_list.Add(newmodel);
            }
            SelectList classifydata_sellist = new SelectList(new_classify_list, "id", "name");
            ViewData["classifydata_sellist"] = classifydata_sellist.AsEnumerable();
            //
            List<Models.ZL_ClassifyData> list2 = db.ZL_ClassifyData.Where(c => c.IsToDb == false).ToList();
            List<Models.ZL_ClassifyNewModel> new_classify_list2 = new List<Models.ZL_ClassifyNewModel>();
            foreach (Models.ZL_ClassifyData model in list2)
            {
                Models.ZL_ClassifyNewModel newmodel2 = new Models.ZL_ClassifyNewModel();
                int cid = Convert.ToInt32(model.Cid);
                Models.ZL_Classify fmodel2 = db.ZL_Classify.Where(c => c.id == cid).ToList().FirstOrDefault();
                Models.ZL_DataType fmodel3 = db.ZL_DataType.Where(d => d.id == fmodel2.Pid).ToList().FirstOrDefault();
                newmodel2.id = model.id;
                newmodel2.name = fmodel3.name + "_" + fmodel2.name + "_" + model.ClassifyID;
                new_classify_list2.Add(newmodel2);
            }
            SelectList classifydata_sellist2 = new SelectList(new_classify_list2, "id", "name");
            ViewData["classifydata_sellist2"] = classifydata_sellist2.AsEnumerable();
            return View();
        } 
        #endregion
        #region 专利数据加载
        public ActionResult LoadZL_Data(int id)
        {
          
            int pageSize = 10;
            int pageIndex = id;
            int rowCount = db.ZL_Data.Count();
            int pageCount = Convert.ToInt32(Math.Ceiling((rowCount * 1.0) / pageSize));
            List<Models.ZL_Data> list =db.ZL_Data.OrderBy(z=>z.id).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            List<Models.ZLDetailModel> detaillist = new List<Models.ZLDetailModel>();
            foreach (Models.ZL_Data model in list) 
            {
                Models.ZLDetailModel detailmodel = new Models.ZLDetailModel();
                detailmodel.id = model.id;
                detailmodel.an = model.AN;
                detailmodel.pd = Convert.ToDateTime(model.Pd).ToString("yyyy-MM-dd");
                detailmodel.ad = Convert.ToDateTime(model.Ad).ToString("yyyy-MM-dd");
                if (model.SqDate != null)
                {
                    detailmodel.sq = Convert.ToDateTime(model.SqDate).ToString("yyyy-MM-dd");
                }
                else 
                {
                    detailmodel.sq = null;
                }
                detailmodel.pa = model.Pa;
                detailmodel.inn = model.Inn;
                detailmodel.pic = model.Pic;
                detailmodel.sic = model.Sic;
                detailmodel.zllx = model.Zllx;
                detailmodel.flzt = model.Flzt;
                detailmodel.issq = Common.PageHelper.IS(Convert.ToBoolean(model.IsSq));
                detailmodel.isgs = Common.PageHelper.IS(Convert.ToBoolean(model.IsGs));
                detaillist.Add(detailmodel);
            }
            Models.PagedDataModel PageModel = new Models.PagedDataModel()
            {
                PageCount = pageCount,
                PageIndex = pageIndex,
                PageSize = pageSize,
                PagedData = detaillist,
                RowCount = rowCount
            };
            return Json(PageModel, JsonRequestBehavior.AllowGet);
        } 
        #endregion
        #region 专利数据新增
        public ActionResult AddZL_Data()
        {
            Models.JsonModel jsmodel = new Models.JsonModel();
            if (!string.IsNullOrEmpty(Request.Form["an"]) && !string.IsNullOrEmpty(Request.Form["pa"]) && !string.IsNullOrEmpty(Request.Form["inn"]) && !string.IsNullOrEmpty(Request.Form["pic"]) && !string.IsNullOrEmpty(Request.Form["sic"])) 
            {
                string an = Request.Form["an"].ToString().ToString().Trim();
                DateTime pd = Convert.ToDateTime(Request.Form["pd"].ToString());
                DateTime ad = Convert.ToDateTime(Request.Form["ad"].ToString());
                DateTime sqdate = Convert.ToDateTime(Request.Form["sq"].ToString());
                string pa = Request.Form["pa"].ToString().ToString().Trim();
                string inn = Request.Form["inn"].ToString().ToString().Trim();
                string pic = Request.Form["pic"].ToString().ToString().Trim();
                string sic = Request.Form["sic"].ToString().ToString().Trim();
                string zllx = Request.Form["zllx"].ToString();
                bool issq = Convert.ToBoolean(Request.Form["issq"].ToString());
                string flzt = Request.Form["flzt"].ToString();
                int cdid = int.Parse(Request.Form["fname"].ToString());
                bool isgs= Common.PageHelper.IsGs(pa);
                Models.ZL_Data zlmodel = new Models.ZL_Data();
                if (cdid == 0)
                {
                    Models.ZL_Data zlmodel2 = new Models.ZL_Data() { AN = an, Pd = pd, Ad = ad, Pa = pa, Inn = inn, Pic = pic, Sic = sic, IsSq = issq, Flzt = flzt, IsHand = true, IsGs = isgs ,Zllx=zllx,SqDate=sqdate};
                    zlmodel = zlmodel2;
                }
                else
                {
                    Models.ZL_Data zlmodel2 = new Models.ZL_Data() { AN = an, Pd = pd, Ad = ad, Pa = pa, Inn = inn, Pic = pic, Sic = sic, IsSq = issq, Flzt = flzt, IsHand = true, CDID = cdid, IsGs = isgs,Zllx=zllx,SqDate=sqdate};
                    zlmodel = zlmodel2;
                }
                try
                {
                    db.ZL_Data.AddObject(zlmodel);
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
        #region 专利数据删除
        public ActionResult DelZL_Data()
        {
            Models.JsonModel jsmodel = new Models.JsonModel();
            try
            {
                int id = int.Parse(Request.Form["ID"].ToString());
                var model = db.ZL_Data.Where(d => d.id == id).ToList().FirstOrDefault();
                db.ZL_Data.Attach(model);
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
        #region 专利数据导入
        public ActionResult InsertZlData()
        {
            int id = int.Parse(Request.Form["fname2"].ToString());
            string pic_or_sic = db.ZL_ClassifyData.Where(d => d.id == id).ToList().FirstOrDefault().ClassifyID;
            Models.JsonModel jsomodel = new Models.JsonModel();
            // Common.InsertZlHelper.GetZlJsonData(pic_or_sic,1);
            int rowcount = Common.InsertZlHelper.GetPageCount(pic_or_sic, 1);
            int pagecount = Convert.ToInt32(Math.Ceiling(rowcount * 1.0 / 10));
            if (rowcount == 0)
            {
                jsomodel.msg = "未查询到数据,导入失败";
                //Models.ZL_ClassifyData zlcl_model=db.ZL_ClassifyData.Where(c => c.id == id).ToList().FirstOrDefault();
                //zlcl_model.IsToDb = true;
                //db.ObjectStateManager.ChangeObjectState(zlcl_model, System.Data.EntityState.Modified);
                //db.SaveChanges();
            }
            else
            {
                if (pagecount == 1)
                {
                    List<Models.ZL_Data> zllist = Common.InsertZlHelper.GetZlJsonData(pic_or_sic, 1,id);
                    Common.InsertZlHelper.InsertToDB(zllist);
                }
                else
                {
                    for (int i = 1; i < pagecount; i++)
                    {
                        List<Models.ZL_Data> zllist = Common.InsertZlHelper.GetZlJsonData(pic_or_sic, i,id);
                        Common.InsertZlHelper.InsertToDB(zllist);
                    }
                    List<Models.ZL_Data> zllist_last = Common.InsertZlHelper.GetZlJsonData(pic_or_sic, pagecount,id);
                        Common.InsertZlHelper.InsertToDB(zllist_last);
                }
                Models.ZL_ClassifyData zlcl_model = db.ZL_ClassifyData.Where(c => c.id == id).ToList().FirstOrDefault();
                zlcl_model.IsToDb = true;
                db.ObjectStateManager.ChangeObjectState(zlcl_model, System.Data.EntityState.Modified);
                db.SaveChanges();
                jsomodel.msg = "数据导入成功";
            }
            return Json(jsomodel);
        } 
        #endregion
        #region 规模上公司显示
        public ActionResult ZL_GsCompany()
        {
            List<Models.ZL_DataType> datatype_list = db.ZL_DataType.ToList();
            SelectList datatype_sellist = new SelectList(datatype_list, "id", "name");
            ViewData["datatype_sellist"] = datatype_sellist.AsEnumerable();
            return View();
        } 
        #endregion
        #region  规模上公司加载
        public ActionResult LoadZL_GsCompany(int id)
        {
            int pageSize = 10;
            int pageIndex = id;
            int rowCount = db.ZL_GsCompany.Count();
            int pageCount = Convert.ToInt32(Math.Ceiling((rowCount * 1.0) / pageSize));
            var list = db.ZL_GsCompany.Join(db.ZL_DataType, g => g.did, d => d.id, (g, d) => new {ID=g.id,CName=g.name,Name=d.name }).OrderBy(z=>z.ID).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
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
        #region  规模上公司新增
        public ActionResult AddZL_GsCompany()
        {
            Models.JsonModel jsmodel = new Models.JsonModel();
            if (!string.IsNullOrEmpty(Request.Form["name"].ToString()))
            {
                string name = Request.Form["name"].ToString().Trim();
                int did = int.Parse(Request.Form["fname"].ToString());
                Models.ZL_GsCompany cmodel = new Models.ZL_GsCompany() { name = name, did = did };
                try
                {
                    db.ZL_GsCompany.AddObject(cmodel);
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
        public ActionResult DelZL_GsCompany() 
        {

            Models.JsonModel jsmodel = new Models.JsonModel();
            try
            {
                int id = int.Parse(Request.Form["ID"].ToString());
                var model = db.ZL_GsCompany.Where(d => d.id == id).ToList().FirstOrDefault();
                db.ZL_GsCompany.Attach(model);
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
        #region 所有表的批量导入
        public ActionResult InsertMuch()
        {
            Models.JsonModel jsmodel = new Models.JsonModel() { msg = "导入完毕" };
            //List<Models.ZL_ClassifyData> list = db.ZL_ClassifyData.ToList();
            //foreach (Models.ZL_ClassifyData model in list)
            //{
            //    int rowcount = Common.InsertZlHelper.GetPageCount(model.ClassifyID, 1);
            //    int pagecount = Convert.ToInt32(Math.Ceiling(rowcount * 1.0 / 10));
            //    if (rowcount != 0)
            //    {
            //        if (pagecount == 1)
            //        {
            //            List<Models.ZL_Data> zllist = Common.InsertZlHelper.GetZlJsonData(model.ClassifyID, 1, model.id);
            //            Common.InsertZlHelper.InsertToDB(zllist);
            //        }
            //        else
            //        {
            //            for (int i = 1; i < pagecount; i++)
            //            {
            //                List<Models.ZL_Data> zllist = Common.InsertZlHelper.GetZlJsonData(model.ClassifyID, i, model.id);
            //                Common.InsertZlHelper.InsertToDB(zllist);
            //            }
            //            List<Models.ZL_Data> zllist_last = Common.InsertZlHelper.GetZlJsonData(model.ClassifyID, pagecount, model.id);
            //            Common.InsertZlHelper.InsertToDB(zllist_last);
            //        }
            //    }
            //}
            return Json(jsmodel);
        } 
        #endregion
    }
}
