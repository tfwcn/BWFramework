using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BWFramework.Common;
using BWFramework.Model;

namespace BWFramework.DAL.Base
{
    public abstract class DALBase<T> where T : new()
    {
        protected BWFramework.DBHelper.Base.DBHelperBase dbHelper;
        protected string tableName = typeof(T).Name;
        /// <summary>
        /// 构造函数
        /// </summary>
        public DALBase()
        {
            dbHelper = BWFramework.DBHelper.Base.DBHelperBase.GetDBHelper(DBConnectionString.Default);
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public DALBase(string connectionString)
        {
            dbHelper = BWFramework.DBHelper.Base.DBHelperBase.GetDBHelper(connectionString);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        public virtual T GetModel(string CID)
        {
            List<DbParameter> paramenters = new List<DbParameter>();
            paramenters.Add(dbHelper.NewDbParameter("@CID", DbType.String, CID, 36));
            return GetModel("where CID=@CID", paramenters);
        }
        /// <summary>
        /// 获取数据(抽象方法)
        /// </summary>
        /// <param name="whereStr">格式：where ... order by ...</param>
        protected virtual T GetModel(string whereStr, List<DbParameter> paramenters)
        {
            return GetModel(null, whereStr, paramenters, null);
        }
        /// <summary>
        /// 获取数据(抽象方法)
        /// </summary>
        /// <param name="selectStr">格式：select ... from ...</param>
        /// <param name="whereStr">格式：where ... order by ...</param>
        protected virtual T GetModel(string selectStr, string whereStr, List<DbParameter> paramenters)
        {
            return GetModel(selectStr, whereStr, paramenters, null);
        }
        protected virtual T GetModel(string selectStr, string whereStr, List<DbParameter> paramenters, int? timeOut)
        {
            if (selectStr == null)
                selectStr = String.Format("select top 1 * from {0} ", tableName);
            string sqlStr = String.Format("{0} {1} ", selectStr, whereStr);
            if (paramenters == null)
                paramenters = new List<DbParameter>();
            DataTable dt = dbHelper.GetDataTable(sqlStr, paramenters.ToArray(), timeOut);
            dt.TableName = tableName;
            if (dt.Rows.Count == 0)
                return default(T);
            T model = dt.Rows[0].DataRowToModel<T>(true, null);
            if (model is Model.Base.ModelBase)
            {
                (model as Model.Base.ModelBase).SaveOldValues();//保存旧值
            }
            return model;
        }
        /// <summary>
        /// 获取数据列表(抽象方法)
        /// </summary>
        /// <param name="whereStr">格式：where ... order by ...</param>
        protected virtual List<T> GetModels(string whereStr, List<DbParameter> paramenters)
        {
            return GetModels(null, whereStr, paramenters, null);
        }
        /// <summary>
        /// 获取数据列表(抽象方法)
        /// </summary>
        /// <param name="selectStr">格式：select ... from ...</param>
        /// <param name="whereStr">格式：where ... order by ...</param>
        protected virtual List<T> GetModels(string selectStr, string whereStr, List<DbParameter> paramenters)
        {
            return GetModels(selectStr, whereStr, paramenters, null);
        }
        protected virtual List<T> GetModels(string selectStr, string whereStr, List<DbParameter> paramenters, int? timeOut)
        {
            if (selectStr == null)
                selectStr = String.Format("select * from {0} ", tableName);
            string sqlStr = String.Format("{0} {1} ", selectStr, whereStr);
            if (paramenters == null)
                paramenters = new List<DbParameter>();
            DataTable dt = dbHelper.GetDataTable(sqlStr, paramenters.ToArray(), timeOut);
            dt.TableName = tableName;
            List<T> list = dt.DataTableToList<T>(true, null);
            foreach (T model in list)
            {
                if (model is Model.Base.ModelBase)
                {
                    (model as Model.Base.ModelBase).SaveOldValues();//保存旧值
                }
            }
            return list;
        }
        /// <summary>
        /// 分页获取数据列表(抽象方法)
        /// </summary>
        /// <param name="pageNum">页码</param>
        /// <param name="pageSize">行数</param>
        protected virtual List<T> GetModelsPage(int pageNum, int pageSize, string whereStr, string orderByStr, List<DbParameter> paramenters)
        {
            return GetModelsPage(pageNum, pageSize, null, whereStr, orderByStr, paramenters, null);
        }
        protected virtual List<T> GetModelsPage(int pageNum, int pageSize, string whereStr, string orderByStr, List<DbParameter> paramenters, int? timeOut)
        {
            return GetModelsPage(pageNum, pageSize, null, whereStr, orderByStr, paramenters, timeOut);
        }
        protected virtual List<T> GetModelsPage(int pageNum, int pageSize, string selectStr, string whereStr, string orderByStr, List<DbParameter> paramenters, int? timeOut)
        {
            int indexBegin = (pageNum - 1) * pageSize;
            int indexEnd = pageNum * pageSize;
            if (orderByStr == null)
                orderByStr = "order by CCreateTime";
            if (selectStr == null)
                selectStr = String.Format("select top {2} ROW_NUMBER() over ({1}) as pageNum,* from {0}", tableName, orderByStr, indexEnd);
            else
            {
                int indexSelect = selectStr.ToLower().IndexOf("select ");
                if (indexSelect == -1)
                    throw new Exception("查询语句有误，无法分页！");
                selectStr = selectStr.Insert(indexSelect, String.Format("top {1} ROW_NUMBER() over ({0}) as pageNum,", orderByStr, indexEnd));
            }
            string sqlStr = String.Format("select * from ({0} {1}) as pageT1 where pageNum>{2} and pageNum<={3} order by pageNum", selectStr, whereStr, indexBegin, indexEnd);
            return GetModels(sqlStr, null, paramenters, timeOut);
        }
        /// <summary>
        /// 新增数据
        /// </summary>
        public virtual void Add(T model)
        {
            List<T> list = new List<T>();
            list.Add(model);
            Add(list);
        }
        public virtual void Add(List<T> list)
        {
            dbHelper.AddDataTable(list.ListToDataTable<T>(true, true), null);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        public virtual void Update(T model)
        {
            List<T> list = new List<T>();
            list.Add(model);
            Update(list);
        }
        public virtual void Update(List<T> list)
        {
            dbHelper.UpdateDataTable(list.ListToDataTable<T>(true, true), null);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        public virtual void Delete(string CID)
        {
            List<T> list = new List<T>();
            T model = new T();
            (model as Model.Base.ModelBase).CID = CID;
            list.Add(model);
            Delete(list);
        }
        public virtual void Delete(T model)
        {
            List<T> list = new List<T>();
            list.Add(model);
            Delete(list);
        }
        public virtual void Delete(List<T> list)
        {
            dbHelper.DeleteDataTable(list.ListToDataTable<T>(true, true), null);
        }
        /// <summary>
        /// 创建事务
        /// </summary>
        public DBTransactionHelper.DBTransactionHelper CreateDBTransactionHelper()
        {
            return dbHelper.CreateDBTransactionHelper();
        }
    }
}
