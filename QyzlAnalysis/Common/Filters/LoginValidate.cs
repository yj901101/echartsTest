using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QyzlAnalysis.Common.Filters
{
    public class LoginValidate:System.Web.Mvc.AuthorizeAttribute
    {
        Models.QyzlEntities db = new Models.QyzlEntities();
        public override void OnAuthorization(System.Web.Mvc.AuthorizationContext filterContext)
        {
            var  routedata = filterContext.RouteData.Values.Values.ToArray();
            string controllername = routedata[0].ToString();
            string actionname = routedata[1].ToString();
            //base.OnAuthorization(filterContext);
            if (controllername == "Add_QYData" || controllername == "Add_ZLData"||(controllername =="Home"&&actionname=="Index"))
            {
                if (!filterContext.ActionDescriptor.IsDefined(typeof(Common.Atrributes.SkipAttribute), false) && !filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(Common.Atrributes.SkipAttribute), false))
                {
                    if (HttpContext.Current.Session["uinfo"] == null)
                    {
                        if (HttpContext.Current.Request.Cookies["ucookie"] == null)
                        {
                            HttpContext.Current.Response.Write("<script>alert(\"还未登陆，无权限访问~\");</script>");
                            string url = "Login/Index";
                            HttpContext.Current.Response.Write("<script>window.parent.location.href=(\"../../" + url + "\")</script>");
                        }
                        else
                        {
                            int id = Convert.ToInt32(HttpContext.Current.Request.Cookies["ucookie"].Value);
                            Models.User user = db.User.Where(u => u.ID == id).ToList().FirstOrDefault();
                            HttpContext.Current.Session["uinfo"] = user;
                            if (user.URole != "root")
                            {
                                HttpContext.Current.Response.Write("<script>alert(\"你木有足够的权限，只有root用户才能访问~\");</script>");
                                string url = "QyTable/Index";
                                HttpContext.Current.Response.Write("<script>window.parent.location.href=(\"../../" + url + "\")</script>");
                            }
                        }
                    }
                    else
                    {
                        Models.User user = HttpContext.Current.Session["uinfo"] as Models.User;
                        if (user.URole != "root")
                        {
                            HttpContext.Current.Response.Write("<script>alert(\"你木有足够的权限，只有root用户才能访问~\");</script>");
                            string url = "QyTable/Index";
                            HttpContext.Current.Response.Write("<script>window.parent.location.href=(\"../../" + url + "\")</script>");
                        }
                    }
                }
            }
            else 
            {
                if (!filterContext.ActionDescriptor.IsDefined(typeof(Common.Atrributes.SkipAttribute), false) && !filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(Common.Atrributes.SkipAttribute), false))
                {
                    if (HttpContext.Current.Session["uinfo"] == null)
                    {
                        if (HttpContext.Current.Request.Cookies["ucookie"] == null)
                        {
                            HttpContext.Current.Response.Write("<script>alert(\"还未登陆，无权限访问~\");</script>");
                            string url = "Login/Index";
                            HttpContext.Current.Response.Write("<script>window.parent.location.href=(\"../../" + url + "\")</script>");
                        }
                        else
                        {
                            int id = Convert.ToInt32(HttpContext.Current.Request.Cookies["ucookie"].Value);
                            Models.User user = db.User.Where(u => u.ID == id).ToList().FirstOrDefault();
                            HttpContext.Current.Session["uinfo"] = user;
                        }
                    }

                }
            }
        }
    }
}