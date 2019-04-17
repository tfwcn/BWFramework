using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BWCore.BLL
{
    public class TUserInfo : BWCore.BLL.Base.BLLBase<BWCore.Model.TUserInfo>
    {
        private new BWCore.DAL.TUserInfo dalObject;
        public TUserInfo(string connectionString) : base(connectionString)
        {
            //base.bllTLog = new TLog();
            dalObject = base.dalObject as BWCore.DAL.TUserInfo;
        }
        /// <summary>
        /// 通过主表ID获取明细数据
        /// </summary>
        public Model.TUserInfo GetModelByCUserID(string CUserID)
        {
            return dalObject.GetModelByCUserID(CUserID);
        }
    }
}
