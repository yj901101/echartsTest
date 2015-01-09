using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QyzlAnalysis.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        #region 用户登录页面初始化
        public ActionResult Index()
        {
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
        #endregion
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
        #region iframe欢迎页
        public ActionResult Show()
        {
            return View();
        } 
        #endregion
    }
}
