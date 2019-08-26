using Blog.AOP;
using Blog.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Infrastruct
{
   public class ArticleRepository: Repository<Article, int>,IArticleRepository, IInterceptorHandler
    {
    }
}
