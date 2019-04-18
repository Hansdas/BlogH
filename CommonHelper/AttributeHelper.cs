using Domain.Attr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CommonHelper
{
   public static  class AttributeHelper
    {
        /// <summary>
        /// 返回表名
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static string GetTableName(this Type propertyInfo)
        {
            var Attributes = propertyInfo.GetCustomAttributes(typeof(TableAttribute), false);
            if (Attributes.Length > 0)
                return ((TableAttribute)Attributes[0]).TableName;
            else
                return "";
        }
    }
}
