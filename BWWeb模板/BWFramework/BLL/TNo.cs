using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BWFramework.BLL
{
    public class TNo : BWFramework.BLL.Base.BLLBase<BWFramework.Model.TNo>
    {
        private BWFramework.DAL.TNo dalObject;
        public TNo()
        {
            dalObject = base.dalObject as BWFramework.DAL.TNo;
        }
        /// <summary>
        /// 通过主表ID获取明细数据
        /// </summary>
        public Model.TNo GetModelByCNo(string CUserID)
        {
            return dalObject.GetModelByCNo(CUserID);
        }
    }
}
