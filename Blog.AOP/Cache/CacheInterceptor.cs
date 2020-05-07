﻿using Blog.Common;
using Blog.Common.CacheFactory;
using Castle.DynamicProxy;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Blog.AOP.Cache
{
    public class CacheInterceptor: ICacheInterceptor
    {
        private readonly ICacheClient _cacheClient;
        /// <summary>
        /// 缓存key前缀
        /// </summary>
        private string keyPrefix = "MapCache";
        public CacheInterceptor(ICacheClient cacheClient)
        {
            _cacheClient = cacheClient;
        }
        /// <summary>
        /// 拦截被特性标记的方法
        /// </summary>
        /// <param name="invocation"></param>
        public  void Intercept(CacheAttribute attribute, IInvocation invocation)
        {
            if (attribute == null)
                invocation.Proceed();
            else
                ProgressCaching(invocation, attribute);
        }
        /// <summary>
        /// 操作缓存
        /// </summary>
        /// <param name="invocation"></param>
        private void ProgressCaching(IInvocation invocation, CacheAttribute mapCacheAttribute)
        {
            var typeName = invocation.TargetType.Name;
            var methodName = invocation.Method.Name;
            Type methodReturnType=invocation.Method.ReturnType;
            var methodArguments = ParseArgumentsToPartOfCacheKey(invocation.Arguments);
            var cacheKey = BuildCacheKey(typeName, methodName, methodArguments, mapCacheAttribute.prefixs);

            var cacheValue = _cacheClient.StringGet(cacheKey);
            if (cacheValue != null)
            {
                //基元类型和string类型不需要序列化
                if (methodReturnType.IsPrimitive || methodReturnType.Name == "String") 
                    invocation.ReturnValue = cacheKey;
                else
                {
                    var contractResolver = new JsonContractResolver();
                    JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings()
                    {
                        ContractResolver = contractResolver
                    };
                    invocation.ReturnValue = JsonConvert.DeserializeObject(cacheValue, methodReturnType, jsonSerializerSettings);
                }
                return;
            }

            invocation.Proceed();

            if (!string.IsNullOrWhiteSpace(cacheKey))
            {
                _cacheClient.StringSet(cacheKey, invocation.ReturnValue);
            }
        }
        /// <summary>
        /// 拼接缓存key
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="methodName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private string BuildCacheKey(string typeName, string methodName, IList<string> parameters,IList<string> dbNames)
        {
            StringBuilder cacheKey = new StringBuilder();
            cacheKey.Append(keyPrefix);
            cacheKey.Append(".");
            cacheKey.Append(typeName);
            cacheKey.Append(".");

            cacheKey.Append(methodName);
            cacheKey.Append(".");

            foreach (var param in parameters)
            {
                cacheKey.Append(param);
                cacheKey.Append(".");
            }
            cacheKey.Append("=>");
            foreach (var name in dbNames)
            {
                cacheKey.Append(name);
                cacheKey.Append(".");
            }
            return cacheKey.ToString().TrimEnd('.');
        }
        /// <summary>
        /// 解析方法参数值来组缓存key
        /// </summary>
        /// <param name="methodArguments"></param>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        private IList<string> ParseArgumentsToPartOfCacheKey(IList<object> methodArguments, int maxCount = 5)
        {
            return methodArguments.Select(GetArgumentValue).Take(maxCount).ToList();
        }

        /// <summary>
        /// 解析参数值
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private string GetArgumentValue(object arg)
        {
            if (arg is int || arg is long || arg is string)
                return arg.ToString();

            if (arg is DateTime)
                return ((DateTime)arg).ToString("yyyyMMddHHmmss");

            return JsonHelper.Serialize(arg);
        }
    }
}
