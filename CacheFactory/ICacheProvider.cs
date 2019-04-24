using System;
using System.Collections.Generic;
using System.Text;

namespace CacheFactory
{
    /// <summary>
    /// 缓存接口
    /// </summary>
    public interface ICacheProvider
    {
        void Set(string key, object value);

        void Set<T>(string key, T value);
    }
}
