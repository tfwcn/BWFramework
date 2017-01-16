using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BWFramework.BLL
{
    public class TUser : BWFramework.BLL.Base.BLLBase<BWFramework.Model.TUser>
    {
        private TUserInfo bllTUserInfo = new TUserInfo();
        private BWFramework.DAL.TUser dalObject;
        public TUser()
        {
            dalObject = base.dalObject as BWFramework.DAL.TUser;
        }
        /// <summary>
        /// 通過登錄帳號獲得數據
        /// </summary>
        public Model.TUser GetModelByLoginNo(string CLoginNo)
        {
            return dalObject.GetModelByLoginNo(CLoginNo);
        }
        /// <summary>
        /// 注册用户
        /// </summary>
        public void Register(Model.TUser tmpTUser, Model.TUserInfo tmpTUserInfo)
        {
            using (BWFramework.DBTransactionHelper.DBTransactionHelper tran = base.CreateDBTransactionHelper())
            {
                base.Add(tmpTUser);
                bllTUserInfo.Add(tmpTUserInfo);
                tran.Commit();
            }
        }
        public void Register(List<Model.TUser> tmpListTUser, List<Model.TUserInfo> tmpListTUserInfo)
        {
            using (BWFramework.DBTransactionHelper.DBTransactionHelper tran = base.CreateDBTransactionHelper())
            {
                base.Add(tmpListTUser);
                bllTUserInfo.Add(tmpListTUserInfo);
                tran.Commit();
            }
        }
    }
}
