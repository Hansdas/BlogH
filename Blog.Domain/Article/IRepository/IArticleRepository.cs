using System.Collections.Generic;

namespace Blog.Domain
{
    public interface IArticleRepository : IRepository<Article, int>
    {
        void Insert(Article article);
        int SelectCount(ArticleCondition condition = null);
        IEnumerable<Article> SelectByPage(int pageSize, int pageIndex, ArticleCondition condition=null);
        Article Select(ArticleCondition articleCondition = null);

        Article SelectById(int id);
        /// <summary>
        /// 查询上一篇和下一篇
        /// </summary>
        /// <param name="articleCondition"></param>
        /// <returns></returns>
        IEnumerable<dynamic> SelectNextUp(int id, ArticleCondition articleCondition = null);
        void Update(Article article);
        void Delete(int id);
        /// <summary>
        /// 评论
        /// </summary>
        void Review(IList<string> commentGuids, Comment comment, int id);
        /// <summary>
        /// 查询文章的评论guid集合
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IList<string> SelectCommentIds(int id);
        /// <summary>
        /// 查询作者
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string SelectAuthorById(int id);
    }
}
