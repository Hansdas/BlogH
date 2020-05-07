using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Common.CacheFactory
{
   public interface ICacheClient
    {
        #region String操作
        void StringSet(string key, string value, TimeSpan expiry);
        void StringSet<T>(string key, T t, TimeSpan? expiry = null);
        T StringGet<T>(string key);
        string StringGet(string key);
        #endregion

        #region 集合（Set）操作
        /// <summary>
        /// 集合添加元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void AddSet(string key, string value);
        /// <summary>
        /// 获取set集合成员
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string[] GetMembers(string key);
        /// <summary>
        /// 删除集合中的指定元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool SetRemove(string key, string value);
        #endregion
        void Remove(string key);
        void BatchRemove(string[] keys);
        void BatchRemovePattern(string keyPattern);
        string[] GetKeys(string keyPattern);
    }
}
