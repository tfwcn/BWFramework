using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BWFramework.BLL
{
    /// <summary>
    /// 启动事务，多线程时加上Mutex保证进程互斥
    /// </summary>
    public class DBTransaction : IDisposable
    {
        private BWFramework.DAL.DBTransaction dbTransaction;
        /// <summary>
        /// 构造函数
        /// </summary>
        public DBTransaction()
        {
            this.dbTransaction = new BWFramework.DAL.DBTransaction();
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public DBTransaction(string connectionString)
        {
            this.dbTransaction = new BWFramework.DAL.DBTransaction(connectionString);
        }
        /// <summary>
        /// 提交事务
        /// </summary>
        public void Commit()
        {
            this.dbTransaction.Commit();
        }

        #region IDisposable 成员

        public void Dispose()
        {
            this.dbTransaction.Dispose();
        }

        #endregion
    }
}
