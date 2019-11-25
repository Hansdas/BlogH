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
using Blog.AOP.Transaction;

namespace Blog.Infrastruct
{
    public class ArticleRepository : Repository<Article, int>, IArticleRepository, IInterceptorHandler
    {
        private ICommentRepository _commentRepository;
        public ArticleRepository(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }
        private string Where(ArticleCondition condition, ref DynamicParameters dynamicParameters)
        {
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
                dynamicParameters.Add("article_author", condition.Account);
                sqlList.Add("article_author = @article_author");
            }
            sqlList.Add(" 1=1 ");
            string sql = string.Join(" AND ", sqlList);
            return sql;
        }
        public Tuple<IList<string>, DynamicParameters> BuildWhere(ICondition condition)
        {
            WhisperCondiiton whisperCondiiton = condition as WhisperCondiiton;
            DynamicParameters parameters = new DynamicParameters();
            IList<string> sqlList = new List<string>();
            if (condition == null)
                return new Tuple<IList<string>, DynamicParameters>(sqlList, parameters);
            if (!string.IsNullOrEmpty(whisperCondiiton.Account))
            {
                parameters.Add("account", whisperCondiiton.Account);
                sqlList.Add("whisper_account = @account");
            }
            return new Tuple<IList<string>, DynamicParameters>(sqlList, parameters);
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
            string sql = "SELECT COUNT(*) FROM Article WHERE " + where;
            int count = DbConnection.ExecuteScalar<int>(sql, dynamicParameters);
            return count;
        }


        public IEnumerable<Article> SelectByPage(int pageSize, int pageIndex, ArticleCondition condition = null)
        {
            int pageId = pageSize * (pageIndex - 1);
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("pageId", pageId, DbType.Int32);
            dynamicParameters.Add("pageSize", pageSize, DbType.Int32);
            string where = Where(condition, ref dynamicParameters);
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


        public Article Select(ArticleCondition articleCondition = null)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            string where = Where(articleCondition, ref dynamicParameters);
            string sql = "SELECT article_id,user_username,article_title,article_content,article_articletype,article_isdraft,article_createtime " +
                         "FROM Article INNER JOIN User ON user_account=article_author WHERE 1=1 AND " + where;
            dynamic d = base.SelectSingle(sql, dynamicParameters);
            Article article = new Article(
                d.article_title
                , d.user_username
                , d.article_content
                , (ArticleType)d.article_articletype
                , Convert.ToBoolean(d.article_isdraft)
                , d.article_comments
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
        public void Update(Article article)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Ttile", article.Title);
            parameters.Add("TextSection", article.TextSection);
            parameters.Add("Content", article.Content);
            parameters.Add("ArticleType",article.ArticleType);
            parameters.Add("PraiseCount", article.PraiseCount);
            parameters.Add("BrowserCount", article.BrowserCount);
            parameters.Add("IsDraft", article.IsDraft);
            parameters.Add("Id", article.Id);
            string sql = "UPDATE Article " +
                "SET article_title = @Ttile" +
                ", article_textsection = @TextSection " +
                ", article_content =@Content " +
                ", article_articletype = @ArticleType" +
                ", article_isdraft = @IsDraft" +
                ", article_createtime = NOW() WHERE article_id =@Id";
            DbConnection.Execute(sql, parameters);
        }

        public void Delete(int id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Id", id);
            string sql = "DELETE FROM Article WHERE article_id=@Id";
            int i=  DbConnection.Execute(sql,parameters);
        }                                                  

        [Transaction(TransactionLevel.ReadCommitted, ScopeOption.Required)]
        public void Comment(IList<Comment> comments, int id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Comments", comments.Select(s=>s.Guid));
            parameters.Add("Id", id);
            string sql = "UPDATE Article " +
                "SET article_comments = @Comments" +
                " WHERE article_id =@Id";
            DbConnection.Execute(sql, parameters);
            string insert = "INSERT INTO Comment(comment_guid,comment_content,comment_account,comment_commentdate)" +
                " VALUES (@Guid,@CommentContent,@CommentAccount,@CommentDate)";
            DbConnection.Execute(insert, comments[comments.Count-1]);//最后一条数据为最新评论
        }

        public IList<string> SelectCommentIds(int id)
        {
            IList<string> commnetIdList = new List<string>();
            string select = "SELECT article_comments FROM Article WHERE article_id = @Id";
            string commentIds = SelectSingle(select, new { Id = id });
            if (!string.IsNullOrEmpty(commentIds))
                commnetIdList = commentIds.Split(',');
            return commnetIdList;
        }
    }
}
