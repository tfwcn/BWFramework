using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BWCore.Common.AttributeEx;

namespace BWCore.Model
{
    public class TUserInfo : BWCore.Model.Base.ModelBase
    {
        public enum UserSex
        {
            保密 = 0,
            男 = 1,
            女 = 2
        }
        /// <summary>
        /// 主表ID
        /// </summary>
        [Summary(Name = "主表ID", Description = "主表ID")]
        public string CUserID { get; set; }
        /// <summary>
        /// 性別類型
        /// </summary>
        [Summary(Name = "性別", Description = "性別")]
        public int CSex { get; set; }
        /// <summary>
        /// 性別
        /// </summary>
        [Summary(Name = "性別", Description = "性別")]
        [DBCol(CanRead = false, CanWrite = false)]
        public string CSexName { get { return ((UserSex)CSex).ToString(); } }
        /// <summary>
        /// 月收入金额
        /// </summary>
        [Summary(Name = "月收入金额", Description = "月收入金额（包括奖金、分红等额外金额）")]
        public decimal CMoney { get; set; }
    }
}
