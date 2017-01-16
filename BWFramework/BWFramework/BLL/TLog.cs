using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BWFramework.Common;

namespace BWFramework.BLL
{
    public class TLog : BWFramework.BLL.Base.BLLBase<BWFramework.Model.TLog>
    {
        private BWFramework.DAL.TLog dalObject;
        public TLog()
        {
            dalObject = base.dalObject as BWFramework.DAL.TLog;
        }
        /// <summary>
        /// 生成更新日志，属性没变化则返回null
        /// </summary>
        public Model.TLog GetUpdateLog(Model.Base.ModelBase model)
        {
            Model.TLog tmpTLog = new Model.TLog();
            tmpTLog.CID = Guid.NewGuid().ToString();
            tmpTLog.CLogID = model.CID;
            foreach (var v in model.OldValues)
            {
                object newValue = model.GetPropertyValue(v.Key);
                if (newValue.ToString() != v.Value.ToString())
                {
                    tmpTLog.CDescription += String.Format("{0}({1}):{2} > {3};", model.GetSummaryName(v.Key), v.Key, v.Value, newValue);
                }
            }
            if (tmpTLog.CDescription.IsVoid())//属性没变化，不生成日志
                return null;
            else
                return tmpTLog;
        }
        /// <summary>
        /// 通过主表ID获取明细数据
        /// </summary>
        public List<Model.TLog> GetModelsByCLogID(string CLogID)
        {
            return dalObject.GetModelsByCLogID(CLogID);
        }
    }
}
