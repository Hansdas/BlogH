using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading;

namespace CacheFactory.Provider
{
    public class MemoryCacheProvider:ICacheProvider
    {
        /// <summary>
        /// 单个写线程锁与多个读线程的锁
        /// </summary>
        private static ReaderWriterLockSlim readerWriterLock = new ReaderWriterLockSlim();
        private static MemoryCache memoryCache = new MemoryCache(new MemoryCacheOptions());
        #region 单键值

        /// <summary>
        /// 向缓存中添加一个对象。
        /// </summary>
        /// <param name="key">缓存的键值，该值通常是使用缓存机制的方法的名称。</param>
        /// <param name="value">需要缓存的对象。</param>
        public void Add(string key, object value)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("缓存key为空");
            try
            {
                readerWriterLock.EnterWriteLock();
                memoryCache.Set(key, value);
            }
            finally
            {
                readerWriterLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// 向缓存中添加一个对象。
        /// </summary>
        /// <param name="key">缓存的键值，该值通常是使用缓存机制的方法的名称。</param>
        /// <param name="value">需要缓存的对象。</param>
        public void Add<T>(string key, T value)
        {

        }

        /// <summary>
        /// 向缓存中添加一个对象。
        /// </summary>
        /// <param name="key">缓存的键值，该值通常是使用缓存机制的方法的名称。</param>
        /// <param name="value">需要缓存的对象。</param>
        /// <param name="slidingExpiration">活动过期时间。</param>
        public void Add(string key, object value, TimeSpan slidingExpiration)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("缓存key为空");
            try
            {
                readerWriterLock.EnterWriteLock();
                MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions();
                cacheEntryOptions.SlidingExpiration = slidingExpiration;
                memoryCache.Set(key, value,slidingExpiration);

            }
            finally
            {
                readerWriterLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// 向缓存中添加一个对象。
        /// </summary>
        /// <param name="key">缓存的键值，该值通常是使用缓存机制的方法的名称。</param>
        /// <param name="value">需要缓存的对象。</param>
        /// <param name="slidingExpiration">活动过期时间。</param>
        public void Add<T>(string key, T value, TimeSpan slidingExpiration)
        {

        }

        /// <summary>
        /// 向缓存中添加一个对象。
        /// </summary>
        /// <param name="key">缓存的键值，该值通常是使用缓存机制的方法的名称。</param>
        /// <param name="value">需要缓存的对象。</param>
        /// <param name="absoluteExpiration">绝对过期时间。</param>
        public void Add(string key, object value, DateTime absoluteExpiration)
        {

        }

        /// <summary>
        /// 向缓存中添加一个对象。
        /// </summary>
        /// <param name="key">缓存的键值，该值通常是使用缓存机制的方法的名称。</param>
        /// <param name="value">需要缓存的对象。</param>
        /// <param name="absoluteExpiration">绝对过期时间。</param>
        public void Add<T>(string key, T value, DateTime absoluteExpiration)
        {

        }

        /// <summary>
        /// 向缓存中更新一个对象。
        /// </summary>
        /// <param name="key">缓存的键值，该值通常是使用缓存机制的方法的名称。</param>
        /// <param name="value">需要缓存的对象。</param>
        public void Set(string key, object value)
        {

        }

        /// <summary>
        /// 向缓存中更新一个对象。
        /// </summary>
        /// <param name="key">缓存的键值，该值通常是使用缓存机制的方法的名称。</param>
        /// <param name="value">需要缓存的对象。</param>
        public void Set<T>(string key, T value)
        {

        }

        /// <summary>
        /// 向缓存中更新一个对象。
        /// </summary>
        /// <param name="key">缓存的键值，该值通常是使用缓存机制的方法的名称。</param>
        /// <param name="value">需要缓存的对象。</param>
        /// <param name="slidingExpiration">活动过期时间。</param>
        public void Set(string key, object value, TimeSpan slidingExpiration)
        {

        }

        /// <summary>
        /// 向缓存中更新一个对象。
        /// </summary>
        /// <param name="key">缓存的键值，该值通常是使用缓存机制的方法的名称。</param>
        /// <param name="value">需要缓存的对象。</param>
        /// <param name="slidingExpiration">活动过期时间。</param>
        public void Set<T>(string key, T value, TimeSpan slidingExpiration)
        {

        }

        /// <summary>
        /// 向缓存中更新一个对象。
        /// </summary>
        /// <param name="key">缓存的键值，该值通常是使用缓存机制的方法的名称。</param>
        /// <param name="value">需要缓存的对象。</param>
        /// <param name="absoluteExpiration">绝对过期时间。</param>
        public void Set(string key, object value, DateTime absoluteExpiration)
        {

        }

        /// <summary>
        /// 向缓存中更新一个对象。
        /// </summary>
        /// <param name="key">缓存的键值，该值通常是使用缓存机制的方法的名称。</param>
        /// <param name="value">需要缓存的对象。</param>
        /// <param name="absoluteExpiration">绝对过期时间。</param>
        public void Set<T>(string key, T value, DateTime absoluteExpiration)
        {

        }

        /// <summary>
        /// 从缓存中读取对象。
        /// </summary>
        /// <param name="key">缓存的键值，该值通常是使用缓存机制的方法的名称。</param>
        /// <returns>被缓存的对象。</returns>
        public T Get<T>(string key)
        {
            object obj = memoryCache.Get(key);
            return obj == null ? default(T) : (T)obj;
        }

        /// <summary>
        /// 从缓存中读取对象。
        /// </summary>
        /// <param name="key">缓存的键值，该值通常是使用缓存机制的方法的名称。</param>
        /// <returns>被缓存的对象。</returns>
        public object Get(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("缓存key为空");
            try
            {
                readerWriterLock.EnterReadLock();
                object obj=null;
                memoryCache.TryGetValue(key, out obj);
                if (obj == null)
                    return obj;
                else
                    return default(object);
            }
            finally
            {

            }
        }

        /// <summary>
        /// 从缓存中移除对象。
        /// </summary>
        /// <param name="key">缓存的键值，该值通常是使用缓存机制的方法的名称。</param>
        public void Remove(string key)
        {

        }

        /// <summary>
        /// 获取一个<see cref="Boolean"/>值，该值表示拥有指定键值的缓存是否存在。
        /// </summary>
        /// <param name="key">指定的键值。</param>
        /// <returns>如果缓存存在，则返回true，否则返回false。</returns>
        public bool Exists(string key)
        {
            object obj = memoryCache.Get(key);
            return obj == null ? false : true;
        }

        /// <summary>
        /// 清空所有缓存
        /// </summary>
        public void FlushAll()
        {

        }

        #endregion

    }
}
