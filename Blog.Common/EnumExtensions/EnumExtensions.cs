using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Blog.Common.EnumExtensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// 返回枚举所对应的值
        /// </summary>
        /// <param name="enum"></param>
        /// <returns></returns>
        public static int GetEnumValue<T>(this T @enum)
        {
            return (int)@enum.GetType().GetField(@enum.ToString()).GetRawConstantValue();
        }
        /// <summary>
        /// 返回枚举名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetEnumText<T>(this T enumValue)
        {
            return Enum.GetName(typeof(T), enumValue);
        }
        /// <summary>
        /// 返回枚举所对应的附加数据
        /// </summary>
        /// <param name="enum"></param>
        /// <returns></returns>
        public static string GetEnumAditional<T>(this T @enum)
        {
            string value = @enum.ToString();
            FieldInfo fieldInfo= @enum.GetType().GetField(value);
            var obj= fieldInfo.GetCustomAttributes(typeof(EnumAdditionalAttribute), false).First();
            EnumAdditionalAttribute attribute = obj as EnumAdditionalAttribute;
            if (attribute == null)
                return "";
            return attribute.Additional;
        }
        /// <summary>
        /// 根据值返回枚举名字
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetEnumText<T>(this int enumValue)
        {
            return Enum.GetName(typeof(T), enumValue);
        }
        public static IEnumerable<T> AsEnumerable<T>() where T : Enum
        {
            EnumQuery<T> query = new EnumQuery<T>();
            return query;
        }
        private class EnumQuery<T> : IEnumerable<T>
        {
            public IEnumerator<T> GetEnumerator()
            {
                IList<T> list = new List<T>();
                var arrays = Enum.GetValues(typeof(T));
                foreach (var item in arrays)
                {
                    list.Add((T)item);
                }
                return list.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
    /// <summary>
    /// 枚举装换
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EnumConvert<T> where T : Enum
    {
        /// <summary>
        /// 讲=将Enum转为Ienumable集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> AsEnumerable()
        {
            EnumQuery<T> query = new EnumQuery<T>();
            return query;
        }
        private class EnumQuery<T> : IEnumerable<T>
        {
            public IEnumerator<T> GetEnumerator()
            {
                IList<T> list = new List<T>();
                Array array = Enum.GetValues(typeof(T));
                foreach (var item in array)
                    list.Add((T)item);
                return list.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

        }
        /// <summary>
        /// 将Enum转为字典结合
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> AsDictionary()
        {
            Type @enum = typeof(T);
            Array array = Enum.GetValues(@enum);
            Dictionary<int, string> enumDic = new Dictionary<int, string>();
            foreach (var item in array)
            {
                int key = item.GetEnumValue(); ;
                enumDic.Add(key, item.ToString());
            }
            return enumDic;
        }
        /// <summary>
        /// 将Enum转为键值对的list
        /// </summary>
        /// <returns></returns>
        public static IList<KeyValueItem> AsKeyValueItem()
        {
            Type @enum = typeof(T);
            Array array = Enum.GetValues(@enum);
            IList<KeyValueItem> list = new List<KeyValueItem>();
            foreach (var item in array)
            {
                int key = item.GetEnumValue(); ;
                KeyValueItem keyValueItem = new KeyValueItem(key.ToString(),item.ToString());
                list.Add(keyValueItem);
            }
            return list;
        }
    }
    /// <summary>
    /// 键值对的item
    /// </summary>
    public class KeyValueItem
    {
        public KeyValueItem(string key,string value)
        {
            Key = key;
            Value = value;
        }
        public string Key { get; private set; }

        public string Value { get; private set; }
    }
}
