using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
   public interface INewsRepository: IRepository<News, int>
    {
        /// <summary>
        /// 不存在则插入，否则更新
        /// </summary>
        /// <param name="newsList"></param>
        void InsertOrUpdate(IList<News> newsList);
        /// <summary>
        /// 从配置表查询
        /// </summary>
        /// <returns></returns>
        IList<News> Select();
    }
}
