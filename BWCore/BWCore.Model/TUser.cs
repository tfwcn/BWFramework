﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BWCore.Common.AttributeEx;

namespace BWCore.Model
{
    [DBTable(Name = "t_user")]
    public class TUser:BWCore.Model.Base.ModelBase
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Summary(Name = "用户名", Description = "用户名")]
        [DBCol(Name ="f_name")]
        public string FName { get; set; }
        ///// <summary>
        ///// 帐号
        ///// </summary>
        //[Summary(Name = "帐号", Description = "登录帐号")]
        //[DBCol(CanRead = true, CanWrite = false)]
        //public string CLoginNo { get; set; }
        ///// <summary>
        ///// 密码
        ///// </summary>
        //[Summary(Name = "密码", Description = "登录密码")]
        //[DBCol(CanRead = true, CanWrite = false)]
        //public string CPassword { get; set; }
        //#region 关联字段
        ///// <summary>
        ///// (关联字段)性別類型
        ///// </summary>
        //[Summary(Name = "性別", Description = "性別")]
        //[DBCol(CanRead = true, CanWrite = false)]
        //public int CSex { get; set; }
        ///// <summary>
        ///// (关联字段)性別
        ///// </summary>
        //[Summary(Name = "性別", Description = "性別")]
        //[DBCol(CanRead = false, CanWrite = false)]
        //public string CSexName { get { return ((BWCore.Model.TUserInfo.UserSex)CSex).ToString(); } }
        ///// <summary>
        ///// (关联字段)月收入金额
        ///// </summary>
        //[Summary(Name = "月收入金额", Description = "月收入金额（包括奖金、分红等额外金额）")]
        //[DBCol(CanRead = true, CanWrite = false)]
        //public decimal CMoney { get; set; }
        //#endregion
    }
}
