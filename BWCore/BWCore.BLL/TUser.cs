using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BWCore.BLL
{
    public class TUser : BWCore.BLL.Base.BLLBase<BWCore.Model.TUser>
    {
        private new BWCore.DAL.TUser dalObject;
        private TUserInfo bllTUserInfo;
        public TUser(string connectionString) : base(connectionString)
        {
            //base.bllTLog = new TLog();
            dalObject = base.dalObject as BWCore.DAL.TUser;
            bllTUserInfo = new TUserInfo(this.dalObject.ConnectionString);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        public List<Model.TUser> GetModels()
        {
            return dalObject.GetModels();
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
            using (BWCore.DBTransactionHelper.DBTransactionHelper tran = base.CreateDBTransactionHelper())
            {
                base.Add(tmpTUser);
                bllTUserInfo.Add(tmpTUserInfo);
                tran.Commit();
            }
        }
        public void Register(List<Model.TUser> tmpListTUser, List<Model.TUserInfo> tmpListTUserInfo)
        {
            using (BWCore.DBTransactionHelper.DBTransactionHelper tran = base.CreateDBTransactionHelper())
            {
                base.Add(tmpListTUser);
                bllTUserInfo.Add(tmpListTUserInfo);
                tran.Commit();
            }
        }
    }
}
