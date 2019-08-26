using System;
using System.Collections.Generic;
using System.Text;
using Blog.Domain;
using Blog.Domain.Core.Bus;

namespace Blog.Application
{
    public class ArticleService : IArticleService
    {
        private readonly IMediatorHandler _mediatorHandler;
        public ArticleService(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }
        public void Publish(Article article)
        {
            try
            {
                var command = new CreateArticleCommand(article);
                _mediatorHandler.SendCommand(command);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
