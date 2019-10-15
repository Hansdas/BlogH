using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.AOP.Cache
{
    /// <summary>
    /// 标记一个方法使用缓存缓
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
   public  class CacheAttribute:Attribute
    {
        /// <summary>
        /// 构找函数
        /// </summary>
        /// <param name="target">执行缓存的操作类型</param>
        /// <param name="prefix">缓存key前缀</param>
        public CacheAttribute(CacheTarget target,params string[] prefix)
        {
            prefixs = prefix;
        }
        public int prefix { get; set; }
        public IList<string> prefixs { get; set; }
    }
    /// <summary>
    /// 执行缓存的操作类型
    /// </summary>
    public enum CacheTarget
    {
        GetOrSet,Update
    }
}
