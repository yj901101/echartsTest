using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QyzlAnalysis.Controllers
{
    [Common.Atrributes.Skip]
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        Models.QyzlEntities db = new Models.QyzlEntities();
        public ActionResult Index() 
        {
            return View();
        }
        public ActionResult Login()
        {
            string UName = "";
            string UPwd = "";
            string VCode = "";
            try
            {
                UName = Request.Form["uname"];
                UPwd = Request.Form["upwd"];
                VCode = Request.Form["vcode"];
            }
            catch {  VCode = Session["vcode"].ToString(); }
            string vcode = Session["vcode"].ToString();
            Models.JsonModel jsmodel = new Models.JsonModel();
            if (vcode != VCode)
            {
                jsmodel.statu = "falsevd";
                jsmodel.msg = "验证码错误";
            }
            else
            {
                Models.User user = db.User.Where(u => u.UName == UName).ToList().FirstOrDefault();
                if (user == null)
                {

                    jsmodel.statu = "falseuser";
                    jsmodel.msg = "用户不存在";
                }
                else 
                {
                    if (user.UPwd != UPwd)
                    {
                        jsmodel.statu = "falsepwd";
                        jsmodel.msg = "密码错误";
                    }
                    else 
                    {
                        Session["uinfo"] = user;
                        if (!string.IsNullOrEmpty(Request.Form["always"]))
                        {
                            HttpCookie cookie = new HttpCookie("ucookie",user.ID.ToString());
                            cookie.Expires = DateTime.Now.AddDays(1);
                            Response.Cookies.Add(cookie);
                        }
                        jsmodel.statu = "ok";
                        jsmodel.msg = "登陆成功";
                        jsmodel.backurl = "Home/Index";
                    }
                }
            }
            
            return Json(jsmodel);
        }

    }
}
