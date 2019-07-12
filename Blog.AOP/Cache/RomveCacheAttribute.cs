using Blog.Common.CacheFactory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blog.AOP.Cache
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RomveCacheAttribute : Attribute
    {
        private readonly ICacheClient _cacheClient;
        public RomveCacheAttribute(ICacheClient cacheClient)
        {
            _cacheClient = cacheClient;
        }
        public RomveCacheAttribute(string dbname, string className)
        {
            RemoveKeyAsync(dbname, className);
        }
        private void RemoveKeyAsync(string dbname, string className)
        {
            Task.Factory.StartNew(() =>
            {
                string[] keys = _cacheClient.GetKeys(className);
                foreach (var item in keys)
                {
                    if (item.Contains(dbname))
                        _cacheClient.Remove(item);
                }
            });
        }
    }

}
