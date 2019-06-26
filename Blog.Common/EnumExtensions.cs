using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Common
{
  public static  class EnumExtensions
    {
        /// <summary>
        /// 返回枚举所对应的值
        /// </summary>
        /// <param name="enum"></param>
        /// <returns></returns>
        public static int GetEnumValue(this Enum @enum)
        {
            return (int)@enum.GetType().GetField(@enum.ToString()).GetRawConstantValue();
        }
    }
}
