using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BWCore.DBTransactionHelper
{
    public class DBTransactionHelper : IDisposable
    {
        private DbConnection conn;
        private DbTransaction dbTransaction;
        private DBTransactionBase dbDBHelper;
        public DBTransactionHelper(DBTransactionBase dbDBHelper)
        {
            this.dbDBHelper = dbDBHelper;
            conn = dbDBHelper.GetConnection();
            conn.Open();
            this.dbTransaction = conn.BeginTransaction();
            dbDBHelper.dbTransaction = this.dbTransaction;
        }
        public void Commit()
        {
            this.dbTransaction.Commit();
        }

        #region IDisposable 成员

        public void Dispose()
        {
            this.conn.Close();
            this.conn = null;
            this.dbDBHelper.dbTransaction = null;
            this.dbDBHelper = null;
            this.dbTransaction = null;
        }

        #endregion
    }
}
