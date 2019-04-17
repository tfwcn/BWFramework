using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BWCore.DAL
{
    public class TUserInfo : BWCore.DAL.Base.DALBase<Model.TUserInfo>
    {
        /// <summary>
        /// 通过主表ID获取明细数据
        /// </summary>
        public Model.TUserInfo GetModelByCUserID(string CUserID)
        {
            string whereStr = "where CUserID=@CUserID";
            List<DbParameter> paramenters = new List<DbParameter>();
            paramenters.Add(dbHelper.NewDbParameter("@CUserID", DbType.String, CUserID, 36));
            return base.GetModel(whereStr, paramenters);
        }
    }
}
