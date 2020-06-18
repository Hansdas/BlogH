using System.Collections.Generic;
using System.Collections.Specialized;

namespace Blog.Domain
{
    public interface IArticleRepository : IRepository
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
        IEnumerable<dynamic> SelectContext(int id, ArticleCondition articleCondition = null);
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
        /// <summary>
        /// 更新赞数量
        /// </summary>
        /// <param name="praiseCount"></param>
        /// <param name="id"></param>
        void Praise(int id,bool cancle);
        /// <summary>
        /// orderby查询
        /// </summary>
        /// <param name="num"></param>
        /// <param name="orderByCollection"></param>
        /// <returns></returns>
        IList<Article> SelectTop(int num, NameValueCollection orderByCollection);
        /// <summary>
        /// 查询每种类型时间最大的
        /// </summary>
        /// <param name="num"></param>
        /// <param name="orderByCollection"></param>
        /// <returns></returns>
        IList<Article> SelectByTypeMaxTime();
        /// <summary>
        /// 根据文章id查询该作者的所有文章
        /// </summary>
        /// <returns></returns>
        IList<Article> SelectAllByArticle(int articleId);
        /// <summary>
        /// 查询文章归档
        /// </summary>
        /// <param name="articleCondition"></param>
        /// <returns></returns>
        IEnumerable<dynamic> SelectArticleFile(ArticleCondition articleCondition);
    }
}
