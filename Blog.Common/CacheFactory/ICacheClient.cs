using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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

        #region list操作
        /// <summary>
        /// 添加到list集合头部，返回集合长度
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        Task<long> AddListTop<T>(string key,T t);
        /// <summary>
        ///  添加到list集合头部，返回集合长度
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<long> AddListTop(string key, string value);
        /// <summary>
        /// 添加到list集合尾部，返回集合长度
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        Task<long> AddListTail<T>(string key, T t);
        /// <summary>
        ///  添加到list集合尾部，返回集合长度
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<long> AddListTail(string key, string value);
        /// <summary>
        /// 指定位置插入值
        /// </summary>
        Task ListInsert<T>(string key, int index, T t);
        /// <summary>
        /// 获取集合长度
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<long> ListLenght(string key);
        /// <summary>
        /// 从头部移走一个对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task listPop(string key);
        /// <summary>
        /// 默认从指定索引，返回指定数量结合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="startindex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        Task<List<T>> ListRange<T>(string key,int startindex, int endIndex, JsonSerializerSettings settings);
        #endregion
        void Remove(string key);
        void BatchRemove(string[] keys);
        void BatchRemovePattern(string keyPattern);
        string[] GetKeys(string keyPattern);
    }
}
