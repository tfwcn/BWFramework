using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BWFramework.Common.AttributeEx
{
    /// <summary>
    /// 数据库字段属性
    /// </summary>
    public class DBColAttribute : Attribute
    {
        private bool canRead = true;
        private bool canWrite = true;
        /// <summary>
        /// 可从数据库读取
        /// </summary>
        public bool CanRead { get { return canRead; } set { canRead = value; } }
        /// <summary>
        /// 可写入数据库
        /// </summary>
        public bool CanWrite { get { return canWrite; } set { canWrite = value; } }
        public bool PKey { get; set; }
    }
}
