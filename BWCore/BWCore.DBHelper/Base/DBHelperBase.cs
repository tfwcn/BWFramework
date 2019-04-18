using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BWCore.Common;

namespace BWCore.DBHelper.Base
{
    public abstract class DBHelperBase : DBTransactionHelper.DBTransactionBase
    {
        public enum DBType { MSSQL, PostgreSql }
        #region 靜態方法
        private static Dictionary<string, DBHelperBase> commonDBHelper = new Dictionary<string, DBHelperBase>();
        /// <summary>
        /// 创建数据库连接类
        /// </summary>
        public static void CreateDBHelper(string connectionString, DBType dbType)
        {
            if (dbType == DBType.MSSQL)
            {
                commonDBHelper.Add(connectionString, new MSSQLHelper(connectionString));
            }
            else if (dbType == DBType.PostgreSql)
            {
                commonDBHelper.Add(connectionString, new PostgreSqlHelper(connectionString));
            }
        }
        /// <summary>
        /// 获得数据库连接类
        /// </summary>
        public static DBHelperBase GetDBHelper(string connectionString, DBType dbType)
        {
            if (commonDBHelper.ContainsKey(connectionString) == false)
                CreateDBHelper(connectionString, dbType);
            return commonDBHelper[connectionString];
        }
        #endregion
        protected string connectionString;
        public DBHelperBase()
        {
        }
        public DBHelperBase(string connectionString)
        {
            this.connectionString = connectionString;
        }
        protected abstract DbCommand GetCommand(string cmdText, DbConnection connection);
        protected abstract DbCommand GetCommand(string cmdText, DbConnection connection, DbTransaction transaction);
        protected abstract DbDataAdapter GetDbDataAdapter();
        protected abstract DbCommandBuilder GetDbCommandBuilder(DbDataAdapter adapter);
        public abstract DbParameter NewDbParameter(string ParameterName, DbType DbType, object Value);
        public abstract DbParameter NewDbParameter(string ParameterName, DbType DbType, object Value, int Size);
        public abstract DbParameter NewDbParameter(string ParameterName, DbType DbType, string SourceColumn);
        /// <summary>
        /// 是否存在事务
        /// </summary>
        public virtual bool HasDBTransactionHelper()
        {
            return this.dbTransaction != null;
        }
        /// <summary>
        /// 创建事务
        /// </summary>
        public virtual DBTransactionHelper.DBTransactionHelper CreateDBTransactionHelper()
        {
            if (this.dbTransaction == null)
                return new DBTransactionHelper.DBTransactionHelper(this);
            else
                throw new Exception("不能重复创建事务！");
        }
        public virtual DbType GetDbType(Type t)
        {
            DbType dbType;
            switch (t.Name.ToLower())
            {
                case "int32":
                    dbType = DbType.Int32;
                    break;
                case "string":
                    dbType = DbType.String;
                    break;
                case "boolean":
                    dbType = DbType.Boolean;
                    break;
                case "datetime":
                    dbType = DbType.DateTime;
                    break;
                case "decimal":
                    dbType = DbType.Decimal;
                    break;
                case "float":
                    dbType = DbType.Decimal;
                    break;
                default:
                    if (t.IsEnum)
                    {
                        dbType = DbType.Int32;
                        break;
                    }
                    throw new Exception("DbType轉換,未定義類型：" + t.Name);
            }
            return dbType;
        }
        public virtual void AddDbParameters(DbCommand com, DbParameter[] paramenters)
        {
            if (paramenters != null && paramenters.Length > 0)
            {
                foreach (var p in paramenters)
                {
                    if (p.Value == null)
                        p.Value = DBNull.Value;
                    com.Parameters.Add(p);
                }
            }
        }
        public virtual DbDataReader ExecuteReader(string cmdText, DbParameter[] paramenters, int? timeOut)
        {
            try
            {
                DbConnection conn = null;
                if (dbTransaction != null)
                {
                    conn = dbTransaction.Connection;
                    DbCommand com = GetCommand(cmdText, conn, dbTransaction);
                    AddDbParameters(com, paramenters);
                    if (timeOut != null)
                        com.CommandTimeout = timeOut.Value;
                    return com.ExecuteReader();
                }
                else
                {
                    conn = GetConnection();
                    conn.Open();
                    DbCommand com = GetCommand(cmdText, conn);
                    AddDbParameters(com, paramenters);
                    if (timeOut != null)
                        com.CommandTimeout = timeOut.Value;
                    return com.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public virtual int ExecuteNonQuery(string cmdText, DbParameter[] paramenters, int? timeOut)
        {
            DbConnection conn = null;
            if (dbTransaction != null)
            {
                try
                {
                    conn = dbTransaction.Connection;
                    DbCommand com = GetCommand(cmdText, conn, dbTransaction);
                    AddDbParameters(com, paramenters);
                    if (timeOut != null)
                        com.CommandTimeout = timeOut.Value;
                    return com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
            }
            else
            {
                try
                {
                    conn = GetConnection();
                    conn.Open();
                    DbCommand com = GetCommand(cmdText, conn);
                    AddDbParameters(com, paramenters);
                    if (timeOut != null)
                        com.CommandTimeout = timeOut.Value;
                    return com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
                finally
                {
                    if (conn != null)
                        conn.Close();
                }
            }
        }
        public virtual object ExecuteScalar(string cmdText, DbParameter[] paramenters, int? timeOut)
        {
            DbConnection conn = null;
            if (dbTransaction != null)
            {
                try
                {
                    conn = dbTransaction.Connection;
                    DbCommand com = GetCommand(cmdText, conn, dbTransaction);
                    AddDbParameters(com, paramenters);
                    if (timeOut != null)
                        com.CommandTimeout = timeOut.Value;
                    return com.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
            }
            else
            {
                try
                {
                    conn = GetConnection();
                    conn.Open();
                    DbCommand com = GetCommand(cmdText, conn);
                    AddDbParameters(com, paramenters);
                    if (timeOut != null)
                        com.CommandTimeout = timeOut.Value;
                    return com.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
                finally
                {
                    if (conn != null)
                        conn.Close();
                }
            }
        }
        /// <summary>
        /// 获取服务器时间
        /// </summary>
        /// <param name="com"></param>
        /// <param name="dt"></param>
        public virtual DateTime GetDBDateTime()
        {
            return Convert.ToDateTime(ExecuteScalar("select getdate()", null, null));
        }
        public virtual DataTable GetDataTable(string cmdText, DbParameter[] paramenters, int? timeOut)
        {
            try
            {
                using (DbDataReader dr = this.ExecuteReader(cmdText, paramenters, timeOut))
                {
                    DataTable dt = new DataTable("T1");
                    dt.Load(dr);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="timeOut"></param>
        public virtual void AddDataTable(DataTable dt, int? timeOut)
        {
            DbConnection conn = null;
            if (dbTransaction != null)
            {
                try
                {
                    conn = dbTransaction.Connection;
                    using (DbDataAdapter da = GetDbDataAdapter())
                    {
                        da.InsertCommand = GetCommand(null, conn, dbTransaction);
                        CreateInsertSql(da.InsertCommand, dt);
                        if (timeOut != null)
                            da.InsertCommand.CommandTimeout = timeOut.Value;//秒
                        da.InsertCommand.UpdatedRowSource = UpdateRowSource.None;//批量更新必须
                        //da.UpdateBatchSize = 0;//批量更新最大值
                        dt.SetRowsAdd();
                        da.Update(dt);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
            }
            else
            {
                try
                {
                    conn = GetConnection();
                    conn.Open();
                    using (DbTransaction t = conn.BeginTransaction())
                    {
                        using (DbDataAdapter da = GetDbDataAdapter())
                        {
                            da.InsertCommand = GetCommand(null, conn, t);
                            CreateInsertSql(da.InsertCommand, dt);
                            if (timeOut != null)
                                da.InsertCommand.CommandTimeout = timeOut.Value;//秒
                            da.InsertCommand.UpdatedRowSource = UpdateRowSource.None;//批量更新必须
                            //da.UpdateBatchSize = 0;//批量更新最大值
                            dt.SetRowsAdd();
                            da.Update(dt);
                        }
                        t.Commit();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
                finally
                {
                    if (conn != null)
                        conn.Close();
                }
            }
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="timeOut"></param>
        public virtual void UpdateDataTable(DataTable dt, int? timeOut)
        {
            DbConnection conn = null;
            if (dbTransaction != null)
            {
                try
                {
                    conn = dbTransaction.Connection;
                    using (DbDataAdapter da = GetDbDataAdapter())
                    {
                        da.UpdateCommand = GetCommand(null, conn, dbTransaction);
                        CreateUpdateSql(da.UpdateCommand, dt);
                        if (timeOut != null)
                            da.UpdateCommand.CommandTimeout = timeOut.Value;//秒
                        da.UpdateCommand.UpdatedRowSource = UpdateRowSource.None;//批量更新必须
                        //da.UpdateBatchSize = 0;//批量更新最大值
                        dt.SetRowsUpdate();
                        da.Update(dt);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
            }
            else
            {
                try
                {
                    conn = GetConnection();
                    conn.Open();
                    using (DbTransaction t = conn.BeginTransaction())
                    {
                        using (DbDataAdapter da = GetDbDataAdapter())
                        {
                            da.UpdateCommand = GetCommand(null, conn, t);
                            CreateUpdateSql(da.UpdateCommand, dt);
                            if (timeOut != null)
                                da.UpdateCommand.CommandTimeout = timeOut.Value;//秒
                            da.UpdateCommand.UpdatedRowSource = UpdateRowSource.None;//批量更新必须
                            //da.UpdateBatchSize = 0;//批量更新最大值
                            dt.SetRowsUpdate();
                            da.Update(dt);
                        }
                        t.Commit();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
                finally
                {
                    if (conn != null)
                        conn.Close();
                }
            }
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="timeOut"></param>
        public virtual void DeleteDataTable(DataTable dt, int? timeOut)
        {
            DbConnection conn = null;
            if (dbTransaction != null)
            {
                try
                {
                    conn = dbTransaction.Connection;
                    using (DbDataAdapter da = GetDbDataAdapter())
                    {
                        da.DeleteCommand = GetCommand(null, conn, dbTransaction);
                        CreateDeleteSql(da.DeleteCommand, dt);
                        if (timeOut != null)
                            da.DeleteCommand.CommandTimeout = timeOut.Value;//秒
                        da.DeleteCommand.UpdatedRowSource = UpdateRowSource.None;//批量更新必须
                        //da.UpdateBatchSize = 0;//批量更新最大值
                        dt.SetRowsDelete();
                        da.Update(dt);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
            }
            else
            {
                try
                {
                    conn = GetConnection();
                    conn.Open();
                    using (DbTransaction t = conn.BeginTransaction())
                    {
                        using (DbDataAdapter da = GetDbDataAdapter())
                        {
                            da.DeleteCommand = GetCommand(null, conn, t);
                            CreateDeleteSql(da.DeleteCommand, dt);
                            if (timeOut != null)
                                da.DeleteCommand.CommandTimeout = timeOut.Value;//秒
                            da.DeleteCommand.UpdatedRowSource = UpdateRowSource.None;//批量更新必须
                            //da.UpdateBatchSize = 0;//批量更新最大值
                            dt.SetRowsDelete();
                            da.Update(dt);
                        }
                        t.Commit();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
                finally
                {
                    if (conn != null)
                        conn.Close();
                }
            }
        }
        /// <summary>
        /// 创建查询sql
        /// </summary>
        /// <param name="tableName"></param>
        public virtual string CreateSelectSql(string tableName)
        {
            string sql = String.Format("select * from {0} ", tableName);
            return sql;
        }
        /// <summary>
        /// 创建查询条记录sql
        /// </summary>
        /// <param name="tableName"></param>
        public virtual string CreateSelectOneSql(string tableName)
        {
            string sql = String.Format("select top 1 * from {0} ", tableName);
            sql += "{0}";//预留where语句位置
            return sql;
        }
        /// <summary>
        /// 创建插入sql
        /// </summary>
        /// <param name="com"></param>
        /// <param name="dt"></param>
        protected virtual void CreateInsertSql(DbCommand com, DataTable dt)
        {
            string sql = "insert into {0}({1}) values({2})";
            string sqlCol = "";
            string sqlVal = "";
            List<DbParameter> paramenters = new List<DbParameter>();
            foreach (DataColumn col in dt.Columns)
            {
                if (col.ColumnName == "CCreateTime")//自动记录创建时间
                {
                    sqlCol += col.ColumnName + ",";
                    sqlVal += "GETDATE(),";
                }
                else
                {
                    sqlCol += col.ColumnName + ",";
                    sqlVal += "@" + col.ColumnName + ",";
                    paramenters.Add(NewDbParameter("@" + col.ColumnName, GetDbType(col.DataType), col.ColumnName));
                }
            }
            sqlCol = sqlCol.Substring(0, sqlCol.Length - 1);
            sqlVal = sqlVal.Substring(0, sqlVal.Length - 1);
            com.CommandText = String.Format(sql, dt.TableName, sqlCol, sqlVal);
            AddDbParameters(com, paramenters.ToArray());
        }
        /// <summary>
        /// 创建更新sql
        /// </summary>
        /// <param name="com"></param>
        /// <param name="dt"></param>
        protected virtual void CreateUpdateSql(DbCommand com, DataTable dt)
        {
            string sql = "update {0} set {1} where {2}";
            string sqlColVal = "";
            string sqlWhere = "";
            List<DbParameter> paramenters = new List<DbParameter>();
            foreach (DataColumn col in dt.Columns)
            {
                bool ispk = false;
                foreach (DataColumn colpk in dt.PrimaryKey)
                {
                    if (col.ColumnName == colpk.ColumnName)
                    {
                        ispk = true;
                        break;
                    }
                }
                if (ispk == false)
                {
                    if (col.ColumnName == "f_version")
                    {
                        sqlColVal += col.ColumnName + "=@" + col.ColumnName + "+1,";
                        sqlWhere += col.ColumnName + "=@" + col.ColumnName + " and ";
                    }
                    else
                    {
                        sqlColVal += col.ColumnName + "=@" + col.ColumnName + ",";
                    }
                }
                else
                {
                    sqlWhere += col.ColumnName + "=@" + col.ColumnName + " and ";
                }
                paramenters.Add(NewDbParameter("@" + col.ColumnName, GetDbType(col.DataType), col.ColumnName));
            }
            sqlColVal = sqlColVal.Substring(0, sqlColVal.Length - 1);
            sqlWhere = sqlWhere.Substring(0, sqlWhere.Length - 4);
            com.CommandText = String.Format(sql, dt.TableName, sqlColVal, sqlWhere);
            AddDbParameters(com, paramenters.ToArray());
        }
        /// <summary>
        /// 创建删除sql
        /// </summary>
        /// <param name="com"></param>
        /// <param name="dt"></param>
        protected virtual void CreateDeleteSql(DbCommand com, DataTable dt)
        {
            string sql = "delete from {0} where {1}";
            string sqlWhere = "";
            List<DbParameter> paramenters = new List<DbParameter>();
            foreach (DataColumn col in dt.Columns)
            {
                bool ispk = false;
                foreach (DataColumn colpk in dt.PrimaryKey)
                {
                    if (col.ColumnName == colpk.ColumnName)
                    {
                        ispk = true;
                        break;
                    }
                }
                if (ispk)
                {
                    sqlWhere += col.ColumnName + "=@" + col.ColumnName + " and ";
                }
                paramenters.Add(NewDbParameter("@" + col.ColumnName, GetDbType(col.DataType), col.ColumnName));
            }
            sqlWhere = sqlWhere.Substring(0, sqlWhere.Length - 4);
            com.CommandText = String.Format(sql, dt.TableName, sqlWhere);
            AddDbParameters(com, paramenters.ToArray());
        }
    }
}
