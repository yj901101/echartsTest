using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QyzlAnalysis.Models;

namespace QyzlAnalysis.Controllers
{
    public class SubSectionController : Controller
    {
        //
        // GET: /SubSection/
        QyzlEntities nec = new QyzlEntities();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(int? dtid)
        {
            ZL_ViewType lz = nec.ZL_ViewType.First(u => u.ZDID == dtid);
            int lzid = lz.id;
            List<ZL_ViewSonType> lvs = nec.ZL_ViewSonType.Where(u => u.zvid == lzid).ToList();
            ViewData["lvsdata"] = lvs;
            return View();
        }
    }
}
