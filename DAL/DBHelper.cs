using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Safe.Base.DbHelper;
using System.Configuration;
using LG.Utility;
using System.Configuration;
namespace TaoKeDAL {
    public class DBHelper {
        public static SQLHelper GetTaoKeDatasDB() {
            var connectionStr = ConfigurationManager.AppSettings["SqlConnectionStr"].GetString("");
            return new SQLHelper(connectionStr, true);
        }

        #region
        /// <summary>
        /// 根据top值返回" top xx " SQL
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        public string GetTopStr(int top) { 
            string topSql=string.Empty;
            if (top > 0) topSql = " LIMIT {0} ".FormatStr(top);
            return topSql;
        }
        /// <summary>
        /// 更具排序集合返回SQL
        /// </summary>
        /// <param name="orders"></param>
        /// <returns></returns>
        public string GetOrderStr(List<OrderBy> orders) {
            StringBuilder orderSql = new StringBuilder();
            if (orders == null || orders.Count <= 0) return orderSql.ToString();
            bool isNeedComma = false;
            foreach (var item in orders) {
                if (isNeedComma) orderSql.Append(",");
                orderSql.Append(item.ToSql(false));
                isNeedComma = true;
            }
            return orderSql.ToString();
        }
        /// <summary>
        /// 获取数据行的SQL语句
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fields"></param>
        /// <param name="whereSql"></param>
        /// <returns></returns>
        public string GetModelSQL(string tableName,string fields,string whereSql) {
            var sqlStr = @"
SELECT {0}
FROM {1}
WHERE {2}
".FormatStr(
    fields,
    tableName,
    whereSql
 );
            return sqlStr;
        }

        /// <summary>
        /// 获取删除的SQL语句
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fields"></param>
        /// <param name="whereSql"></param>
        /// <returns></returns>
        public string GetDeleteSQL(string tableName,string whereSql) {
            var sqlStr = @"
DELETE FROM {0}
WHERE {1}
".FormatStr(
    tableName,
    whereSql
 );
            return sqlStr;
        }
        /// <summary>
        /// 获取top SQL
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fields"></param>
        /// <param name="whereSql"></param>
        /// <param name="orderSql"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public string GetTopSQL(string tableName,string fields,string whereSql, string orderSql, int top) {
            StringBuilder sqlStr = new StringBuilder(); 
            sqlStr.Append(@" 
SELECT {0} 
FROM {1}
{2}
{3}
{4}
".FormatStr(
 fields,
 tableName,
 whereSql.IsNullOrWhiteSpace() ? string.Empty : " WHERE " + whereSql,
 orderSql.IsNullOrWhiteSpace() ? string.Empty : " ORDER BY " + orderSql,
 GetTopStr(top)
));
            return sqlStr.ToString();
        }
        #endregion
    }
    public class MYSQLPageHelper{
        private string _TableName;
        private string _WhereSql;
        private string _Field;
        private string _PrimaryKey;
        public MYSQLPageHelper(string tableName, string primaryKey, string field, string whereSql) {
            _TableName = tableName;
            _WhereSql = whereSql;
            _Field = field;
            _PrimaryKey = primaryKey;
        }
        /// <summary>
        /// 获取数据源SQL
        /// </summary>
        /// <returns></returns>
        public string GetCountSQL() {
            string whereSql=_WhereSql.IsNullOrWhiteSpace()?"":" WHERE {0}".FormatStr(_WhereSql);
            string sqlStr = @"
SELECT COUNT({0})
FROM {1} 
{2}
".FormatStr(
     _PrimaryKey,
     _TableName,
      whereSql
  );
            return sqlStr;
        }
        /// <summary>
        /// 获取分页SQL
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="dataCount"></param>
        /// <param name="orderSQL"></param>
        /// <returns></returns>
        public string GetPageSQL(int pageSize,int pageIndex,long dataCount,string orderSQL) {
            string whereSql=_WhereSql.IsNullOrWhiteSpace()?"":" WHERE {0}".FormatStr(_WhereSql);
            string orderSql = orderSQL.IsNullOrWhiteSpace() ? "" : " ORDER BY {0} ".FormatStr(orderSQL);
            var showPageIndex = (pageIndex - 1) > 0 ? (pageIndex - 1) : 0;
            string limitSql = " LIMIT {0},{1} ".FormatStr(showPageIndex,pageSize);
            string sqlStr = @"
SELECT {0} 
FROM {1} 
{2} 
{3}
{4}
".FormatStr(
    _Field,
    _TableName,
    whereSql,
    orderSql,
    limitSql
 );
            return sqlStr;
        }

      
    }
}
