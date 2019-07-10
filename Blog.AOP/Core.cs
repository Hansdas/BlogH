using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Blog.AOP
{
  public  class Core
    {
        public static T GetAttribute<T>(MethodInfo method, Type attributeType) where T:Attribute
        {
            object[] attributes = method.GetCustomAttributes(true);
            return  attributes.FirstOrDefault(s => s.GetType() == attributeType) as T;
        }
    }
}
