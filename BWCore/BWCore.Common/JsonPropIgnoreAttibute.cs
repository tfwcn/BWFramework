using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BWCore.Common
{
    /// <summary>
    /// 特性：Json序列化时忽略某个属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface | AttributeTargets.Enum | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class JsonPropIgnoreAttibute : Attribute
    {
    }
}
