using BWFramework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BWFramework.DAL
{
    public class DBTransaction : IDisposable
    {
        private BWFramework.DBHelper.Base.DBHelperBase dbHelper;
        private DBTransactionHelper.DBTransactionHelper dbTransactionHelper;
        /// <summary>
        /// 构造函数
        /// </summary>
        public DBTransaction()
        {
            this.dbHelper = BWFramework.DBHelper.Base.DBHelperBase.GetDBHelper(DBConnectionString.Default);
            this.dbTransactionHelper = this.dbHelper.CreateDBTransactionHelper();
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public DBTransaction(string connectionString)
        {
            this.dbHelper = BWFramework.DBHelper.Base.DBHelperBase.GetDBHelper(connectionString);
            this.dbTransactionHelper = this.dbHelper.CreateDBTransactionHelper();
        }
        /// <summary>
        /// 提交事务
        /// </summary>
        public void Commit()
        {
            this.dbTransactionHelper.Commit();
        }

        #region IDisposable 成员

        public void Dispose()
        {
            this.dbTransactionHelper.Dispose();
        }

        #endregion
    }
}
