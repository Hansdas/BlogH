﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CommonHelper
{
   public static class ListExtensionMethod
    {
        /// <summary>
        /// 将集合转为字符串
        /// </summary>
        /// <param name="separator">连接符</param>
        /// <param name="isWarp">是否首尾两端添加分隔符</param>
        /// <returns></returns>
        public static string ConvertTostring(this IEnumerable source, char separator)
        {
            StringBuilder builder = new StringBuilder(); 
            foreach(var item in source)
            {
                builder.Append(item);
                builder.Append(separator);
            }
           return builder.ToString().Trim(separator);
        }
    }
}
