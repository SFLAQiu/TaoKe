using Safe.Base.DbHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaoKeModel;
using LG.Utility;
using System.Data.SqlClient;
using System.Data;
namespace TaoKeDAL {
    
    public class DGoodInfo {
        private SQLHelper _DB = DBHelper.GetTaoKeDatasDB();
        private string _Field = "Id,Title,NowPrice,OldPrice,Type,SourceMall,AddDateTime,GoodId,ImgUrl,BuyUrl";
        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="dataCount"></param>
        /// <param name="selectColumns"></param>
        /// <param name="orderBySql"></param>
        /// <returns></returns>
        public List<MGoodInfo> GetList(int pageSize, int pageIndex,out long dataCount, MGoodInfoSearch search,params OrderBy[] orderBySql) {
            List<SqlParameter> parames=new List<SqlParameter>();
            string where= GetWhere(search, ref parames);
            dataCount=0;
            var page = new SQLSuperPager("GoodInfo","", "Id", "",where);
            dataCount = _DB.ExecuteScalar(page.GetCountSQL(), parames.ToArray()).GetLong(0, false);
            return _DB.ExecuteFillDataSet(page.GetPagerSql(pageSize, pageIndex, dataCount, "*", orderBySql) , parames.ToArray()).GetModels<MGoodInfo>();
        }
        /// <summary>
        /// 获取商品对象集合
        /// </summary>
        /// <param name="search"></param>
        /// <param name="orderBySql"></param>
        /// <returns></returns>
        public List<MGoodInfo> GetList(MGoodInfoSearch search, params OrderBy[] orderBySql) {
            List<SqlParameter> parames = new List<SqlParameter>();
            string where = GetWhere(search, ref parames);
            StringBuilder order = new StringBuilder();
            if (orderBySql != null && orderBySql.Count() > 0) {
                var isNeedAnd=false;
                foreach (var item in orderBySql) {
                    if(isNeedAnd)order.Append(",");
                    order.Append(item.ToSql(item.IsAsc));
                    if(!isNeedAnd)isNeedAnd=true;
                }
            }
            var sqlStr = @"
SELECT {0}
FROM GoodInfo
{1}
{2}
".FormatStr(
     _Field,
     where.IsNullOrWhiteSpace() ? "" : "WHERE {0} ".FormatStr(where),
    order.ToString().IsNullOrWhiteSpace() ? "" : "ORDER BY {0} ".FormatStr(order.ToString())
 );

            return _DB.ExecuteFillDataSet(sqlStr,parames.ToArray()).GetModels<MGoodInfo>();
        }
        /// <summary>
        /// 获取查询语句
        /// </summary>
        /// <param name="search"></param>
        /// <param name="parames"></param>
        /// <param name="isNeedAnd"></param>
        /// <returns></returns>
        public string GetWhere(MGoodInfoSearch search, ref List<SqlParameter> parames, bool isNeedAnd = false) {
            StringBuilder whereSql = new StringBuilder();
            if (search == null) return whereSql.ToString();
            if (!search.Title.IsNullOrWhiteSpace()) {
                if (isNeedAnd) whereSql.Append(" AND ");
                whereSql.Append("Title LIKE '%'+@Title+'%'");
                parames.Add(new SqlParameter("@Title", search.Title) {
                    DbType = DbType.String
                });
                if (!isNeedAnd) isNeedAnd = true;
            }
            if (search.SourceMall.HasValue && search.SourceMall.Value.GetInt(0,false)>0) {
                if (isNeedAnd) whereSql.Append(" AND ");
                whereSql.Append("SourceMall=@SourceMall");
                parames.Add(new SqlParameter("@SourceMall", search.SourceMall) {
                    DbType = DbType.Int32
                });
                if (!isNeedAnd) isNeedAnd = true;
            }
            if (search.Type.HasValue && search.Type.Value.GetInt(0, false) > 0) {
                if (isNeedAnd) whereSql.Append(" AND ");
                whereSql.Append("Type=@Type");
                parames.Add(new SqlParameter("@Type", search.Type) {
                    DbType = DbType.Int32
                });
                if (!isNeedAnd) isNeedAnd = true;
            }
            return whereSql.ToString();
        }
        /// <summary>
        /// 获取商品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MGoodInfo GetModel(int id) {
            if (id <= 0) return null;
            var str = @"
SELECT {0}
FROM dbo.GoodInfo
WHERE id=@Id
".FormatStr(_Field);
            SqlParameter[] parames ={
                new SqlParameter("@Id",id){ SqlDbType=SqlDbType.Int}
            };
            return _DB.ExecuteFillDataSet(str, parames).GetModel<MGoodInfo>();
        }

        /// <summary>
        /// 获取商品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Add(MGoodInfo model, out int id) {
            id = 0;
            if (model == null) return false;
            var str = @"
INSERT INTO GoodInfo(Title,NowPrice,OldPrice,Type,SourceMall,AddDateTime,GoodId,ImgUrl,BuyUrl) 
VALUES (@Title,@NowPrice,@OldPrice,@Type,@SourceMall,@AddDateTime,@GoodId,@ImgUrl,@BuyUrl)
SELECT SCOPE_IDENTITY();
";
            SqlParameter[] parames ={
                new SqlParameter("@Title",model.Title){ DbType=DbType.String},
                new SqlParameter("@NowPrice",model.NowPrice){ DbType=DbType.Decimal},
                new SqlParameter("@OldPrice",model.OldPrice){ DbType=DbType.Decimal},
                new SqlParameter("@Type",model.Type){ DbType=DbType.Int32},
                new SqlParameter("@SourceMall",model.SourceMall){ DbType=DbType.Int32},
                new SqlParameter("@AddDateTime",model.AddDateTime){ DbType=DbType.DateTime},
                new SqlParameter("@GoodId",model.GoodId){ DbType=DbType.String},
                new SqlParameter("@ImgUrl",model.ImgUrl){ DbType=DbType.String},
                 new SqlParameter("@BuyUrl",model.BuyUrl){ DbType=DbType.String}
            };
            id = _DB.ExecuteScalar(str, parames).GetInt(0, false);
            return id > 0;
        }

        /// <summary>
        /// 获取商品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Edit(MGoodInfo model) {
            var str = @"
UPDATE GoodInfo
SET 
    Title=@Title,
    NowPrice=@NowPrice,
    OldPrice=@OldPrice,
    Type=@Type,
    SourceMall=@SourceMall,
    GoodId=@GoodId,
    ImgUrl=@ImgUrl,
    BuyUrl=@BuyUrl
WHERE Id=@Id
";
            SqlParameter[] parames ={
                new SqlParameter("@Id",model.Id){ DbType=DbType.Int32},
                new SqlParameter("@Title",model.Title){ DbType=DbType.String},
                new SqlParameter("@NowPrice",model.NowPrice){ DbType=DbType.Decimal},
                new SqlParameter("@OldPrice",model.OldPrice){ DbType=DbType.Decimal},
                new SqlParameter("@Type",model.Type){ DbType=DbType.Int32},
                new SqlParameter("@SourceMall",model.SourceMall){ DbType=DbType.Int32},
                new SqlParameter("@GoodId",model.GoodId){ DbType=DbType.String},
                new SqlParameter("@ImgUrl",model.ImgUrl){ DbType=DbType.String},
                new SqlParameter("@BuyUrl",model.BuyUrl){ DbType=DbType.String}
            };
            return _DB.ExecuteNonQuery(str, parames)>0;
        }


    }
}
