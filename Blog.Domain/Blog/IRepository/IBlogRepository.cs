using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
   public interface IBlogRepository: IRepository<Blog, int>
    {
        /// <summary>
        /// 发表微语
        /// </summary>
        void InsertWhisper(Blog blogContent);
        /// <summary>
        /// 分页查询微语
        /// </summary>
        /// <returns></returns>
        IEnumerable<Domain.Blog> SelectByPage(int pageIndex, int pageSize, out int recordCount);
    }
}
