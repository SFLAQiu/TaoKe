using LG.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using TaoKeDAL;
using TaoKeModel;
namespace TaoKeBLL {
    public class BGoodInfo {
       private DGoodInfo _dal = new DGoodInfo();
        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="dataCount"></param>
        /// <param name="selectColumns"></param>
        /// <param name="orderBySql"></param>
        /// <returns></returns>
       public List<MGoodInfo> GetList(int pageSize, int pageIndex, out long dataCount, MGoodInfoSearch search, params OrderBy[] orderBySql) {
           return _dal.GetList(pageSize, pageIndex, out dataCount, search, orderBySql);
        }
        /// <summary>
        /// 获取商品对象集合
        /// </summary>
        /// <param name="search"></param>
        /// <param name="orderBySql"></param>
        /// <returns></returns>
       public List<MGoodInfo> GetList(MGoodInfoSearch search, params OrderBy[] orderBySql) { 
         return _dal.GetList(search, orderBySql);
       }
        public List<Object> GetShowDatas(List<MGoodInfo>  datas) {
            if (datas == null) return null;
            List<Object> rtnDatas = new List<object>();
            foreach (var item in datas) {
                rtnDatas.Add(new {
                    Id = item.Id,
                    Title = item.Title,
                    NowPrice = item.NowPrice,
                    OldPrice = item.OldPrice,
                    SourceMall = item.SourceMall.GetEnumAttr() != null ? item.SourceMall.GetEnumAttr().Text : "无",
                    Type = item.Type.GetEnumAttr() != null ? item.Type.GetEnumAttr().Text : "无",
                    AddDateTime = item.AddDateTime.GetString("yyyy-MM-dd HH:mm:ss"),
                    GoodId = item.GoodId,
                    ImgUrl=item.ImgUrl,
                    BuyUrl=item.BuyUrl
                });
            }
            return rtnDatas;
        }
        /// <summary>
        /// 获取商品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MGoodInfo GetModel(int id) {
            return _dal.GetModel(id);
        }

        /// <summary>
        /// 添加商品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Add(MGoodInfo model, out int id) {
            return _dal.Add(model,out id);
        }
        
        /// <summary>
        /// 编辑商品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Edit(MGoodInfo model) {
            return _dal.Edit(model);
        }

        public List<ESourceMall> GetSourceMall() {
            return EnumHelper<ESourceMall>.GetAllItem();
        }
        public List<EGoodType> GetGoodTypes() {
            return EnumHelper<EGoodType>.GetAllItem();
        }

        #region "生产静态文件"
        /// <summary>
        /// 根据类型分组生成静态JSON
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="msg"></param>
        public void DoCreateGroupByType(List<MGoodInfo> datas, out string msg) {
            msg = string.Empty;
            if (datas == null || datas.Count <= 0) return;
            var groupDatas = datas.GroupBy(d => d.Type);
            StringBuilder createMsg = new StringBuilder();
            //根据类型分页生产
            var pageSize = 20;
            List<object> infos = new List<object>();
            infos.Add(new {
                Type = "all",
                AllCount = datas.Count(),
                MaxPage = (datas.Count() + pageSize - 1) / pageSize
            });
            foreach (var gItems in groupDatas) {
                var type = gItems.Key;
                var dataCount = gItems.Count();
                var pageCount = (gItems.Count() + pageSize - 1) / pageSize;
                createMsg.Append("=>生成:[{0}],总页数:{1} \r\n".FormatStr(type.GetEnumAttr().Text, pageCount));
                for (int i = 1; i <= pageCount; i++) {
                    var pageDatas = LinqHelper.GetIenumberable(gItems, null, null, pageSize, i, out dataCount);
                    var filePath = "datas/goods/info_{0}_{1}.js".FormatStr(type.GetInt(0, false), i);
                    StaticFunctions.WriteStringToFile("{0}{1}".FormatStr(
                        HttpContext.Current.Request.MapPath("~"),
                        filePath
                    ), pageDatas.GetJSON(), false, Encoding.UTF8);
                    infos.Add(new {
                        Type = type,
                        AllCount = gItems.Count(),
                        MaxPage = (gItems.Count() + pageSize - 1) / pageSize
                    });
                    createMsg.Append("|页码:{0},生成成功！！".FormatStr(i));
                }
            }
            //生成商品类型|所有商品统计数据
            StaticFunctions.WriteStringToFile("{0}{1}".FormatStr(
                HttpContext.Current.Request.MapPath("~"),
                "datas/goods/info.js"
            ), "window.GoodsInfo={0};".FormatStr(infos.GetJSON()), false, Encoding.UTF8);
            msg = createMsg.ToString();

        }
        /// <summary>
        /// 生产所有商品静态JSON
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="msg"></param>
        public void DoCreateAll(List<MGoodInfo> datas, out string msg) {
            msg = string.Empty;
            if (datas == null || datas.Count <= 0) return;
            var pageSize = 20;
            StringBuilder createMsg = new StringBuilder();
            var dataCount = datas.Count();
            var pageCount = (datas.Count() + pageSize - 1) / pageSize;
            datas = datas.OrderBy(d => d.AddDateTime).ToList();
            createMsg.Append("=>生成:[所有],总页数:{0} \r\n".FormatStr(pageCount));
            for (int i = 1; i <= pageCount; i++) {
                var pageDatas = LinqHelper.GetIenumberable(datas, null, null, pageSize, i, out dataCount);
                var filePath = "datas/goods/info_all_{0}.js".FormatStr(i);
                StaticFunctions.WriteStringToFile("{0}{1}".FormatStr(
                   HttpContext.Current.Request.MapPath("~"),
                    filePath
                ), pageDatas.GetJSON(), false, Encoding.UTF8);
                createMsg.Append("|页码:{0},生成成功！！".FormatStr(i));
            }
            msg = createMsg.ToString();
        }
        #endregion
    }
}
