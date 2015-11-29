using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaoKeBLL;
using LG.Utility;
using TaoKeModel;
using System.Text;
namespace TaoKeAdmin.Controllers {
    public class GoodInfoController : Controller {
        public ActionResult List() {
            var bll = new BGoodInfo();
            ViewBag.GoodType = bll.GetGoodTypes();
            ViewBag.SourceMall = bll.GetSourceMall();
            return View();
        }
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetList(MGoodInfoSearch search) {
            var bll = new BGoodInfo();
            var pageSize = 20;
            var pageIndex = Request.GetQ("p").GetInt(0, false);
            long dataCount = 0;
            int pageCount = 0;
            var datas = bll.GetList(pageSize, pageIndex, out dataCount, search, new OrderBy { 
                 IsAsc=false,
                 Name = "AddDateTime"
            },new OrderBy { 
                 IsAsc=false,
                 Name = "IsHot"
            });
            pageCount = (dataCount.GetInt(0,false) + pageSize - 1) / pageSize;
            return Json(new {
                pageIndex=pageIndex,
                pageCount=pageCount,
                datas = bll.GetShowDatas(datas) ?? new List<object>()
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 处理页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult HandlePage() {
            var action = Request.GetQ("action").GetString("").ToLower();
            var bll = new BGoodInfo();
            if (action.IsNullOrWhiteSpace()) return Json(new {
                Err = "Are You 弄啥呢？"
            }, JsonRequestBehavior.AllowGet);
            ViewBag.Action = action;
            ViewBag.GoodType = bll.GetGoodTypes();
            ViewBag.SourceMall = bll.GetSourceMall();
            switch (action) {
                case "add": return AddLoadPage();
                case "edit": return EditLoadPage();
            }
            return Json(new {
                Err = "Are You 弄啥呢？"
            }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 编辑界面
        /// </summary>
        /// <returns></returns>
        private ActionResult EditLoadPage() {
            var id = Request.GetQ("id").GetInt(0, false);
            if (id <= 0) return Json(new {
                Err = "商品id不能没有值！！"
            }, JsonRequestBehavior.AllowGet);
            var bll = new BGoodInfo();
            var model = bll.GetModel(id);
            ViewBag.Data = model.GetJSON();
            return View();
        }
        /// <summary>
        /// 添加界面
        /// </summary>
        /// <returns></returns>
        private ActionResult AddLoadPage() {
            ViewBag.Data = "{}";
            return View();
        }
        /// <summary>
        /// 添加商品信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddGood(MGoodInfo model) {
            if (model == null) return Json(new {
                code = "101",
                msg = "参数不能为空！！"
            }, JsonRequestBehavior.AllowGet);
            var bll = new BGoodInfo();
            int addId = 0;
            model.AddDateTime = DateTime.Now;
            var isSc = bll.Add(model, out addId);
            if (!isSc) return Json(new {
                code = "101",
                msg = "添加失败！！"
            }, JsonRequestBehavior.AllowGet);
            return Json(new {
                code = "100",
                msg = "Bingo！！"
            }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 编辑商品
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditGood(MGoodInfo model) {
            var id = Request.GetF("Id").GetInt(0, false);
            if (id <= 0) return Json(new {
                code = "101",
                msg = "需要编辑的商品ID不正确！！"
            }, JsonRequestBehavior.AllowGet);
            if (model == null) return Json(new {
                code = "101",
                msg = "参数不能为空！！"
            }, JsonRequestBehavior.AllowGet);
            var bll = new BGoodInfo();
            var isSc = bll.Edit(model);
            if (!isSc) return Json(new {
                code = "101",
                msg = "编辑失败！！"
            });
            return Json(new {
                code = "100",
                msg = "Bingo！！"
            }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 生产静态数据
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateStaticDatas() {
            var bll=new BGoodInfo();
            var datas = bll.GetList(null,new OrderBy(){
                IsAsc=true,
                Name="Type"
            },new OrderBy(){
                 IsAsc=false,
                Name="AddDateTime"
            });
            if (datas == null || datas.Count <= 0) Json(new {
                code = "101",
                msg = "数据集合为空！！"
            }, JsonRequestBehavior.AllowGet);
            StringBuilder rtnMsg=new StringBuilder();
            var msg=string.Empty;
            //分组生成
            bll.DoCreateGroupByType(datas, out msg);
            rtnMsg.Append(msg);
            msg = string.Empty;
            //所有分页
            bll.DoCreateAll(datas, out msg);
            rtnMsg.Append(msg);
            return Json(new {
                code = "100",
                msg = rtnMsg.ToString(),
            }, JsonRequestBehavior.AllowGet);
        }
        
    }
}
