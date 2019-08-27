using Blog.AOP;
using Blog.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Data;
using Blog.Domain.Core;
using System.Dynamic;

namespace Blog.Infrastruct
{
    public class ArticleRepository : Repository<Article, int>, IArticleRepository, IInterceptorHandler
    {
        private Article Map(dynamic d)
        {
            return new Article(
                d.article_id
                , d.article_author
                , d.article_title
                , d.article_textsection
                , d.article_content
                , (ArticleType)d.article_articletype
                , d.article_isdraft
                , d.article_relatedfiles
                , d.article_praisecount
                , d.article_browsercount
                , d.article_createtime
                , d.updatetime);
        }
        private IList<Article> Map(IEnumerable<dynamic> dynamics)
        {
            IList<Article> articles = new List<Article>();
            foreach (var item in dynamics)
            {
                Article article = Map(item);
                articles.Add(item);
            }
            return articles;
        }
        public void Insert(Article article)
        {
            string sql = "INSERT INTO Article(article_author,article_title,article_textsection, article_content, article_articletype, article_praisecount, article_browsercount, article_isdraft, article_relatedfiles, article_createtime)" +
                   " VALUES (@Author,@Title,@TextSection, @Content, @ArticleType, @PraiseCount,@BrowserCount,@IsDraft,@RelatedFiles, NOW())";
            DbConnection.Execute(sql, article);
        }

        public int SelectCount()
        {
            string sql = "SELECT COUNT(*) FROM Article";
            int count = DbConnection.ExecuteScalar<int>(sql);
            return count;
        }

        public IEnumerable<Article> SelectByPage(int pageSize, int pageIndex)
        {
            int pageId = pageSize * (pageIndex - 1);
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("pageId", pageId, DbType.Int32);
            dynamicParameters.Add("pageSize", pageSize, DbType.Int32);
            //string sql = "SELECT * FROM s_log WHERE Id <=(SELECT Id FROM s_log   ORDER BY Id desc LIMIT 35000, 1) ORDER BY Id DESC LIMIT 10";
            string sql = "SELECT article_id,article_title,article_textsection,article_articletype FROM Article WHERE article_id <=" +
                "(SELECT article_id FROM Article   ORDER BY article_id desc LIMIT " + pageId + ", 1) ORDER BY article_id DESC LIMIT " + pageSize;
            IEnumerable<dynamic> dynamics = DbConnection.Query(sql);
            IList<Article> articles = new List<Article>();
            foreach (var d in dynamics)
            {

                Article article = new Article(
                  d.article_id
                , d.article_title
                , d.article_textsection
                , (ArticleType)d.article_articletype
                );
                articles.Add(article);
            }
            return articles;
        }
    }
}
