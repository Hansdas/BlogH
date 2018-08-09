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
        #region 单键值

        /// <summary>
        /// 向缓存中添加一个对象。
        /// </summary>
        /// <param name="key">缓存的键值，该值通常是使用缓存机制的方法的名称。</param>
        /// <param name="value">需要缓存的对象。</param>
        void Add(string key, object value);

        /// <summary>
        /// 向缓存中添加一个对象。
        /// </summary>
        /// <param name="key">缓存的键值，该值通常是使用缓存机制的方法的名称。</param>
        /// <param name="value">需要缓存的对象。</param>
        void Add<T>(string key, T value);

        /// <summary>
        /// 向缓存中添加一个对象。
        /// </summary>
        /// <param name="key">缓存的键值，该值通常是使用缓存机制的方法的名称。</param>
        /// <param name="value">需要缓存的对象。</param>
        /// <param name="slidingExpiration">活动过期时间。</param>
        void Add(string key, object value, TimeSpan slidingExpiration);

        /// <summary>
        /// 向缓存中添加一个对象。
        /// </summary>
        /// <param name="key">缓存的键值，该值通常是使用缓存机制的方法的名称。</param>
        /// <param name="value">需要缓存的对象。</param>
        /// <param name="slidingExpiration">活动过期时间。</param>
        void Add<T>(string key, T value, TimeSpan slidingExpiration);

        /// <summary>
        /// 向缓存中添加一个对象。
        /// </summary>
        /// <param name="key">缓存的键值，该值通常是使用缓存机制的方法的名称。</param>
        /// <param name="value">需要缓存的对象。</param>
        /// <param name="absoluteExpiration">绝对过期时间。</param>
        void Add(string key, object value, DateTime absoluteExpiration);

        /// <summary>
        /// 向缓存中添加一个对象。
        /// </summary>
        /// <param name="key">缓存的键值，该值通常是使用缓存机制的方法的名称。</param>
        /// <param name="value">需要缓存的对象。</param>
        /// <param name="absoluteExpiration">绝对过期时间。</param>
        void Add<T>(string key, T value, DateTime absoluteExpiration);

        /// <summary>
        /// 向缓存中更新一个对象。
        /// </summary>
        /// <param name="key">缓存的键值，该值通常是使用缓存机制的方法的名称。</param>
        /// <param name="value">需要缓存的对象。</param>
        void Set(string key, object value);

        /// <summary>
        /// 向缓存中更新一个对象。
        /// </summary>
        /// <param name="key">缓存的键值，该值通常是使用缓存机制的方法的名称。</param>
        /// <param name="value">需要缓存的对象。</param>
        void Set<T>(string key, T value);

        /// <summary>
        /// 向缓存中更新一个对象。
        /// </summary>
        /// <param name="key">缓存的键值，该值通常是使用缓存机制的方法的名称。</param>
        /// <param name="value">需要缓存的对象。</param>
        /// <param name="slidingExpiration">活动过期时间。</param>
        void Set(string key, object value, TimeSpan slidingExpiration);

        /// <summary>
        /// 向缓存中更新一个对象。
        /// </summary>
        /// <param name="key">缓存的键值，该值通常是使用缓存机制的方法的名称。</param>
        /// <param name="value">需要缓存的对象。</param>
        /// <param name="slidingExpiration">活动过期时间。</param>
        void Set<T>(string key, T value, TimeSpan slidingExpiration);

        /// <summary>
        /// 向缓存中更新一个对象。
        /// </summary>
        /// <param name="key">缓存的键值，该值通常是使用缓存机制的方法的名称。</param>
        /// <param name="value">需要缓存的对象。</param>
        /// <param name="absoluteExpiration">绝对过期时间。</param>
        void Set(string key, object value, DateTime absoluteExpiration);

        /// <summary>
        /// 向缓存中更新一个对象。
        /// </summary>
        /// <param name="key">缓存的键值，该值通常是使用缓存机制的方法的名称。</param>
        /// <param name="value">需要缓存的对象。</param>
        /// <param name="absoluteExpiration">绝对过期时间。</param>
        void Set<T>(string key, T value, DateTime absoluteExpiration);

        /// <summary>
        /// 从缓存中读取对象。
        /// </summary>
        /// <param name="key">缓存的键值，该值通常是使用缓存机制的方法的名称。</param>
        /// <returns>被缓存的对象。</returns>
        T Get<T>(string key);

        /// <summary>
        /// 从缓存中读取对象。
        /// </summary>
        /// <param name="key">缓存的键值，该值通常是使用缓存机制的方法的名称。</param>
        /// <returns>被缓存的对象。</returns>
        object Get(string key);

        /// <summary>
        /// 从缓存中移除对象。
        /// </summary>
        /// <param name="key">缓存的键值，该值通常是使用缓存机制的方法的名称。</param>
        void Remove(string key);

        /// <summary>
        /// 获取一个<see cref="Boolean"/>值，该值表示拥有指定键值的缓存是否存在。
        /// </summary>
        /// <param name="key">指定的键值。</param>
        /// <returns>如果缓存存在，则返回true，否则返回false。</returns>
        bool Exists(string key);

        /// <summary>
        /// 清空所有缓存
        /// </summary>
        void FlushAll();

        #endregion

    }
}
