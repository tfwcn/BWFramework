using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BWFramework.Common.AttributeEx;

namespace BWFramework.Model
{
    public class TNo:BWFramework.Model.Base.ModelBase
    {
        /// <summary>
        /// 单号
        /// </summary>
        [Summary(Name = "单号", Description = "单号")]
        public string CNo { get; set; }
    }
}
