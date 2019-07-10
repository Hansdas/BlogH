using Blog.Common.CacheFactory;
using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blog.AOP.Cache
{
    public class CacheInterceptor : IInterceptor
    {
        private readonly ICacheClient _cacheClient;
        public CacheInterceptor(ICacheClient cacheClient)
        {
            _cacheClient = cacheClient;
        }
        public void Intercept(IInvocation invocation)
        {
            MapCacheAttribute cacheAttribute = Core.GetAttribute<MapCacheAttribute>
                (invocation.MethodInvocationTarget ?? invocation.Method,typeof(MapCacheAttribute));
            if (cacheAttribute == null)
                invocation.Proceed();
            else
                ProgressCaching(invocation);
        }
        private void ProgressCaching(IInvocation invocation)
        {
            var typeName = invocation.TargetType.Name;
            var methodName = invocation.Method.Name;
            var methodArguments = this.FormatArgumentsToPartOfCacheKey(invocation.Arguments);

            var cacheKey = BuildCacheKey(typeName,methodName,methodArguments);

            var cacheValue = _cacheClient.Get(cacheKey);
            if (cacheValue != null)
            {
                invocation.ReturnValue = cacheValue;
                return;
            }

            invocation.Proceed();

            if (!string.IsNullOrWhiteSpace(cacheKey))
            {
                _cacheClient.Set(cacheKey, invocation.ReturnValue);
            }
        }
        private string BuildCacheKey(string typeName, string methodName, IList<string> parameters)
        {
            StringBuilder cacheKey = new StringBuilder();
            cacheKey.Append(typeName);
            cacheKey.Append(":");

            cacheKey.Append(methodName);
            cacheKey.Append(":");

            foreach (var param in parameters)
            {
                cacheKey.Append(param);
                cacheKey.Append(":");
            }

            return cacheKey.ToString().TrimEnd(':');
        }
        private IList<string> FormatArgumentsToPartOfCacheKey(IList<object> methodArguments, int maxCount = 5)
        {
            return methodArguments.Select(this.GetArgumentValue).Take(maxCount).ToList();
        }

        private string GetArgumentValue(object arg)
        {
            if (arg is int || arg is long || arg is string)
                return arg.ToString();

            if (arg is DateTime)
                return ((DateTime)arg).ToString("yyyyMMddHHmmss");

            if (arg is ICache)
                return ((ICache)arg).CacheKey;

            return null;
        }
    }
   public  interface ICache
    {
        string CacheKey { get; }
    }
}
