﻿using Blog.AOP;
using Blog.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Data;
using Blog.Domain.Core;
using System.Linq;
using System.Dynamic;
using System.Collections;
using System.Threading.Tasks;

namespace Blog.Infrastruct
{
    public class ArticleRepository : Repository<Article, int>, IArticleRepository, IInterceptorHandler
    {
        private string Where(ArticleCondition condition, ref DynamicParameters dynamicParameters)
        {
            if (condition == null)
                return "";
            StringBuilder stringBuilder = new StringBuilder();
            IList<string> sqlList = new List<string>();
            if (!string.IsNullOrEmpty(condition.ArticleType))
            {
                dynamicParameters.Add("articleType", Convert.ToInt32(condition.ArticleType), DbType.Int32);
                sqlList.Add("article_articletype = @articleType");
            }
            if (condition.Id.HasValue)
            {
                dynamicParameters.Add("article_id", condition.Id.Value);
                sqlList.Add("article_id = @article_id");
            }
            if (!string.IsNullOrEmpty(condition.Account))
            {
                dynamicParameters.Add("article_author", condition.Id.Value);
                sqlList.Add("article_author = @article_author");
            }
            sqlList.Add(" 1=1 ");
            if (sqlList.Count == 0)
                return "";
            string sql = string.Join(" and ", sqlList);
            stringBuilder.Append(sql);
            return stringBuilder.ToString();
        }

        private Article Map(dynamic d)
        {
            return new Article(d.article_id,
                               d.article_author,
                               d.article_title,
                               d.article_textsection,
                               d.article_content,
                               (ArticleType)d.article_articletype,
                               d.article_isdraft,
                               d.article_relatedfiles,
                               d.article_praisecount,
                               d.article_browsercount,
                               d.article_createtime,
                               d.updatetime);
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
            string sql = "INSERT INTO Article(" +
                "article_author,article_title,article_textsection, article_content, article_articletype, article_praisecount, article_browsercount, article_isdraft, article_relatedfiles, article_createtime)" +
                   " VALUES (@Author,@Title,@TextSection, @Content, @ArticleType, @PraiseCount,@BrowserCount,@IsDraft,@RelatedFiles, NOW())";
            DbConnection.Execute(sql, article);
        }

        public int SelectCount(ArticleCondition condition = null)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            string where = Where(condition, ref dynamicParameters);
            string sql = "SELECT COUNT(*) FROM Article WHERE" + where;
            int count = DbConnection.ExecuteScalar<int>(sql, dynamicParameters);
            return count;
        }

        public async Task<int> SelectCountAsync(ArticleCondition condition = null)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            string where = Where(condition, ref dynamicParameters);
            string sql = "SELECT COUNT(*) FROM Article " + where;
            int count = await DbConnection.ExecuteScalarAsync<int>(sql, dynamicParameters);
            return count;
        }

        public IEnumerable<Article> SelectByPage(int pageSize, int pageIndex, ArticleCondition condition = null)
        {
            int pageId = pageSize * (pageIndex - 1);
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("pageId", pageId, DbType.Int32);
            dynamicParameters.Add("pageSize", pageSize, DbType.Int32);
            string where = Where(condition, ref dynamicParameters);
            //string sql = "SELECT * FROM s_log WHERE Id <=(SELECT Id FROM s_log   ORDER BY Id desc LIMIT 35000, 1) ORDER BY Id DESC LIMIT 10";

            string sql = "SELECT article_id,user_username,article_title,article_textsection,article_articletype,article_isdraft,article_createtime " +
                         "FROM Article INNER JOIN User ON user_account=article_author WHERE " + where +
                         " AND  article_id <=(" +
                         "SELECT article_id FROM Article WHERE "
                         + where +
                         "ORDER BY article_id DESC " +
                         "LIMIT @pageId, 1) " +
                         "ORDER BY article_id DESC " +
                         "LIMIT @pageSize";

            IEnumerable<dynamic> dynamics = Select(sql, dynamicParameters);
            IList<Article> articles = new List<Article>();
            foreach (var d in dynamics)
            {

                Article article = new Article(
                  d.article_id
                , d.article_title
                , d.user_username
                , d.article_textsection
                , (ArticleType)d.article_articletype
                , Convert.ToBoolean(d.article_isdraft)
                , (DateTime)d.article_createtime
                ); 
                articles.Add(article);
            }
            return articles;
        }

        public async Task<IEnumerable<Article>> SelectByPageAsync(int pageSize, int pageIndex, ArticleCondition condition = null)
        {
            int pageId = pageSize * (pageIndex - 1);
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("pageId", pageId, DbType.Int32);
            dynamicParameters.Add("pageSize", pageSize, DbType.Int32);
            string where = Where(condition, ref dynamicParameters);
            //string sql = "SELECT * FROM s_log WHERE Id <=(SELECT Id FROM s_log   ORDER BY Id desc LIMIT 35000, 1) ORDER BY Id DESC LIMIT 10";
            string sql = "SELECT article_id,article_title,article_textsection,article_articletype " +
                       "FROM Article WHERE " + where +
                       " AND  article_id <=(" +
                       "SELECT article_id FROM Article WHERE"
                       + where + " AND " +
                       "ORDER BY article_id DESC " +
                       "LIMIT @pageId, 1) " +
                       "ORDER BY article_id DESC " +
                       "LIMIT @pageSize";
            IEnumerable<dynamic> dynamics = await DbConnection.QueryAsync<dynamic>(sql, dynamicParameters);
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

        public Article Select(ArticleCondition articleCondition = null)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            string where = Where(articleCondition, ref dynamicParameters);
            string sql = "SELECT article_id,user_username,article_title,article_content,article_articletype,article_createtime " +
                         "FROM Article INNER JOIN User ON user_account=article_author WHERE 1=1 AND " + where;
            dynamic d = base.SelectSingle(sql, dynamicParameters);
            Article article = new Article(
                 d.article_id
                , d.user_username
                , d.article_title
                , d.article_content
                , (ArticleType)d.article_articletype
                , d.article_createtime
                );
            return article;
        }

        public IEnumerable<dynamic> SelectNextUp(int id, ArticleCondition articleCondition = null)
        {

            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("article_id", id, DbType.Int32);
            string where = Where(articleCondition, ref dynamicParameters);
            string sql = "SELECT article_id,article_title FROM Article "
                       + "WHERE article_id IN( "
                             + "SELECT MAX(article_id) "
                             + "FROM Article "
                             + "WHERE article_id <@article_id  AND " + where
                             + "UNION "
                             + "SELECT MIN(article_id) "
                             + "FROM Article "
                             + "WHERE article_id >@article_id  AND " + where
                             + ")";
            IEnumerable<dynamic> dynamics = DbConnection.Query(sql, dynamicParameters);
            return dynamics;
        }
    }
}
