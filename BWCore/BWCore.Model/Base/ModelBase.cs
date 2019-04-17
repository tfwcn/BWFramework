﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using BWCore.Common;
using BWCore.Common.AttributeEx;

namespace BWCore.Model.Base
{
    public abstract class ModelBase
    {
        /// <summary>
        /// ID
        /// </summary>
        [Summary(Name = "ID", Description = "主键")]
        [DBCol(Name = "f_id", PKey = true)]
        public string FId { get; set; }
        ///// <summary>
        ///// 数据版本号(自动)
        ///// </summary>
        //[Summary(Name = "数据版本号", Description = "操作数据时防止冲突")]
        //[DBCol(Name = "f_version", PKey = true)]
        //public int FVersion { get; set; }
        /// <summary>
        /// 创建时间(自动)
        /// </summary>
        [Summary(Name = "创建时间", Description = "创建时间")]
        [DBCol(Name = "f_create_time", PKey = true)]
        public DateTime? FCreateTime { get; set; }

        /// <summary>
        /// 属性旧值
        /// </summary>
        [DBCol(CanRead = false, CanWrite = false)]
        public Dictionary<string, object> OldValues { get; set; }
        /// <summary>
        /// 保存属性旧值（用于更新日志）
        /// </summary>
        public void SaveOldValues()
        {
            if (OldValues == null)
                OldValues = new Dictionary<string, object>();
            OldValues.Clear();
            foreach (var property in this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.SetProperty))
            {
                DBColAttribute attribute = Attribute.GetCustomAttribute(property, typeof(DBColAttribute)) as DBColAttribute;
                if (attribute == null || (attribute.CanRead == true && attribute.CanWrite == true))
                {
                    OldValues.Add(property.Name, property.GetValue(this));
                }
            }
        }
        ///// <summary>
        ///// 数据版本号加1
        ///// </summary>
        //public void AddCVersion()
        //{
        //    FVersion++;
        //}
    }
}
