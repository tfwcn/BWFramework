using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BWCore.Common.AttributeEx;

namespace BWCore.Model
{
    /// <summary>
    /// 日志类
    /// </summary>
    public class TLog : BWCore.Model.Base.ModelBase
    {
        /// <summary>
        /// 主表ID
        /// </summary>
        [Summary(Name = "主表ID", Description = "主表ID")]
        public string FLogID { get; set; }
        /// <summary>
        /// 操作描述
        /// </summary>
        [Summary(Name = "操作描述", Description = "操作描述")]
        public string FDescription { get; set; }
    }
}
