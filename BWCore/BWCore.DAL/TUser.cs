using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BWCore.DAL
{
    public class TUser : BWCore.DAL.Base.DALBase<Model.TUser>
    {
        public override Model.TUser GetModel(string CID)
        {
            string selectStr = "select top 1 a.*,b.CSex,b.CMoney from TUser as a left join TUserInfo as b on a.CID=b.CUserID";
            string whereStr = "where a.CID=@CID";
            List<DbParameter> paramenters = new List<DbParameter>();
            paramenters.Add(dbHelper.NewDbParameter("@CID", DbType.String, CID, 36));
            return base.GetModel(selectStr, whereStr, paramenters);
        }

        public List<Model.TUser> GetModels()
        {
            string whereStr = "";
            List<DbParameter> paramenters = new List<DbParameter>();
            //paramenters.Add(dbHelper.NewDbParameter("@CID", DbType.String, CID, 36));
            return base.GetModels(whereStr, paramenters);
        }
        /// <summary>
        /// 通過登錄帳號獲得數據
        /// </summary>
        public Model.TUser GetModelByLoginNo(string CLoginNo)
        {
            string selectStr = "select top 1 a.*,b.CSex,b.CMoney from TUser as a left join TUserInfo as b on a.CID=b.CUserID";
            string whereStr = "where a.CLoginNo=@CLoginNo";
            List<DbParameter> paramenters = new List<DbParameter>();
            paramenters.Add(dbHelper.NewDbParameter("@CLoginNo", DbType.String, CLoginNo, 50));
            return base.GetModel(selectStr, whereStr, paramenters);
        }
    }
}
