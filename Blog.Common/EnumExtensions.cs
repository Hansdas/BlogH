using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Blog.Common
{
  public static  class EnumExtensions
    {
        /// <summary>
        /// 返回枚举所对应的值
        /// </summary>
        /// <param name="enum"></param>
        /// <returns></returns>
        public static int GetEnumValue(this Enum @enum)
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
        public static IEnumerable<T> AsEnumerable<T>() where T:Enum
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
}
