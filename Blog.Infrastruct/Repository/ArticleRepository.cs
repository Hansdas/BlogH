using Blog.AOP;
using Blog.Domain;
using System;
using System.Collections.Generic;
using Dapper;
using System.Data;
using Blog.Domain.Core;
using System.Linq;
using Blog.AOP.Transaction;
using System.Collections.Specialized;
using System.Text;
using Blog.Common;

namespace Blog.Infrastruct
{
    public class ArticleRepository : Repository, IArticleRepository, IInterceptorHandler
    {
        private ICommentRepository _commentRepository;
        public ArticleRepository(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }
        /// <summary>
        /// 拼接查询条件
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dynamicParameters"></param>
        /// <returns></returns>
        private string Where(ArticleCondition condition, ref DynamicParameters dynamicParameters)
        {
            IList<string> sqlList = new List<string>();
            if (condition.ArticleType.HasValue&&condition.ArticleType!=0)
            {
                dynamicParameters.Add("articleType", condition.ArticleType.Value);
                sqlList.Add("article_articletype = @articleType");
            }
            if (condition.Id.HasValue)
            {
                dynamicParameters.Add("id", condition.Id.Value);
                sqlList.Add("article_id = @id");
            }
            if (!string.IsNullOrEmpty(condition.Account))
            {
                dynamicParameters.Add("author", condition.Account);
                sqlList.Add("article_author = @author");
            }
            if(!string.IsNullOrEmpty(condition.TitleContain))
            {
                dynamicParameters.Add("titleContain", condition.TitleContain);
                sqlList.Add("article_title like CONCAT('%',@titleContain,'%')");
            }
            if(condition.IsDraft.HasValue)
            {
                dynamicParameters.Add("isDraft", condition.IsDraft.Value,DbType.Boolean);
                sqlList.Add("article_isdraft = @isDraft");
            }
            if (!string.IsNullOrEmpty(condition.FullText))
            {
                dynamicParameters.Add("fullText", condition.FullText,DbType.String);
                sqlList.Add("MATCH(article_title,article_content) AGAINST(@fullText IN BOOLEAN MODE)");
            }
            sqlList.Add(" 1=1 ");
            string sql = string.Join(" AND ", sqlList);
            return sql;
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
            string sql = "INSERT INTO T_Article(" +
                "article_author,article_title,article_textsection, article_content, article_articletype, article_praisecount, article_browsercount, article_isdraft,article_sendemail, article_relatedfiles, article_createtime)" +
                   " VALUES (@Author,@Title,@TextSection, @Content, @ArticleType, @PraiseCount,@BrowserCount,@IsDraft,@IsSendEmail,@RelatedFiles, NOW())";
            DbConnection.Execute(sql, article);
        }

        public int SelectCount(ArticleCondition condition = null)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            string where = Where(condition, ref dynamicParameters);
            string sql = "SELECT COUNT(*) FROM T_Article WHERE " + where;
            int count = base.SelectCount(sql, dynamicParameters);
            return count;
        }


        public IEnumerable<Article> SelectByPage(int pageSize, int pageIndex, ArticleCondition condition = null)
        {
            int pageId = pageSize * (pageIndex - 1);
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("pageId", pageId, DbType.Int32);
            dynamicParameters.Add("pageSize", pageSize, DbType.Int32);
            string where = Where(condition, ref dynamicParameters);
            string sql = "SELECT article_id,article_author,article_title,article_textsection,article_articletype,article_isdraft,article_praisecount,article_browsercount,article_comments,article_createtime " +
                  "FROM T_Article  WHERE " + where +
                  " ORDER BY article_createtime DESC LIMIT @pageId,@pageSize";

            IEnumerable<dynamic> dynamics = Select(sql, dynamicParameters);
            IList<Article> articles = new List<Article>();
            foreach (var d in dynamics)
            {

                Article article = new Article(
                  d.article_id
                , d.article_title
                , d.article_author
                , d.article_textsection
                , (ArticleType)d.article_articletype
                , Convert.ToBoolean(d.article_isdraft)
                ,d.article_praisecount
                ,d.article_browsercount
                ,d.article_comments
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
            string sql = "SELECT article_id,article_author,article_title,article_content,article_articletype,article_comments,article_isdraft,article_createtime " +
                         "FROM T_Article  WHERE 1=1 AND " + where;
            dynamic d = base.SelectSingle(sql, dynamicParameters);
            IList<Comment> comments = new List<Comment>();
            if (!string.IsNullOrEmpty(d.article_comments))
            {
                string article_comments = d.article_comments;
                comments = _commentRepository.SelectByIds(article_comments.Split(','));
            }
            Article article = new Article(
                d.article_id
                ,d.article_title
                , d.article_author
                , d.article_content
                , (ArticleType)d.article_articletype
                , Convert.ToBoolean(d.article_isdraft)
                , comments
                , d.article_createtime
                );
            return article;
        }

        public IEnumerable<dynamic> SelectContext(int id, ArticleCondition articleCondition = null)
        {

            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("article_id", id, DbType.Int32);
            string where = Where(articleCondition, ref dynamicParameters);
            string sql = "SELECT article_id,article_title FROM T_Article "
                       + "WHERE article_id IN( "
                             + "SELECT MAX(article_id) "
                             + "FROM T_Article "
                             + "WHERE article_id <@article_id  AND " + where
                             + "UNION "
                             + "SELECT MIN(article_id) "
                             + "FROM T_Article "
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
            parameters.Add("IsDraft", article.IsDraft);
            parameters.Add("IsSendEmail", article.IsSendEmail);
            parameters.Add("Id", article.Id);
            string sql = "UPDATE T_Article " +
                "SET article_title = @Ttile" +
                ", article_textsection = @TextSection " +
                ", article_content =@Content " +
                ", article_articletype = @ArticleType" +
                ", article_isdraft = @IsDraft" +
                ", article_sendemail=@IsSendEmail" +
                ", article_createtime = NOW() WHERE article_id =@Id";
            DbConnection.Execute(sql, parameters);
        }

        public void Delete(int id)
        {
            string sql = "DELETE FROM T_Article WHERE article_id=@Id";
            DbConnection.Execute(sql,new { Id=id});
        }

        [Transaction(TransactionLevel.ReadCommitted)]
        public void Review(IList<string> commentGuids,Comment comment, int id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Comments", string.Join(',',commentGuids));
            parameters.Add("Id", id);
            string sql = "UPDATE T_Article " +
                "SET article_comments = @Comments" +
                " WHERE article_id =@Id";
            DbConnection.Execute(sql, parameters);
            _commentRepository.Insert(comment);
        }

        public IList<string> SelectCommentIds(int id)
        {
            IList<string> commnetIdList = new List<string>();
            string select = "SELECT article_comments FROM T_Article WHERE article_id = @Id";
            string commentIds = SelectSingle(select, new { Id = id }).article_comments;
            if (!string.IsNullOrEmpty(commentIds))
                commnetIdList = commentIds.Split(',').ToList();
            return commnetIdList;
        }
        public string SelectAuthorById(int id)
        {
            string sql = "SELECT article_author FROM T_Article WHERE article_Id=@Id";
            string postReviceUser = SelectSingle(sql, new { Id = id }).article_author;
            return postReviceUser;
        }

        public Article SelectById(int id)
        {
            string sql = "SELECT article_id,article_author,article_title,article_content,article_articletype,article_comments,article_isdraft,article_createtime " +
                         "FROM T_Article  WHERE 1=1 AND article_id=@id";
            dynamic d = base.SelectSingle(sql, new { id =id});
            IList<Comment> comments = new List<Comment>();
            if (!string.IsNullOrEmpty(d.article_comments))
                comments = _commentRepository.SelectByIds(d.article_comments.Split(','));
            Article article = new Article(
                d.article_id
                , d.article_title
                , d.article_author
                , d.article_content
                , (ArticleType)d.article_articletype
                , Convert.ToBoolean(d.article_isdraft)
                , comments
                , d.article_createtime
                );
            return article;
        }

        public void Praise(int id,bool cancle)
        {
            string sql = "";
            if(cancle)
                sql = "UPDATE T_Article SET article_praisecount=article_praisecount-1 WHERE article_id=@id";
            else
                sql = "UPDATE T_Article SET article_praisecount=article_praisecount+1 WHERE article_id=@id";
            DbConnection.Execute(sql,new { id });
        }

        public IList<Article> SelectTop(int num, NameValueCollection orderByCollection)
        {
            StringBuilder sb = new StringBuilder("SELECT article_id,article_title,article_articletype FROM T_Article ORDER BY ");
            foreach(string key in orderByCollection.AllKeys)
            {
                sb.Append(key);
                sb.Append(" ");
                sb.Append(orderByCollection[key]);
                sb.Append(" ");
            }
            sb.Append("LIMIT 0,@num");
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("num", num);
            IEnumerable<dynamic> resultList = DbConnection.Query(sb.ToString(),parameters);
            IList<Article> articles = new List<Article>();
            foreach (var d in resultList)
            {

                Article article = new Article(
                  d.article_id
                , d.article_title
                , (ArticleType)d.article_articletype
                );
                articles.Add(article);
            }
            return articles;
        }

        public IList<Article> SelectByTypeMaxTime()
        {
            string sql = "select article_id,article_title,article_author,article_textsection,article_articletype,article_createtime " +
                 "from T_Article a where not exists(select * from T_Article where a.article_createtime<article_createtime and a.article_articletype=article_articletype )";
            IEnumerable<dynamic> resultList = DbConnection.Query(sql);
            IList<Article> articles = new List<Article>();
            foreach(dynamic d in resultList)
            {
                Article article = new Article(
                    d.article_id,
                    d.article_author,
                    d.article_title,
                    d.article_textsection,
                    (ArticleType)d.article_articletype,
                    d.article_createtime
                    );
                articles.Add(article);
            }
            return articles;
        }

        public IList<Article> SelectAllByArticle(int articleId)
        {
            string sql = "SELECT article_id, article_title, article_articletype,article_createtime FROM T_Article " +
                "WHERE article_author = (SELECT article_author FROM T_Article where article_id = @articleId) ORDER BY article_createtime DESC";
             IEnumerable<dynamic> resultList= Select(sql, new { articleId });
            IList<Article> articles = new List<Article>();
            foreach (dynamic d in resultList)
            {
                Article article = new Article(
                    d.article_id,
                    d.article_title,
                    (ArticleType)d.article_articletype,
                    d.article_createtime
                    );
                articles.Add(article);
            }
            return articles;
        }
        public IEnumerable<dynamic> SelectArticleFile(ArticleCondition articleCondition)
        {
            DynamicParameters parameters = new DynamicParameters();
            string where = Where(articleCondition, ref parameters);
            string sql = "SELECT COUNT(*) as count,article_author,article_articletype FROM T_Article WHERE " + where + "   GROUP BY article_author,article_articletype";
            IEnumerable<dynamic> resultList = Select(sql, parameters);
            return resultList;
        }

        public void UpdateBrowserCount(int id)
        {
            string sql = "UPDATE T_Article SET article_browsercount=article_browsercount+1 WHERE article_id=@id";
            DbConnection.Execute(sql, new { id });
        }
    }
}
