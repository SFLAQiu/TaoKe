using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaoKeBLL;
using LG.Utility;
namespace TaoKe.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            var bll = new BGoodInfo();
            var type = Request.GetQ("t").GetString("all");
            ViewBag.GoodType = bll.GetGoodTypes();
            ViewBag.Type = type; 
            //ViewBag.SourceMall = bll.GetSourceMall();

            return View();
        }
    }
}
