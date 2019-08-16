using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Blog.AOP
{
  public  class Core
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method">被特性拦截的方法</param>
        /// <param name="attributeType">目标特性</param>
        /// <returns></returns>
        public static T GetAttribute<T>(MethodInfo method, Type attributeType) where T:Attribute
        {
            object[] attributes = method.GetCustomAttributes(true);
            return  attributes.FirstOrDefault(s => s.GetType() == attributeType) as T;
        }
    }
}
