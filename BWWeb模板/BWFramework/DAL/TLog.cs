using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BWFramework.DAL
{
    public class TLog : BWFramework.DAL.Base.DALBase<Model.TLog>
    {
        /// <summary>
        /// 通过主表ID获取明细数据
        /// </summary>
        public List<Model.TLog> GetModelsByCLogID(string CLogID)
        {
            string whereStr = "where CLogID=@CLogID order by CCreateTime";
            List<DbParameter> paramenters = new List<DbParameter>();
            paramenters.Add(dbHelper.NewDbParameter("@CLogID", DbType.String, CLogID, 36));
            return base.GetModels(whereStr, paramenters);
        }
        /// <summary>
        /// 通过主表ID获取明细数据(分页)
        /// </summary>
        public List<Model.TLog> GetModelsPageByCLogID(int pageNum, int pageSize, string CLogID)
        {
            string whereStr = "where CLogID=@CLogID";
            string orderByStr = "order by CCreateTime";
            List<DbParameter> paramenters = new List<DbParameter>();
            paramenters.Add(dbHelper.NewDbParameter("@CLogID", DbType.String, CLogID, 36));
            return base.GetModelsPage(pageNum, pageSize, whereStr, orderByStr, paramenters);
        }
    }
}
