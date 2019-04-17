using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BWCore.Common.AttributeEx
{
    /// <summary>
    /// 数据库字段属性
    /// </summary>
    public class DBColAttribute : Attribute
    {
        /// <summary>
        /// 可从数据库读取
        /// </summary>
        public bool CanRead { get; set; } = true;
        /// <summary>
        /// 可写入数据库
        /// </summary>
        public bool CanWrite { get; set; } = true;
        /// <summary>
        /// 是否主键
        /// </summary>
        public bool PKey { get; set; }
        /// <summary>
        /// 字段名
        /// </summary>
        public string Name { get; set; }
    }
}
