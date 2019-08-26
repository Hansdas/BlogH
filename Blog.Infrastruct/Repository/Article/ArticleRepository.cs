using Blog.AOP;
using Blog.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;

namespace Blog.Infrastruct
{
   public class ArticleRepository: Repository<Article, int>,IArticleRepository, IInterceptorHandler
    {
        public void Insert(Article article)
        {
           string sql = "INSERT INTO Article(article_author,article_title,article_textsection, article_content, article_articletype, article_praisecount, article_browsercount, article_isdraft, article_relatedfiles, article_createtime)" +
                  " VALUES (@Author,@Title,@TextSection, @Content, @ArticleType, @PraiseCount,@BrowserCount,@IsDraft,@RelatedFiles, NOW())";
            DbConnection.Execute(sql,article);
            DbConnection.Dispose();
        }
    }
}
