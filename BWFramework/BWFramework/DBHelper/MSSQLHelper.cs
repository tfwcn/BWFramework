using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;

namespace BWFramework.DBHelper
{
    /// <summary>
    /// 数据库操作基类(for PostgreSQL)
    /// </summary>
    public class MSSQLHelper : DBHelper.Base.DBHelperBase
    {
        public override DbConnection GetConnection()
        {
            this.connectionString = "Data Source=.;Initial Catalog=DBTest;Persist Security Info=True;"
                + "User ID=sa;Password=sa;";
            SqlConnection conn = new SqlConnection(connectionString);
            return conn;
        }
        protected override DbCommand GetCommand(string cmdText, DbConnection connection)
        {
            SqlConnection conn = connection as SqlConnection;
            SqlCommand com = new SqlCommand(cmdText, conn);
            return com;
        }
        protected override DbCommand GetCommand(string cmdText, DbConnection connection, DbTransaction transaction)
        {
            SqlConnection conn = connection as SqlConnection;
            SqlCommand com = new SqlCommand(cmdText, conn, transaction as SqlTransaction);
            return com;
        }
        public override DbParameter NewDbParameter(string ParameterName, DbType DbType,object Value )
        {
            return NewDbParameter(ParameterName, DbType, Value, -1);
        }
        public override DbParameter NewDbParameter(string ParameterName, DbType DbType, object Value,int Size)
        {
            return new SqlParameter() { ParameterName = ParameterName, DbType = DbType, Value = Value, Size = Size };
        }
        public override DbParameter NewDbParameter(string ParameterName, DbType DbType, string SourceColumn)
        {
            return new SqlParameter() { ParameterName = ParameterName, DbType = DbType, SourceColumn = SourceColumn };
        }

        protected override DbDataAdapter GetDbDataAdapter()
        {
            return new SqlDataAdapter();
        }

        protected override DbCommandBuilder GetDbCommandBuilder(DbDataAdapter adapter)
        {
            return new SqlCommandBuilder(adapter as SqlDataAdapter);
        }
    }
}
