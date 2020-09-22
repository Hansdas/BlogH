using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Common.EnumExtensions
{
    /// <summary>
    /// 枚举附加属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumAdditionalAttribute : Attribute
    {
        public EnumAdditionalAttribute(string additional)
        {
            Additional = additional;
        }
        public string Additional { get; set; }
    }
}
