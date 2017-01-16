using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BWFramework.DBTransactionHelper
{
    public abstract class DBTransactionBase
    {
        public DbTransaction dbTransaction;
        public abstract DbConnection GetConnection();
    }
}
