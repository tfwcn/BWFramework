using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BWCore.Common.AttributeEx
{
    /// <summary>
    /// 数据库表属性
    /// </summary>
    public class DBTableAttribute : Attribute
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string Name { get; set; }
    }
}
