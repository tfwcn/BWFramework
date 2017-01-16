using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BWFramework.BLL
{
    public class TUserInfo : BWFramework.BLL.Base.BLLBase<BWFramework.Model.TUserInfo>
    {
        private BWFramework.DAL.TUserInfo dalObject;
        public TUserInfo()
        {
            //base.bllTLog = new TLog();
            dalObject = base.dalObject as BWFramework.DAL.TUserInfo;
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
