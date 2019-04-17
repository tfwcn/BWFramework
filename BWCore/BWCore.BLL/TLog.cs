using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BWCore.Common;

namespace BWCore.BLL
{
    public class TLog : BWCore.BLL.Base.BLLBase<BWCore.Model.TLog>
    {
        private new BWCore.DAL.TLog dalObject;
        public TLog(string connectionString) : base(connectionString)
        {
            dalObject = base.dalObject as BWCore.DAL.TLog;
        }
        /// <summary>
        /// 生成更新日志，属性没变化则返回null
        /// </summary>
        public Model.TLog GetUpdateLog(Model.Base.ModelBase model)
        {
            Model.TLog tmpTLog = new Model.TLog();
            tmpTLog.FId = Guid.NewGuid().ToString();
            tmpTLog.FLogID = model.FId;
            foreach (var v in model.OldValues)
            {
                object newValue = model.GetPropertyValue(v.Key);
                if (newValue.ToString() != v.Value.ToString())
                {
                    tmpTLog.FDescription += String.Format("{0}({1}):{2} > {3};", model.GetSummaryName(v.Key), v.Key, v.Value, newValue);
                }
            }
            if (tmpTLog.FDescription.IsNullOrEmpty())//属性没变化，不生成日志
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
