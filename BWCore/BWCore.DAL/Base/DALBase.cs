using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BWCore.Common;
using BWCore.Model;

namespace BWCore.DAL.Base
{
    public abstract class DALBase<T> where T : new()
    {
        public string ConnectionString { get; set; }
        protected BWCore.DBHelper.Base.DBHelperBase dbHelper;
        public DALBase()
        {
        }
        /// <summary>
        /// 设置连接字符串
        /// </summary>
        public void SetConnectionString(string connectionString, DBHelper.Base.DBHelperBase.DBType dbType)
        {
            this.ConnectionString = connectionString;
            dbHelper = BWCore.DBHelper.Base.DBHelperBase.GetDBHelper(connectionString, dbType);
            //获取表名
            Common.AttributeEx.DBTableAttribute tableAttribute = Attribute.GetCustomAttribute(typeof(T), typeof(Common.AttributeEx.DBTableAttribute)) as Common.AttributeEx.DBTableAttribute;
            if (tableAttribute != null && !tableAttribute.Name.IsNullOrEmpty())
            {
                tableName = tableAttribute.Name;
            }
        }
        protected string tableName = typeof(T).Name;
        /// <summary>
        /// 获取数据
        /// </summary>
        public virtual T GetModel(string id)
        {
            //获取主键名
            string pkName = typeof(T).GetPKName();
            List<DbParameter> paramenters = new List<DbParameter>();
            paramenters.Add(dbHelper.NewDbParameter(String.Format("@{0}", pkName), DbType.String, id, 36));
            return GetModel(String.Format("where {0}=@{0}", pkName), paramenters);
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
            string sqlStr;
            if (selectStr == null)
            {
                selectStr = dbHelper.CreateSelectOneSql(tableName);
                sqlStr = String.Format(selectStr, whereStr);
            }
            else
                sqlStr = String.Format("{0} {1} ", selectStr, whereStr);
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
                selectStr = dbHelper.CreateSelectSql(tableName);
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
            (model as Model.Base.ModelBase).FId = CID;
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
