using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QyzlAnalysis.Models;

namespace QyzlAnalysis.Controllers
{
    public class QyTableController : Controller
    {
        //
        // GET: /QyTable/
        QyzlEntities ta = new QyzlEntities();
        public ActionResult Index()
        {
            var mess = ta.QY_DataType.Where(u=>u.id<=23).ToList();
            ViewData["DataType"] = mess;
            //专利生成图
            var datatestmess1 =(from n1 in ta.QY_DataType
                              where n1.id >28 && n1.defaultType ==1
                                   select n1).ToList();
            ViewData["datatestDataType1"] = datatestmess1;
            var datatestmess2 = (from n1 in ta.QY_DataType
                                 where n1.id > 28 && n1.defaultType == 2
                                 select n1).ToList();
            ViewData["datatestDataType2"] = datatestmess2;
            var datatestmess3 = (from n1 in ta.QY_DataType
                                 where n1.id > 28 && n1.defaultType == 3
                                 select n1).ToList();
            ViewData["datatestDataType3"] = datatestmess3;
            var zlmess = ta.ZL_DataType.Where(u=>u.id!=5).ToList();//剔除马鞍山专利
            ViewData["ZLDataType"] = zlmess;
            Models.User usermodel = Session["uinfo"] as Models.User;
            if (usermodel != null)
            {
                ViewData["UName"] = usermodel.UName;
                ViewData["URole"] = usermodel.URole;
            }
            else
            {
                ViewData["UName"] = "";
                ViewData["URole"] = "";
            }
            return View();
        }
        #region 用户退出
        public ActionResult Quit()
        {
            string uname = Request.QueryString["uname"];
            Models.JsonModel jsmodel = new Models.JsonModel();
            Models.User user = Session["uinfo"] as Models.User;
            if (user.UName != uname)
            {
                jsmodel.statu = "fail";
                jsmodel.msg = "退出失败，用户信息错误，请联系网站管理员";
            }
            else
            {
                Session["uinfo"] = null;
                HttpCookie mycookie = Request.Cookies["ucookie"];
                if (mycookie != null)
                {
                    mycookie.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(mycookie);
                }
                jsmodel.statu = "ok";
                jsmodel.msg = "退出成功";
                jsmodel.backurl = "Login/Index";
            }
            return Json(jsmodel, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}
