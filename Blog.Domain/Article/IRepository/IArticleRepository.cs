using Blog.Domain.Core.Enum;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain
{
    public interface IArticleRepository : IRepository<Article, int>
    {
        void Insert(Article article);
        int SelectCount(ArticleCondition condition = null);
        Task<int> SelectCountAsync(ArticleCondition condition = null);
        IEnumerable<Article> SelectByPage(int pageSize, int pageIndex, ArticleCondition condition=null);
        Task<IEnumerable<Article>> SelectByPageAsync(int pageSize, int pageIndex, ArticleCondition condition = null);
        Article Select(ArticleCondition articleCondition = null);
        /// <summary>
        /// 查询上一篇和下一篇
        /// </summary>
        /// <param name="articleCondition"></param>
        /// <returns></returns>

        IEnumerable<dynamic> SelectNextUp(int id, ArticleCondition articleCondition = null);
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="article"></param>
        void Update(Article article);
        /// <summary>
        /// 根据id删除
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);
    }
}
